using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Title("Formula")]
    [Category("Formula")]
    
    [Image(typeof(IconAttr), ColorTheme.Type.Blue)]
    [Description("A direct reference to the Formula value")]

    [Serializable] [HideLabelsInEditor]
    public class GetFormulaInstance : PropertyTypeGetFormula
    {
        [SerializeField] protected Formula m_Formula;

        public override Formula Get(Args args) => this.m_Formula;
        public override Formula Get(GameObject gameObject) => this.m_Formula;

        public override string String => this.m_Formula != null
            ? $"{this.m_Formula.name}"
            : "(none)";
    }
}