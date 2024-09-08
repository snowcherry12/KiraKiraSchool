using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Stats
{
    [Title("Local List Variable")]
    [Category("Variables/Local List Variable")]
    
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal)]
    [Description("Returns the Attribute value of a Local List Variable")]

    [Serializable]
    public class GetAttributeLocalList : PropertyTypeGetAttribute
    {
        [SerializeField]
        protected FieldGetLocalList m_Variable = new FieldGetLocalList(ValueAttribute.TYPE_ID);

        public override Attribute Get(Args args) => this.m_Variable.Get<Attribute>(args);

        public override string String => this.m_Variable.ToString();
    }
}