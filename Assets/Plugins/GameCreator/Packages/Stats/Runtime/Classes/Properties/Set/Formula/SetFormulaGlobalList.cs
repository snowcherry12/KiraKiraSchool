using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Stats
{
    [Title("Global List Variable")]
    [Category("Variables/Global List Variable")]
    
    [Description("Sets the Formula value of a Global List Variable")]
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal, typeof(OverlayDot))]

    [Serializable]
    public class SetFormulaGlobalList : PropertyTypeSetFormula
    {
        [SerializeField]
        protected FieldSetGlobalList m_Variable = new FieldSetGlobalList(ValueFormula.TYPE_ID);

        public override void Set(Formula value, Args args) => this.m_Variable.Set(value, args);
        public override Formula Get(Args args) => this.m_Variable.Get(args) as Formula;

        public static PropertySetFormula Create => new PropertySetFormula(
            new SetFormulaGlobalList()
        );
        
        public override string String => this.m_Variable.ToString();
    }
}