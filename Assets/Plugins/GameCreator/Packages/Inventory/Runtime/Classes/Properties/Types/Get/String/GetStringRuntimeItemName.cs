using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Runtime Item Name")]
    [Category("Inventory/Runtime Item Name")]
    
    [Image(typeof(IconItem), ColorTheme.Type.Blue)]
    [Description("Returns the description of the Runtime Item")]
    
    [Serializable]
    public class GetStringRuntimeItemName : PropertyTypeGetString
    {
        [SerializeField] private PropertyGetRuntimeItem m_RuntimeItem = new PropertyGetRuntimeItem();

        public override string Get(Args args) => this.GetName(args);

        private string GetName(Args args)
        {
            RuntimeItem runtimeItem = this.m_RuntimeItem.Get(args);
            return runtimeItem != null && runtimeItem.Item != null 
                ? runtimeItem.Item.Info.Name(args)
                : string.Empty;
        }

        public override string String => $"{this.m_RuntimeItem}'s Name";
    }
}