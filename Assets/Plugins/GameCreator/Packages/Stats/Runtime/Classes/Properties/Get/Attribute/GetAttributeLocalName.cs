using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Stats
{
    [Title("Local Name Variable")]
    [Category("Variables/Local Name Variable")]

    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple)]
    [Description("Returns the Attribute value of a Local Name Variable")]
    
    [Serializable]
    public class GetAttributeLocalName : PropertyTypeGetAttribute
    {
        [SerializeField]
        protected FieldGetLocalName m_Variable = new FieldGetLocalName(ValueAttribute.TYPE_ID);

        public override Attribute Get(Args args) => this.m_Variable.Get<Attribute>(args);

        public override string String => this.m_Variable.ToString();
    }
}