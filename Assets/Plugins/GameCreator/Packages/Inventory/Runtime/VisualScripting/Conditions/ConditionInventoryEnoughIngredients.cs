using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Enough Ingredients")]
    [Description("Returns true if the item can be crafted")]

    [Category("Inventory/Tinker/Enough Ingredients")]
    
    [Parameter("From Bag", "The Bag where ingredients are picked")]
    [Parameter("Item", "The item type attempted to craft")]

    [Keywords("Inventory", "Create", "Make", "Cook", "Smith", "Combine", "Assemble")]
    
    [Image(typeof(IconCraft), ColorTheme.Type.Teal)]
    [Serializable]
    public class ConditionInventoryEnoughIngredients : Condition
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_FromBag = GetGameObjectPlayer.Create();
        [SerializeField] private PropertyGetItem m_Item = new PropertyGetItem();

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"enough Ingredients for {this.m_Item} in {this.m_FromBag}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Bag fromBag = this.m_FromBag.Get<Bag>(args);
            Item item = this.m_Item.Get(args);
            
            return fromBag != null && item != null &&
                   Crafting.EnoughCraftingIngredients(item, fromBag);
        }
    }
}
