using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Stats
{
    [Title("Local List Variable")]
    [Category("Variables/Local List Variable")]
    
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal)]
    [Description("Returns the Formula value of a Local List Variable")]

    [Serializable]
    public class GetFormulaLocalList : PropertyTypeGetFormula
    {
        [SerializeField]
        protected FieldGetLocalList m_Variable = new FieldGetLocalList(ValueFormula.TYPE_ID);

        public override Formula Get(Args args) => this.m_Variable.Get<Formula>(args);

        public override string String => this.m_Variable.ToString();
    }
}