using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Property Last Attachment Attached")]
    [Category("Inventory/Sockets/Property Last Attachment Attached")]
    
    [Image(typeof(IconSocket), ColorTheme.Type.Yellow, typeof(OverlayPlus))]
    [Description("The numeric Property value of the last Item attached to a Socket")]

    [Parameter("Item Property", "The property ID of the item")]
    
    [Keywords("Float", "Decimal", "Double")]
    [Keywords("Attach", "Embed", "Enchant", "Socket")]
    
    [Serializable] [HideLabelsInEditor]
    public class GetDecimalInventoryLastAttachmentAttached : PropertyTypeGetDecimal
    {
        [SerializeField] protected IdString m_ItemProperty;

        public override double Get(Args args) => this.GetValue();
        public override double Get(GameObject gameObject) => this.GetValue();

        private float GetValue()
        {
            if (RuntimeItem.Socket_LastAttachmentAttached == null) return 0f;
            
            RuntimeProperty property = RuntimeItem
                .Socket_LastAttachmentAttached
                .Properties[this.m_ItemProperty];

            return property?.Number ?? 0f;
        }

        public static PropertyGetDecimal Create => new PropertyGetDecimal(
            new GetDecimalInventoryLastAttachmentAttached()
        );

        public override string String => $"Attachment:Attached[{this.m_ItemProperty.String}]";
    }
}