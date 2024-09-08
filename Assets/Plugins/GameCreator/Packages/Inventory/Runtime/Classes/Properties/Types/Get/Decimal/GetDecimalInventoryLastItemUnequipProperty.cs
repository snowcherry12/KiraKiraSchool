using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Property Last Item Unequipped")]
    [Category("Inventory/Equipment/Property Last Item Unequipped")]
    
    [Image(typeof(IconEquipment), ColorTheme.Type.Red)]
    [Description("The numeric Property value of the last Item unequipped")]

    [Parameter("Item Property", "The property ID of the item")]
    
    [Keywords("Float", "Decimal", "Double")]
    [Keywords("Wear", "Equip", "Unequip", "Value", "Hold")]
    
    [Serializable] [HideLabelsInEditor]
    public class GetDecimalInventoryLastItemUnequipProperty : PropertyTypeGetDecimal
    {
        [SerializeField] protected IdString m_ItemProperty;

        public override double Get(Args args) => this.GetValue();
        public override double Get(GameObject gameObject) => this.GetValue();

        private float GetValue()
        {
            if (RuntimeItem.Bag_LastItemUnequipped == null) return 0f;
            
            RuntimeProperty property = RuntimeItem
                .Bag_LastItemUnequipped
                .Properties[this.m_ItemProperty];

            return property?.Number ?? 0f;
        }

        public static PropertyGetDecimal Create => new PropertyGetDecimal(
            new GetDecimalInventoryLastItemUnequipProperty()
        );

        public override string String => $"Item:Unequip[{this.m_ItemProperty.String}]";
    }
}