using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Inventory
{
    [Title("Global Name Variable")]
    [Category("Global Name Variable")]
    
    [Description("Sets the Item value on a Global Name Variable")]
    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple, typeof(OverlayDot))]

    [Serializable] [HideLabelsInEditor]
    public class SetItemGlobalName : PropertyTypeSetItem
    {
        [SerializeField]
        protected FieldSetGlobalName m_Variable = new FieldSetGlobalName(ValueItem.TYPE_ID);

        public override void Set(Item value, Args args) => this.m_Variable.Set(value, args);
        public override Item Get(Args args) => this.m_Variable.Get(args) as Item;

        public static PropertySetItem Create => new PropertySetItem(
            new SetItemGlobalName()
        );
        
        public override string String => this.m_Variable.ToString();
    }
}