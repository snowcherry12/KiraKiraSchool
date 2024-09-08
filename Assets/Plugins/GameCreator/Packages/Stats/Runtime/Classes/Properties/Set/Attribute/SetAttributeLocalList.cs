using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Stats
{
    [Title("Local List Variable")]
    [Category("Variables/Local List Variable")]
    
    [Description("Sets the Attribute value of a Local List Variable")]
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal)]

    [Serializable]
    public class SetAttributeLocalList : PropertyTypeSetAttribute
    {
        [SerializeField]
        protected FieldSetLocalList m_Variable = new FieldSetLocalList(ValueAttribute.TYPE_ID);

        public override void Set(Attribute value, Args args) => this.m_Variable.Set(value, args);
        public override Attribute Get(Args args) => this.m_Variable.Get(args) as Attribute;

        public static PropertySetAttribute Create => new PropertySetAttribute(
            new SetAttributeLocalList()
        );
        
        public override string String => this.m_Variable.ToString();
    }
}