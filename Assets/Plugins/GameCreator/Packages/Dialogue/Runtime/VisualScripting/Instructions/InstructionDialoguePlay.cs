using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Dialogue
{
    [Version(0, 1, 1)]
    
    [Title("Play Dialogue")]
    [Category("Dialogue/Play Dialogue")]
    
    [Image(typeof(IconNodeText), ColorTheme.Type.Blue)]
    [Description("Plays a dialogue")]

    [Parameter("Dialogue", "The Dialogue component to play")]
    [Parameter("Wait to Finish", "Whether to wait until the Dialogue is finished or not")]

    [Keywords("Dialogue", "Narration", "Speech", "Next", "Skip")]

    [Serializable]
    public class InstructionDialoguePlay : Instruction
    {
        [SerializeField] private PropertyGetGameObject m_Dialogue = GetGameObjectDialogue.Create();
        [SerializeField] private bool m_WaitToFinish = true;

        public override string Title => string.Format(
            "Play {0} {1}",
            this.m_Dialogue, 
            this.m_WaitToFinish ? "and wait" : string.Empty
        );
        
        protected override async Task Run(Args args)
        {
            Dialogue dialogue = this.m_Dialogue.Get<Dialogue>(args);
            if (dialogue == null) return;

            if (this.m_WaitToFinish) await dialogue.Play(args);
            else _ = dialogue.Play(args);
        }
    }
}