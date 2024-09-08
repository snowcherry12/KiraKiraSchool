using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Stats
{
    [Title("Local List Variable")]
    [Category("Variables/Local List Variable")]
    
    [Description("Sets the Formula value of a Local List Variable")]
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal)]

    [Serializable]
    public class SetFormulaLocalList : PropertyTypeSetFormula
    {
        [SerializeField]
        protected FieldSetLocalList m_Variable = new FieldSetLocalList(ValueFormula.TYPE_ID);

        public override void Set(Formula value, Args args) => this.m_Variable.Set(value, args);
        public override Formula Get(Args args) => this.m_Variable.Get(args) as Formula;

        public static PropertySetFormula Create => new PropertySetFormula(
            new SetFormulaLocalList()
        );
        
        public override string String => this.m_Variable.ToString();
    }
}