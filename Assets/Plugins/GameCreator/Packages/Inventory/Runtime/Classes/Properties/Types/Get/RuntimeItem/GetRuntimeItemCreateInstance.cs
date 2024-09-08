using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Create Runtime Item")]
    [Category("Create Runtime Item")]
    
    [Image(typeof(IconItem), ColorTheme.Type.Green)]
    [Description("A new runtime instance from an Item asset")]

    [Serializable] [HideLabelsInEditor]
    public class GetRuntimeItemCreateInstance : PropertyTypeGetRuntimeItem
    {
        [SerializeField] protected PropertyGetItem m_Item = GetItemInstance.Create();

        public override RuntimeItem Get(Args args)
        {
            Item item = this.m_Item.Get(args);
            return item != null ? item.CreateRuntimeItem(args) : null;
        }

        public static PropertyGetRuntimeItem Create(Item item = null)
        {
            GetRuntimeItemCreateInstance instance = new GetRuntimeItemCreateInstance
            {
                m_Item = GetItemInstance.Create(item)
            };
            
            return new PropertyGetRuntimeItem(instance);
        }

        public override string String => $"new {this.m_Item}";
    }
}