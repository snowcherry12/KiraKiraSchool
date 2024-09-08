using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Item Sprite")]
    [Category("Inventory/Item Sprite")]
    
    [Image(typeof(IconItem), ColorTheme.Type.Green)]
    [Description("Returns the Sprite of the Item")]
    
    [Serializable]
    public class GetSpriteItem : PropertyTypeGetSprite
    {
        [SerializeField] private PropertyGetItem m_Item = new PropertyGetItem();

        public override Sprite Get(Args args) => this.GetSprite(args);

        private Sprite GetSprite(Args args)
        {
            Item item = this.m_Item.Get(args);
            return item != null ? item.Info.Sprite(args) : null;
        }

        public override string String => $"{this.m_Item} Sprite";
    }
}