using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Property Last Item Used")]
    [Category("Inventory/Property Last Item Used")]
    
    [Image(typeof(IconItem), ColorTheme.Type.Green)]
    [Description("The numeric Property value of the last Item used")]

    [Parameter("Item Property", "The property ID of the item")]
    
    [Keywords("Float", "Decimal", "Double")]
    [Keywords("Use", "Consume", "Drink")]
    
    [Serializable] [HideLabelsInEditor]
    public class GetDecimalInventoryLastItemUsedProperty : PropertyTypeGetDecimal
    {
        [SerializeField] protected IdString m_ItemProperty;

        public override double Get(Args args) => this.GetValue();
        public override double Get(GameObject gameObject) => this.GetValue();

        private float GetValue()
        {
            if (RuntimeItem.Bag_LastItemUsed == null) return 0f;
            
            RuntimeProperty property = RuntimeItem
                .Bag_LastItemUsed
                .Properties[this.m_ItemProperty];

            return property?.Number ?? 0f;
        }

        public static PropertyGetDecimal Create => new PropertyGetDecimal(
            new GetDecimalInventoryLastItemUsedProperty()
        );

        public override string String => $"Item:Use[{this.m_ItemProperty.String}]";
    }
}