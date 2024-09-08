using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class Crafting
    {
        #if UNITY_EDITOR
        
        [UnityEditor.InitializeOnEnterPlayMode]
        private static void InitializeOnEnterPlayMode()
        {
            EventCraft = null;
            EventDismantle = null;

            LastItemAttemptedCraft = null;
            LastItemAttemptedDismantle = null;
            
            LastItemCrafted = null;
            LastItemDismantled = null;
        }
        
        #endif

        public static Item LastItemAttemptedCraft     { get; internal set; }
        public static Item LastItemAttemptedDismantle { get; internal set; }
        
        public static RuntimeItem LastItemCrafted    { get; internal set; }
        public static RuntimeItem LastItemDismantled { get; internal set; }
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private Ingredient[] m_Ingredients = Array.Empty<Ingredient>();

        [SerializeField] private bool m_CanCraft;
        [SerializeField] private RunConditionsList m_ConditionsCraft = new RunConditionsList();
        [SerializeField] private RunInstructionsList m_InstructionsOnCraft = new RunInstructionsList();

        [SerializeField] private bool m_CanDismantle;
        [SerializeField] private RunConditionsList m_ConditionsDismantle = new RunConditionsList();
        [SerializeField] private RunInstructionsList m_InstructionsOnDismantle = new RunInstructionsList();

        // PROPERTIES: ----------------------------------------------------------------------------

        public Ingredient[] Ingredients => this.m_Ingredients;

        public bool AllowToCraft => this.m_CanCraft;
        public bool AllowToDismantle => this.m_CanDismantle;

        // EVENTS: --------------------------------------------------------------------------------

        public static event Action EventCraft;
        public static event Action EventDismantle;

        // CRAFT METHODS: -------------------------------------------------------------------------
        
        public static bool CanCraft(Item item, Bag inputBag, Bag outputBag)
        {
            if (item == null || inputBag == null || outputBag == null) return false;
            if (!item.Crafting.m_CanCraft) return false;

            RunnerConfig config = new RunnerConfig
            {
                Name = $"Can Craft {TextUtils.Humanize(item.name)}",
                Location = new RunnerLocationParent(outputBag.Wearer != null
                    ? outputBag.Wearer.transform
                    : outputBag.transform
                )
            };

            Args args = new Args(inputBag.gameObject, outputBag.gameObject);
            
            bool canCraft = item.Crafting.m_ConditionsCraft.Check(args, config);
            return canCraft && outputBag.Content.CanAddType(item, true);
        }

        public static bool EnoughCraftingIngredients(Item item, Bag inputBag)
        {
            if (item == null || inputBag == null) return false;
            
            int ingredientsLength = item.Crafting.Ingredients.Length;
            for (int i = 0; i < ingredientsLength; ++i)
            {
                Ingredient ingredient = item.Crafting.Ingredients[i];
                if (inputBag.Content.ContainsType(ingredient.Item, ingredient.Amount)) continue;
                
                return false;
            }

            return true;
        }
        
        public static RuntimeItem Craft(Item item, Bag inputBag, Bag outputBag)
        {
            LastItemAttemptedCraft = item;
            
            if (!CanCraft(item, inputBag, outputBag)) return null;
            if (!EnoughCraftingIngredients(item, inputBag)) return null;

            int ingredientsLength = item.Crafting.Ingredients.Length;
            List<RuntimeItem> removeRuntimeItemList = new List<RuntimeItem>();
            
            for (int i = 0; i < ingredientsLength; ++i)
            {
                Ingredient ingredient = item.Crafting.Ingredients[i];
                for (int j = 0; j < ingredient.Amount; ++j)
                {
                    RuntimeItem removeRuntimeItem = inputBag.Content.RemoveType(ingredient.Item);
                    if (removeRuntimeItem != null)
                    {
                        removeRuntimeItemList.Add(removeRuntimeItem);
                        continue;
                    }

                    foreach (RuntimeItem restoreRuntimeItem in removeRuntimeItemList)
                    {
                        inputBag.Content.Add(restoreRuntimeItem, true);
                    }
                    
                    return null;
                }
            }

            RuntimeItem craftRuntimeItem = outputBag.Content.AddType(item, true);
            if (craftRuntimeItem == null)
            {
                foreach (RuntimeItem restoreRuntimeItem in removeRuntimeItemList)
                {
                    inputBag.Content.Add(restoreRuntimeItem, true);
                }

                return null;
            }

            LastItemCrafted = craftRuntimeItem;
            
            try
            {
                RunnerConfig config = new RunnerConfig
                {
                    Name = $"On Craft {TextUtils.Humanize(item.name)}",
                    Location = new RunnerLocationParent(outputBag.Wearer != null
                        ? outputBag.Wearer.transform
                        : outputBag.transform
                    )
                };

                Args args = new Args(inputBag.gameObject, outputBag.gameObject);
                _ = item.Crafting.m_InstructionsOnCraft.Run(args, config);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.ToString(), inputBag.gameObject);
            }

            EventCraft?.Invoke();
            return craftRuntimeItem;
        }

        // DISMANTLE METHODS: ---------------------------------------------------------------------

        public static bool CanDismantle(Item item, Bag inputBag, Bag outputBag)
        {
            if (item == null || inputBag == null || outputBag == null) return false;
            if (!inputBag.Content.ContainsType(item, 1)) return false;
            if (!item.Crafting.m_CanDismantle) return false;
            
            RunnerConfig config = new RunnerConfig
            {
                Name = $"Can Dismantle {TextUtils.Humanize(item.name)}",
                Location = new RunnerLocationParent(outputBag.Wearer != null
                    ? outputBag.Wearer.transform
                    : outputBag.transform
                )
            };

            Args args = new Args(inputBag.gameObject, outputBag.gameObject);
            bool canDismantle = item.Crafting.m_ConditionsDismantle.Check(args, config);

            return canDismantle;
        }
        
        public static RuntimeItem[] Dismantle(Item item, Bag inputBag, Bag outputBag, float chance)
        {
            LastItemAttemptedDismantle = item;
            if (!CanDismantle(item, inputBag, outputBag)) return null;
            
            RuntimeItem removeRuntimeItem = inputBag.Content.RemoveType(item);
            return removeRuntimeItem != null 
                ? ProcessDismantle(removeRuntimeItem, inputBag, outputBag, chance) 
                : null;
        }
        
        public static RuntimeItem[] Dismantle(RuntimeItem runtimeItem, Bag inputBag, Bag outputBag, float chance)
        {
            LastItemAttemptedDismantle = runtimeItem?.Item;
            if (!CanDismantle(runtimeItem?.Item, inputBag, outputBag)) return null;
            
            RuntimeItem removeRuntimeItem = inputBag.Content.Remove(runtimeItem);
            return ProcessDismantle(removeRuntimeItem, inputBag, outputBag, chance);
        }
        
        private static RuntimeItem[] ProcessDismantle(RuntimeItem removedItem, Bag inputBag, Bag outputBag, float chance)
        {
            if (removedItem?.Item == null) return null;
            
            int ingredientsLength = removedItem.Item.Crafting.Ingredients.Length;
            List<RuntimeItem> dismantleRuntimeItems = new List<RuntimeItem>();
            
            for (int i = 0; i < ingredientsLength; ++i)
            {
                Ingredient ingredient = removedItem.Item.Crafting.Ingredients[i];
                for (int j = 0; j < ingredient.Amount; ++j)
                {
                    float randomChance = UnityEngine.Random.value;
                    if (randomChance > chance) continue;
                    
                    RuntimeItem dismantleRuntimeItem = outputBag
                        .Content.AddType(ingredient.Item, true);
                    
                    if (dismantleRuntimeItem != null)
                    {
                        dismantleRuntimeItems.Add(dismantleRuntimeItem);
                    }
                }
            }
            
            LastItemDismantled = removedItem;
            
            try
            {
                RunnerConfig config = new RunnerConfig
                {
                    Name = $"On Dismantle {TextUtils.Humanize(removedItem.Item.name)}",
                    Location = new RunnerLocationParent(outputBag.Wearer != null
                        ? outputBag.Wearer.transform
                        : outputBag.transform
                    )
                };

                Args args = new Args(inputBag.gameObject, outputBag.gameObject);
                _ = removedItem.Item.Crafting.m_InstructionsOnDismantle.Run(args, config);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.ToString(), inputBag.gameObject);
            }

            EventDismantle?.Invoke();
            return dismantleRuntimeItems.ToArray();
        }
    }
}