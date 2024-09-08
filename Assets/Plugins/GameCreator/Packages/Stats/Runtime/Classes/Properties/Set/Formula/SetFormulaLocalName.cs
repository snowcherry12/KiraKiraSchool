using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Stats
{
    [Title("Local Name Variable")]
    [Category("Variables/Local Name Variable")]
    
    [Description("Sets the Formula value of a Local Name Variable")]
    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple)]

    [Serializable]
    public class SetFormulaLocalName : PropertyTypeSetFormula
    {
        [SerializeField]
        protected FieldSetLocalName m_Variable = new FieldSetLocalName(ValueFormula.TYPE_ID);

        public override void Set(Formula value, Args args) => this.m_Variable.Set(value, args);
        public override Formula Get(Args args) => this.m_Variable.Get(args) as Formula;

        public static PropertySetFormula Create => new PropertySetFormula(
            new SetFormulaLocalName()
        );
        
        public override string String => this.m_Variable.ToString();
    }
}