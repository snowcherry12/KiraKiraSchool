using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Stats
{
    [Title("Global List Variable")]
    [Category("Variables/Global List Variable")]
    
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal, typeof(OverlayDot))]
    [Description("Returns the Attribute value of a Global List Variable")]

    [Serializable]
    public class GetAttributeGlobalList : PropertyTypeGetAttribute
    {
        [SerializeField]
        protected FieldGetGlobalList m_Variable = new FieldGetGlobalList(ValueAttribute.TYPE_ID);

        public override Attribute Get(Args args) => this.m_Variable.Get<Attribute>(args);

        public override string String => this.m_Variable.ToString();
    }
}