using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Inventory
{
    [Title("Global Name Variable")]
    [Category("Global Name Variable")]
    
    [Description("Sets the Runtime Item value on a Global Name Variable")]
    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple, typeof(OverlayDot))]

    [Serializable] [HideLabelsInEditor]
    public class SetRuntimeItemGlobalName : PropertyTypeSetRuntimeItem
    {
        [SerializeField]
        protected FieldSetGlobalName m_Variable = new FieldSetGlobalName(ValueRuntimeItem.TYPE_ID);

        public override void Set(RuntimeItem value, Args args) => this.m_Variable.Set(value, args);
        public override RuntimeItem Get(Args args) => this.m_Variable.Get(args) as RuntimeItem;

        public static PropertySetRuntimeItem Create => new PropertySetRuntimeItem(
            new SetRuntimeItemGlobalName()
        );
        
        public override string String => this.m_Variable.ToString();
    }
}