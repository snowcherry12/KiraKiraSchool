using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Dialogue.UnityUI
{
    [Version(0, 1, 1)]
    
    [Title("Choice Index")]
    [Category("Dialogue/UI/Choice Index")]
    
    [Image(typeof(IconNodeChoice), ColorTheme.Type.Blue)]
    [Description("Attempts to choose a Choice node by its index (starting at 1), if it exists")]

    [Parameter("Index", "The numeric index of the Choice, starting from 1")]

    [Keywords("Dialogue", "Narration", "Speech", "Choose", "Pick")]

    [Serializable]
    public class InstructionDialogueUIChoiceIndex : Instruction
    {
        [SerializeField] private PropertyGetInteger m_Index = GetDecimalInteger.Create(1);

        public override string Title => $"Choice Index {this.m_Index}";
        
        protected override Task Run(Args args)
        {
            int index = (int) this.m_Index.Get(args);
            DialogueChoiceUI.SelectIndex(index);
            
            return DefaultResult;
        }
    }
}