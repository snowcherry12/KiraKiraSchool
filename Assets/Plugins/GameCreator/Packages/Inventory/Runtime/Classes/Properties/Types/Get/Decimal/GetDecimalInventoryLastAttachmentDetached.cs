using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Property Last Attachment Detached")]
    [Category("Inventory/Sockets/Property Last Attachment Detached")]
    
    [Image(typeof(IconSocket), ColorTheme.Type.Yellow, typeof(OverlayMinus))]
    [Description("The numeric Property value of the last Item detached from a Socket")]

    [Parameter("Item Property", "The property ID of the item")]
    
    [Keywords("Float", "Decimal", "Double")]
    [Keywords("Detach", "Embed", "Disenchant", "Socket")]
    
    [Serializable] [HideLabelsInEditor]
    public class GetDecimalInventoryLastAttachmentDetached : PropertyTypeGetDecimal
    {
        [SerializeField] protected IdString m_ItemProperty;

        public override double Get(Args args) => this.GetValue();
        public override double Get(GameObject gameObject) => this.GetValue();

        private float GetValue()
        {
            if (RuntimeItem.Socket_LastAttachmentDetached == null) return 0f;
            
            RuntimeProperty property = RuntimeItem
                .Socket_LastAttachmentDetached
                .Properties[this.m_ItemProperty];

            return property?.Number ?? 0f;
        }

        public static PropertyGetDecimal Create => new PropertyGetDecimal(
            new GetDecimalInventoryLastAttachmentDetached()
        );

        public override string String => $"Attachment:Detached[{this.m_ItemProperty.String}]";
    }
}