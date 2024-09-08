using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Stats
{
    [Title("Global List Variable")]
    [Category("Variables/Global List Variable")]
    
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal, typeof(OverlayDot))]
    [Description("Returns the Formula value of a Global List Variable")]

    [Serializable]
    public class GetFormulaGlobalList : PropertyTypeGetFormula
    {
        [SerializeField]
        protected FieldGetGlobalList m_Variable = new FieldGetGlobalList(ValueFormula.TYPE_ID);

        public override Formula Get(Args args) => this.m_Variable.Get<Formula>(args);

        public override string String => this.m_Variable.ToString();
    }
}