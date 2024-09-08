using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Stats
{
    [Title("Global List Variable")]
    [Category("Variables/Global List Variable")]
    
    [Description("Sets the Attribute value of a Global List Variable")]
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal, typeof(OverlayDot))]

    [Serializable]
    public class SetAttributeGlobalList : PropertyTypeSetAttribute
    {
        [SerializeField]
        protected FieldSetGlobalList m_Variable = new FieldSetGlobalList(ValueAttribute.TYPE_ID);

        public override void Set(Attribute value, Args args) => this.m_Variable.Set(value, args);
        public override Attribute Get(Args args) => this.m_Variable.Get(args) as Attribute;

        public static PropertySetAttribute Create => new PropertySetAttribute(
            new SetAttributeGlobalList()
        );
        
        public override string String => this.m_Variable.ToString();
    }
}