using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Stats
{
    [Title("Global Name Variable")]
    [Category("Variables/Global Name Variable")]
    
    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple, typeof(OverlayDot))]
    [Description("Returns the Attribute value of a Global Name Variable")]

    [Serializable]
    public class GetAttributeGlobalName : PropertyTypeGetAttribute
    {
        [SerializeField]
        protected FieldGetGlobalName m_Variable = new FieldGetGlobalName(ValueAttribute.TYPE_ID);

        public override Attribute Get(Args args) => this.m_Variable.Get<Attribute>(args);

        public override string String => this.m_Variable.ToString();
    }
}