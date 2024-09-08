using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Title("Formula")]
    [Category("Stats/Formula")]

    [Image(typeof(IconFormula), ColorTheme.Type.Purple)]
    [Description("Evaluates a formula and returns the resulting value")]

    [Serializable]
    public class GetDecimalFormula : PropertyTypeGetDecimal
    {
        [SerializeField] private PropertyGetFormula m_Formula = new PropertyGetFormula();

        [SerializeField] private PropertyGetGameObject m_Source = GetGameObjectSelf.Create();
        [SerializeField] private PropertyGetGameObject m_Target = GetGameObjectTarget.Create();

        public override double Get(Args args)
        {
            Formula formula = this.m_Formula.Get(args);
            if (formula == null) return 0f;
            
            GameObject source = this.m_Source.Get(args);
            GameObject target = this.m_Target.Get(args);
            
            return formula.Calculate(source, target);
        }

        public static PropertyGetDecimal Create => new PropertyGetDecimal(
            new GetDecimalFormula()
        );

        public override string String => this.m_Formula.ToString();
    }
}