using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Can Craft")]
    [Description("Returns true if the item can be crafted")]

    [Category("Inventory/Tinker/Can Craft")]
    
    [Parameter("From Bag", "The Bag where ingredients are picked")]
    [Parameter("Item", "The item type attempted to craft")]
    [Parameter("To Bag", "The target destination Bag after creating the new Item")]

    [Keywords("Inventory", "Create", "Make", "Cook", "Smith", "Combine", "Assemble")]
    
    [Image(typeof(IconCraft), ColorTheme.Type.Teal)]
    [Serializable]
    public class ConditionInventoryCanCraft : Condition
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_FromBag = GetGameObjectPlayer.Create();
        [SerializeField] private PropertyGetItem m_Item = new PropertyGetItem();
        [SerializeField] private PropertyGetGameObject m_ToBag = GetGameObjectPlayer.Create();

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"can craft {this.m_Item} from {this.m_FromBag}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Bag fromBag = this.m_FromBag.Get<Bag>(args);
            Item item = this.m_Item.Get(args);
            Bag toBag = this.m_ToBag.Get<Bag>(args);

            if (fromBag == null) return false;
            if (item == null) return false;
            if (toBag == null) return false;

            bool canCraft = Crafting.CanCraft(item, fromBag, toBag);
            bool enoughIngredients = Crafting.EnoughCraftingIngredients(item, fromBag);
            
            return canCraft && enoughIngredients;
        }
    }
}
