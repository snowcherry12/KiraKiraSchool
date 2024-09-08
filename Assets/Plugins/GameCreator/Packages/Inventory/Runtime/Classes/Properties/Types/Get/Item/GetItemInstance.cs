using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Item")]
    [Category("Item")]
    
    [Image(typeof(IconItem), ColorTheme.Type.Green)]
    [Description("A reference to an Item asset")]

    [Serializable] [HideLabelsInEditor]
    public class GetItemInstance : PropertyTypeGetItem
    {
        [SerializeField] protected Item m_Item;

        public override Item Get(Args args) => this.m_Item;
        public override Item Get(GameObject gameObject) => this.m_Item;

        public static PropertyGetItem Create(Item item = null)
        {
            GetItemInstance instance = new GetItemInstance
            {
                m_Item = item
            };
            
            return new PropertyGetItem(instance);
        }

        public override string String => this.m_Item != null
            ? this.m_Item.name
            : "(none)";
    }
}