using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Inventory
{
    [Title("Global List Variable")]
    [Category("Global List Variable")]
    
    [Description("Sets the Runtime Item instance on a Global List Variable")]
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal, typeof(OverlayDot))]

    [Serializable] [HideLabelsInEditor]
    public class SetRuntimeItemGlobalList : PropertyTypeSetRuntimeItem
    {
        [SerializeField]
        protected FieldSetGlobalList m_Variable = new FieldSetGlobalList(ValueRuntimeItem.TYPE_ID);

        public override void Set(RuntimeItem value, Args args) => this.m_Variable.Set(value, args);
        public override RuntimeItem Get(Args args) => this.m_Variable.Get(args) as RuntimeItem;

        public static PropertySetRuntimeItem Create => new PropertySetRuntimeItem(
            new SetRuntimeItemGlobalList()
        );
        
        public override string String => this.m_Variable.ToString();
    }
}