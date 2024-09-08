using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Stats
{
    [Title("Local Name Variable")]
    [Category("Variables/Local Name Variable")]

    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple)]
    [Description("Returns the Formula value of a Local Name Variable")]
    
    [Serializable]
    public class GetFormulaLocalName : PropertyTypeGetFormula
    {
        [SerializeField]
        protected FieldGetLocalName m_Variable = new FieldGetLocalName(ValueFormula.TYPE_ID);

        public override Formula Get(Args args) => this.m_Variable.Get<Formula>(args);

        public override string String => this.m_Variable.ToString();
    }
}