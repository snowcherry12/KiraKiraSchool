using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Property Last Item Equipped")]
    [Category("Inventory/Equipment/Property Last Item Equipped")]
    
    [Image(typeof(IconEquipment), ColorTheme.Type.Blue)]
    [Description("The numeric Property value of the last Item equipped")]

    [Parameter("Item Property", "The property ID of the item")]
    
    [Keywords("Float", "Decimal", "Double")]
    [Keywords("Wear", "Equip", "Unequip", "Value", "Hold")]
    
    [Serializable] [HideLabelsInEditor]
    public class GetDecimalInventoryLastItemEquipProperty : PropertyTypeGetDecimal
    {
        [SerializeField] protected IdString m_ItemProperty;

        public override double Get(Args args) => this.GetValue();
        public override double Get(GameObject gameObject) => this.GetValue();

        private float GetValue()
        {
            if (RuntimeItem.Bag_LastItemEquipped == null) return 0f;
            
            RuntimeProperty property = RuntimeItem
                .Bag_LastItemEquipped
                .Properties[this.m_ItemProperty];

            return property?.Number ?? 0f;
        }

        public static PropertyGetDecimal Create => new PropertyGetDecimal(
            new GetDecimalInventoryLastItemEquipProperty()
        );

        public override string String => $"Item:Equip[{this.m_ItemProperty.String}]";
    }
}