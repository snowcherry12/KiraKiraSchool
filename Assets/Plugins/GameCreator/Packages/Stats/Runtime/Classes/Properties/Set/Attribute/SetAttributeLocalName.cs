using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Stats
{
    [Title("Local Name Variable")]
    [Category("Variables/Local Name Variable")]
    
    [Description("Sets the Attribute value of a Local Name Variable")]
    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple)]

    [Serializable]
    public class SetAttributeLocalName : PropertyTypeSetAttribute
    {
        [SerializeField]
        protected FieldSetLocalName m_Variable = new FieldSetLocalName(ValueAttribute.TYPE_ID);

        public override void Set(Attribute value, Args args) => this.m_Variable.Set(value, args);
        public override Attribute Get(Args args) => this.m_Variable.Get(args) as Attribute;

        public static PropertySetAttribute Create => new PropertySetAttribute(
            new SetAttributeLocalName()
        );
        
        public override string String => this.m_Variable.ToString();
    }
}