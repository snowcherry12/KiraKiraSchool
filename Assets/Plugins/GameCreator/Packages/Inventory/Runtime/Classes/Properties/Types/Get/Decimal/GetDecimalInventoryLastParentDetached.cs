using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Property Last Parent Detached")]
    [Category("Inventory/Sockets/Property Last Parent Detached")]
    
    [Image(typeof(IconSocket), ColorTheme.Type.Blue, typeof(OverlayMinus))]
    [Description("The numeric Property value of the last Item that one of its Sockets was detached")]

    [Parameter("Item Property", "The property ID of the item")]
    
    [Keywords("Float", "Decimal", "Double")]
    [Keywords("Detach", "Embed", "Disenchant", "Socket")]
    
    [Serializable] [HideLabelsInEditor]
    public class GetDecimalInventoryLastParentDetached : PropertyTypeGetDecimal
    {
        [SerializeField] protected IdString m_ItemProperty;

        public override double Get(Args args) => this.GetValue();
        public override double Get(GameObject gameObject) => this.GetValue();

        private float GetValue()
        {
            if (RuntimeItem.Socket_LastParentDetached == null) return 0f;
            
            RuntimeProperty property = RuntimeItem
                .Socket_LastParentDetached
                .Properties[this.m_ItemProperty];

            return property?.Number ?? 0f;
        }

        public static PropertyGetDecimal Create => new PropertyGetDecimal(
            new GetDecimalInventoryLastParentDetached()
        );

        public override string String => $"Parent:Detached[{this.m_ItemProperty.String}]";
    }
}