using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Version(0, 1, 1)]
    
    [Title("Set Formula")]
    [Category("Stats/Set Formula")]
    
    [Image(typeof(IconFormula), ColorTheme.Type.Purple)]
    [Description("Sets a Formula value")]

    [Parameter("To", "Where to store the Formula asset")]
    [Parameter("Formula", "The Formula asset to store")]

    [Serializable]
    public class InstructionFormulasSetFormula : Instruction
    {
        [SerializeField] private PropertySetFormula m_To = SetFormulaNone.Create;
        [SerializeField] private PropertyGetFormula m_Formula = new PropertyGetFormula();

        public override string Title => $"Set {this.m_To} = {this.m_Formula}";
        
        protected override Task Run(Args args)
        {
            this.m_To.Set(this.m_Formula.Get(args), args);
            return DefaultResult;
        }
    }
}