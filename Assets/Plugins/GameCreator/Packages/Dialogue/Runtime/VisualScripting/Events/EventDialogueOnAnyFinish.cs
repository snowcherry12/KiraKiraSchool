using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Dialogue
{
    [Title("On Finish Any Dialogue")]
    [Category("Dialogue/On Finish Any Dialogue")]
    [Description("Executed when any Dialogue component finishes playing")]

    [Image(typeof(IconDialogue), ColorTheme.Type.Purple, typeof(OverlayCross))]
    
    [Keywords("Node", "Conversation", "Speech", "Text")]
    [Keywords("End", "Complete")]

    [Serializable]
    public class EventDialogueOnAnyFinish : VisualScripting.Event
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        private Args Args { get; set; }
        
        // INITIALIZERS: --------------------------------------------------------------------------
        
        protected override void OnEnable(Trigger trigger)
        {
            base.OnEnable(trigger);

            this.Args = new Args(this.Self, this.Self);
            
            Dialogue.EventAnyFinish -= this.OnDialogueFinish;
            Dialogue.EventAnyFinish += this.OnDialogueFinish;
        }

        protected override void OnDisable(Trigger trigger)
        {
            base.OnDisable(trigger);
            Dialogue.EventAnyFinish -= this.OnDialogueFinish;
        }

        private void OnDialogueFinish(Dialogue dialogue)
        {
            this.Args.ChangeTarget(dialogue);
            _ = this.m_Trigger.Execute(this.Args);
        }
    }
}