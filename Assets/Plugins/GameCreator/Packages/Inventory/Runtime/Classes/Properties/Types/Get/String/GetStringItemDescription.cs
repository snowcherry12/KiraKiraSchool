using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Item Description")]
    [Category("Inventory/Item Description")]
    
    [Image(typeof(IconItem), ColorTheme.Type.Green)]
    [Description("Returns the description of the Item")]
    
    [Serializable]
    public class GetStringItemDescription : PropertyTypeGetString
    {
        [SerializeField] private PropertyGetItem m_Item = new PropertyGetItem();

        public override string Get(Args args) => this.GetName(args);

        private string GetName(Args args)
        {
            Item item = this.m_Item.Get(args);
            return item != null ? item.Info.Description(args) : string.Empty;
        }

        public override string String => $"{this.m_Item}'s Description";
    }
}