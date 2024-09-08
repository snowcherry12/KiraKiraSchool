using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Item Property Color")]
    [Category("Inventory/Item Property Color")]
    
    [Image(typeof(IconProperty), ColorTheme.Type.Green)]
    [Description("Returns the property Color of the Item")]
    
    [Serializable]
    public class GetColorItemPropertyColor : PropertyTypeGetColor
    {
        [SerializeField] private PropertyGetItem m_Item = new PropertyGetItem();
        [SerializeField] private IdString m_PropertyId = IdString.EMPTY;

        public override Color Get(Args args) => this.GetColor(args);

        private Color GetColor(Args args)
        {
            Item item = this.m_Item.Get(args);
            if (item == null) return default;

            Property property = item.Properties.Get(this.m_PropertyId, item);
            return property?.Color ?? default;
        }

        public override string String => $"{this.m_Item}[{this.m_PropertyId}] Color";
    }
}