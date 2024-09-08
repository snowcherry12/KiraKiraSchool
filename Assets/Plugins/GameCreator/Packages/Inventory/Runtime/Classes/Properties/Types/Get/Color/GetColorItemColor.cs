using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Item Color")]
    [Category("Inventory/Item Color")]
    
    [Image(typeof(IconItem), ColorTheme.Type.Green)]
    [Description("Returns the Color of the Item")]
    
    [Serializable]
    public class GetColorItemColor : PropertyTypeGetColor
    {
        [SerializeField] private PropertyGetItem m_Item = new PropertyGetItem();

        public override Color Get(Args args)
        {
            Item item = this.m_Item.Get(args);
            return item != null ? item.Info.Color(args) : default;
        }

        public override string String => $"{this.m_Item} Color";
    }
}