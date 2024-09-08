using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Runtime Item Property Value")]
    [Category("Inventory/Runtime Item Property Value")]
    
    [Image(typeof(IconProperty), ColorTheme.Type.Blue)]
    [Description("Returns the property Value of the Runtime Item")]
    
    [Serializable]
    public class GetStringRuntimeItemPropertyValue : PropertyTypeGetString
    {
        [SerializeField] private PropertyGetRuntimeItem m_RuntimeItem = new PropertyGetRuntimeItem();
        [SerializeField] private IdString m_PropertyId = IdString.EMPTY;

        public override string Get(Args args) => this.GetValue(args);

        private string GetValue(Args args)
        {
            RuntimeItem runtimeItem = this.m_RuntimeItem.Get(args);
            if (runtimeItem == null) return string.Empty;

            return runtimeItem.Properties.TryGetValue(this.m_PropertyId, out RuntimeProperty prop)
                ? prop.Number.ToString("G")
                : string.Empty;
        }

        public override string String => $"{this.m_RuntimeItem}[{this.m_PropertyId}] Value";
    }
}