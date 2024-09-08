using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Stats
{
    [Title("Global Name Variable")]
    [Category("Variables/Global Name Variable")]
    
    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple, typeof(OverlayDot))]
    [Description("Returns the Formula value of a Global Name Variable")]

    [Serializable]
    public class GetFormulaGlobalName : PropertyTypeGetFormula
    {
        [SerializeField]
        protected FieldGetGlobalName m_Variable = new FieldGetGlobalName(ValueFormula.TYPE_ID);

        public override Formula Get(Args args) => this.m_Variable.Get<Formula>(args);

        public override string String => this.m_Variable.ToString();
    }
}