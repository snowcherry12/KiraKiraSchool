using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Runtime Item Description")]
    [Category("Inventory/Runtime Item Description")]
    
    [Image(typeof(IconItem), ColorTheme.Type.Blue)]
    [Description("Returns the description of the Runtime Item")]
    
    [Serializable]
    public class GetStringRuntimeItemDescription : PropertyTypeGetString
    {
        [SerializeField] private PropertyGetRuntimeItem m_RuntimeItem = new PropertyGetRuntimeItem();

        public override string Get(Args args) => this.GetName(args);

        private string GetName(Args args)
        {
            RuntimeItem runtimeItem = this.m_RuntimeItem.Get(args);
            return runtimeItem != null && runtimeItem.Item != null 
                ? runtimeItem.Item.Info.Description(args)
                : string.Empty;
        }

        public override string String => $"{this.m_RuntimeItem}'s Description";
    }
}