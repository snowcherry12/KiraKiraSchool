using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Runtime Item Color")]
    [Category("Inventory/Runtime Item Color")]
    
    [Image(typeof(IconItem), ColorTheme.Type.Green)]
    [Description("Returns the Color of the Runtime Item")]
    
    [Serializable]
    public class GetColorRuntimeItemColor : PropertyTypeGetColor
    {
        [SerializeField] private PropertyGetRuntimeItem m_RuntimeItem = new PropertyGetRuntimeItem();

        public override Color Get(Args args)
        {
            RuntimeItem runtimeItem = this.m_RuntimeItem.Get(args);
            return runtimeItem?.Item.Info.Color(args) ?? default;
        }

        public override string String => $"{this.m_RuntimeItem} Color";
    }
}