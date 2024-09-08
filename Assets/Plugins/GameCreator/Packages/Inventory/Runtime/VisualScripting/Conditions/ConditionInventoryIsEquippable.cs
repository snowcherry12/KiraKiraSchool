using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Is Equippable")]
    [Description("Returns true if the chosen Item can be equipped")]

    [Category("Inventory/Equipment/Is Equippable")]
    
    [Parameter("Item", "The item type to check")]
    [Keywords("Inventory", "Wear", "Equip")]
    
    [Image(typeof(IconEquipment), ColorTheme.Type.Green)]
    
    [Serializable]
    public class ConditionInventoryIsEquippable : Condition
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetItem m_Item = new PropertyGetItem();

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"is {this.m_Item} equippable";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Item item = this.m_Item.Get(args);
            return item != null && item.Equip.IsEquippable;
        }
    }
}
