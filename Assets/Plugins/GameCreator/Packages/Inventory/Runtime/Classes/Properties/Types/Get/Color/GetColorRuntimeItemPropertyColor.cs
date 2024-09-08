using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Runtime Item Property Color")]
    [Category("Inventory/Runtime Item Property Color")]
    
    [Image(typeof(IconProperty), ColorTheme.Type.Green)]
    [Description("Returns the property Color of the Runtime Item")]
    
    [Serializable]
    public class GetColorRuntimeItemPropertyColor : PropertyTypeGetColor
    {
        [SerializeField] private PropertyGetRuntimeItem m_RuntimeItem = new PropertyGetRuntimeItem();
        [SerializeField] private IdString m_PropertyId = IdString.EMPTY;

        public override Color Get(Args args) => this.GetColor(args);

        private Color GetColor(Args args)
        {
            RuntimeItem runtimeItem = this.m_RuntimeItem.Get(args);
            if (runtimeItem == null) return default;

            return runtimeItem.Properties.TryGetValue(this.m_PropertyId,
                out RuntimeProperty property)
                ? property.Color
                : default;
        }

        public override string String => $"{this.m_RuntimeItem}[{this.m_PropertyId}] Color";
    }
}