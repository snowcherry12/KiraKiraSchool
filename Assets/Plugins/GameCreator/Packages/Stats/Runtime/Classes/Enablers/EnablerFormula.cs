using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Stats
{
    [Serializable]
    public class EnablerFormula : TEnablerValue<Formula>
    {
        public EnablerFormula()
        { }

        public EnablerFormula(Formula value) : base(false, value)
        { }
        
        public EnablerFormula(bool isEnabled, Formula value) : base(isEnabled, value)
        { }
    }
}