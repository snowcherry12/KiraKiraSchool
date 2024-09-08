using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [Serializable]
    public class TinkerCraftingUI : TTinkerUI
    {
        [SerializeField] private CraftingItemUI m_SelectedUI;
        [FormerlySerializedAs("m_CraftingRecipesContent")] 
        [SerializeField] private RectTransform m_CraftingContent;
        [SerializeField] private GameObject m_CraftingItemUIPrefab;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        protected override TTinkerItemUI SelectedUI => this.m_SelectedUI;
        protected override RectTransform Content => this.m_CraftingContent;
        protected override GameObject Prefab => this.m_CraftingItemUIPrefab;
        
        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected override List<RuntimeItem> GetList(TinkerUI tinkerUI)
        {
            Item[] catalogue = Settings.From<InventoryRepository>().Items.List;
            List<RuntimeItem> items = new List<RuntimeItem>();
            
            foreach (Item item in catalogue)
            {
                if (!item.Crafting.AllowToCraft) continue;
                if (!Crafting.CanCraft(item, tinkerUI.InputBag, tinkerUI.OutputBag)) continue;

                RuntimeItem dummyRuntimeItem = new RuntimeItem(item);
                items.Add(dummyRuntimeItem);
            }

            return items;
        }

        protected override void RefreshItemUI(TinkerUI tinkerUI, Transform child, RuntimeItem runtimeItem)
        {
            CraftingItemUI craftingUI = child.Get<CraftingItemUI>();
            if (craftingUI == null) return;
            
            craftingUI.RefreshUI(tinkerUI, runtimeItem);
        }

        protected override bool ValidSelection(TinkerUI tinkerUI)
        {
            return tinkerUI.Selection is CraftingItemUI;
        }
    }
}