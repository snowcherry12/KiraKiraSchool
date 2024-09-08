using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Stats
{
    [Title("Global Name Variable")]
    [Category("Variables/Global Name Variable")]
    
    [Description("Sets the Attribute value of a Global Name Variable")]
    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple, typeof(OverlayDot))]

    [Serializable]
    public class SetAttributeGlobalName : PropertyTypeSetAttribute
    {
        [SerializeField]
        protected FieldSetGlobalName m_Variable = new FieldSetGlobalName(ValueAttribute.TYPE_ID);

        public override void Set(Attribute value, Args args) => this.m_Variable.Set(value, args);
        public override Attribute Get(Args args) => this.m_Variable.Get(args) as Attribute;

        public static PropertySetAttribute Create => new PropertySetAttribute(
            new SetAttributeGlobalName()
        );
        
        public override string String => this.m_Variable.ToString();
    }
}