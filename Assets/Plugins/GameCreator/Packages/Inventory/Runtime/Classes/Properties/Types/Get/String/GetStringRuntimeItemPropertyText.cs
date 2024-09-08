using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Runtime Item Property Text")]
    [Category("Inventory/Runtime Item Property Text")]
    
    [Image(typeof(IconProperty), ColorTheme.Type.Blue)]
    [Description("Returns the property Text of the Runtime Item")]
    
    [Serializable]
    public class GetStringRuntimeItemPropertyText : PropertyTypeGetString
    {
        [SerializeField] private PropertyGetRuntimeItem m_RuntimeItem = new PropertyGetRuntimeItem();
        [SerializeField] private IdString m_PropertyId = IdString.EMPTY;

        public override string Get(Args args) => this.GetText(args);

        private string GetText(Args args)
        {
            RuntimeItem runtimeItem = this.m_RuntimeItem.Get(args);
            if (runtimeItem == null) return string.Empty;

            return runtimeItem.Properties.TryGetValue(this.m_PropertyId, out RuntimeProperty prop)
                ? prop.Text
                : string.Empty;
        }

        public override string String => $"{this.m_RuntimeItem}[{this.m_PropertyId}] Text";
    }
}