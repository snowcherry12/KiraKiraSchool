using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Stats
{
    [Title("Global Name Variable")]
    [Category("Variables/Global Name Variable")]
    
    [Description("Sets the Formula value of a Global Name Variable")]
    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple, typeof(OverlayDot))]

    [Serializable]
    public class SetFormulaGlobalName : PropertyTypeSetFormula
    {
        [SerializeField]
        protected FieldSetGlobalName m_Variable = new FieldSetGlobalName(ValueFormula.TYPE_ID);

        public override void Set(Formula value, Args args) => this.m_Variable.Set(value, args);
        public override Formula Get(Args args) => this.m_Variable.Get(args) as Formula;

        public static PropertySetFormula Create => new PropertySetFormula(
            new SetFormulaGlobalName()
        );
        
        public override string String => this.m_Variable.ToString();
    }
}