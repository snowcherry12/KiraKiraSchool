using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Is Craftable")]
    [Description("Returns true if the chosen Item can be crafted")]

    [Category("Inventory/Tinker/Is Craftable")]
    
    [Parameter("Item", "The item type to check")]
    [Keywords("Inventory", "Create", "Forge", "Alchemy", "Brew")]
    
    [Image(typeof(IconCraft), ColorTheme.Type.Green)]
    
    [Serializable]
    public class ConditionInventoryIsCraftable : Condition
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetItem m_Item = new PropertyGetItem();

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"is {this.m_Item} craftable";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Item item = this.m_Item.Get(args);
            return item != null && item.Crafting.AllowToCraft;
        }
    }
}
