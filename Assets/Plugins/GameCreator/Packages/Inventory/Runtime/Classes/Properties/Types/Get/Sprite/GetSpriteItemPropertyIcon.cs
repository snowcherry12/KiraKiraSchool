using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Item Property Sprite")]
    [Category("Inventory/Item Property Sprite")]
    
    [Image(typeof(IconProperty), ColorTheme.Type.Green)]
    [Description("Returns the property Sprite of the Item")]
    
    [Serializable]
    public class GetSpriteItemPropertyIcon : PropertyTypeGetSprite
    {
        [SerializeField] private PropertyGetItem m_Item = new PropertyGetItem();
        [SerializeField] private IdString m_PropertyId = IdString.EMPTY;

        public override Sprite Get(Args args) => this.GetSprite(args);

        private Sprite GetSprite(Args args)
        {
            Item item = this.m_Item.Get(args);
            if (item == null) return null;

            Property property = item.Properties.Get(this.m_PropertyId, item);
            return property?.Icon != null ? property.Icon : null;
        }

        public override string String => $"{this.m_Item}[{this.m_PropertyId}] Sprite";
    }
}