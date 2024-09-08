using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Dialogue.UnityUI
{
    [Version(0, 1, 1)]
    
    [Title("Skip Line")]
    [Category("Dialogue/UI/Skip Line")]
    
    [Image(typeof(IconNodeText), ColorTheme.Type.Blue, typeof(OverlayArrowRight))]
    [Description("Finishes a dialogue UI line or skips to the next one")]

    [Parameter("Speech UI", "The Speech UI component associated")]

    [Keywords("Dialogue", "Narration", "Speech", "Next", "Skip")]

    [Serializable]
    public class InstructionDialogueUISkip : Instruction
    {
        [SerializeField] private SpeechUI m_SpeechUI;

        public override string Title => "Skip Line";
        
        protected override Task Run(Args args)
        {
            if (this.m_SpeechUI != null) this.m_SpeechUI.Skip();
            return DefaultResult;
        }
    }
}