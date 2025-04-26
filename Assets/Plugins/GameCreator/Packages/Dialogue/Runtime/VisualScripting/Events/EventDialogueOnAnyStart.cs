using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Dialogue
{
    [Title("On Start Any Dialogue")]
    [Category("Dialogue/On Start Any Dialogue")]
    [Description("Executed when any Dialogue component starts to play")]

    [Image(typeof(IconDialogue), ColorTheme.Type.Purple, typeof(OverlayArrowRight))]
    
    [Keywords("Node", "Conversation", "Speech", "Text")]
    [Keywords("Begin", "Play")]

    [Serializable]
    public class EventDialogueOnAnyStart : VisualScripting.Event
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        private Args Args { get; set; }
        
        // INITIALIZERS: --------------------------------------------------------------------------
        
        protected override void OnEnable(Trigger trigger)
        {
            base.OnEnable(trigger);

            this.Args = new Args(this.Self, this.Self);
            
            Dialogue.EventAnyStart -= this.OnDialogueStart;
            Dialogue.EventAnyStart += this.OnDialogueStart;
        }

        protected override void OnDisable(Trigger trigger)
        {
            base.OnDisable(trigger);
            Dialogue.EventAnyStart -= this.OnDialogueStart;
        }

        private void OnDialogueStart(Dialogue dialogue)
        {
            this.Args.ChangeTarget(dialogue);
            _ = this.m_Trigger.Execute(this.Args);
        }
    }
}