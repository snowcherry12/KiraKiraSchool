using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Inventory
{
    [Title("Global List Variable")]
    [Category("Global List Variable")]
    
    [Description("Sets the Item value on a Global List Variable")]
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal, typeof(OverlayDot))]

    [Serializable] [HideLabelsInEditor]
    public class SetItemGlobalList : PropertyTypeSetItem
    {
        [SerializeField]
        protected FieldSetGlobalList m_Variable = new FieldSetGlobalList(ValueItem.TYPE_ID);

        public override void Set(Item value, Args args) => this.m_Variable.Set(value, args);
        public override Item Get(Args args) => this.m_Variable.Get(args) as Item;

        public static PropertySetItem Create => new PropertySetItem(
            new SetItemGlobalList()
        );
        
        public override string String => this.m_Variable.ToString();
    }
}