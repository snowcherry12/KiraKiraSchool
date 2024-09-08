using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Title("Check Formula")]
    [Category("Stats/Check Formula")]
    
    [Image(typeof(IconFormula), ColorTheme.Type.Purple)]
    [Description("Returns the comparison between the result of a Formula against another value")]

    [Parameter("Formula", "The Formula used in the operation")]
    [Parameter("Source", "The game object that the Formula identifies as the Source")]
    [Parameter("Target", "The game object that the Formula identifies as the Target")]
    
    [Parameter("Compare To", "The value that the result of the Formula is compared to")]

    [Keywords("Skill", "Throw", "Check", "Dice")]
    [Keywords("Lock", "Pick", "Charisma", "Speech")]

    [Serializable]
    public class ConditionFormula : Condition
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetFormula m_Formula = new PropertyGetFormula();
        [SerializeField] private PropertyGetGameObject m_Source = GetGameObjectSelf.Create();
        [SerializeField] private PropertyGetGameObject m_Target = GetGameObjectTarget.Create();

        [Space]
        [SerializeField] private CompareDouble m_CompareTo = new CompareDouble(50f);

        // PROPERTIES: ----------------------------------------------------------------------------

        protected override string Summary => $"{this.m_Formula} {this.m_CompareTo}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Formula formula = this.m_Formula.Get(args);
            if (formula == null) return false;
            
            GameObject source = this.m_Source.Get(args);
            GameObject target = this.m_Target.Get(args);
            
            double result = formula.Calculate(source, target);
            return this.m_CompareTo.Match(result, args);
        }
    }
}
