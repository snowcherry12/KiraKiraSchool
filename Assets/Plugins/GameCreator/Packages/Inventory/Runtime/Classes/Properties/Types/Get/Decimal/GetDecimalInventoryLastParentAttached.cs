using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Property Last Parent Attached")]
    [Category("Inventory/Sockets/Property Last Parent Attached")]
    
    [Image(typeof(IconSocket), ColorTheme.Type.Blue, typeof(OverlayPlus))]
    [Description("The numeric Property value of the last Item that one of its Sockets was attached")]

    [Parameter("Item Property", "The property ID of the item")]
    
    [Keywords("Float", "Decimal", "Double")]
    [Keywords("Attach", "Embed", "Enchant", "Socket")]
    
    [Serializable] [HideLabelsInEditor]
    public class GetDecimalInventoryLastParentAttached : PropertyTypeGetDecimal
    {
        [SerializeField] protected IdString m_ItemProperty;

        public override double Get(Args args) => this.GetValue();
        public override double Get(GameObject gameObject) => this.GetValue();

        private float GetValue()
        {
            if (RuntimeItem.Socket_LastParentAttached == null) return 0f;
            
            RuntimeProperty property = RuntimeItem
                .Socket_LastParentAttached
                .Properties[this.m_ItemProperty];

            return property?.Number ?? 0f;
        }

        public static PropertyGetDecimal Create => new PropertyGetDecimal(
            new GetDecimalInventoryLastParentAttached()
        );

        public override string String => $"Parent:Attached[{this.m_ItemProperty.String}]";
    }
}