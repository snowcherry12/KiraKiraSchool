using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Dialogue
{
    [Title("On Finish Dialogue Line")]
    [Category("Dialogue/On Finish Dialogue Line")]
    [Description("Executed when any or a specific Dialogue finishes playing the current line")]

    [Image(typeof(IconNodeText), ColorTheme.Type.Red)]
    
    [Keywords("Node", "Conversation", "Speech", "Text")]
    [Keywords("Play", "New", "Next", "Continue", "Skip")]

    [Serializable]
    public class EventDialogueFinishLine : VisualScripting.Event
    {
        [SerializeField]
        private CompareGameObjectOrAny m_Dialogue = new CompareGameObjectOrAny(
            true,
            GetGameObjectDialogue.Create()
        );
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        [field: NonSerialized] private Dialogue Dialogue { get; set; }
        [field: NonSerialized] private Args Args { get; set; }
        
        // INITIALIZERS: --------------------------------------------------------------------------
        
        protected override void OnEnable(Trigger trigger)
        {
            base.OnEnable(trigger);
            this.Args = new Args(this.Self);

            Dialogue.EventFinishLine -= this.OnDialogueFinishLine;
            if (this.Dialogue != null) this.Dialogue.EventFinishNext -= this.OnDialogueFinishLine;

            if (this.m_Dialogue.Any)
            {
                Dialogue.EventFinishLine += this.OnDialogueFinishLine;
            }
            else
            {
                this.Dialogue = this.m_Dialogue.Get<Dialogue>(trigger.gameObject);
                if (this.Dialogue != null)
                {
                    this.Args.ChangeTarget(this.Dialogue);
                    this.Dialogue.EventFinishNext += this.OnDialogueFinishLine;
                }
            }
        }

        protected override void OnDisable(Trigger trigger)
        {
            base.OnDisable(trigger);
            
            Dialogue.EventFinishLine -= this.OnDialogueFinishLine;
            if (this.Dialogue != null) this.Dialogue.EventFinishNext -= this.OnDialogueFinishLine;
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private void OnDialogueFinishLine(int nodeId)
        {
            _ = this.m_Trigger.Execute(this.Args);
        }
        
        private void OnDialogueFinishLine(Dialogue dialogue)
        {
            this.Dialogue = dialogue;
            this.Args.ChangeTarget(dialogue);
            
            _ = this.m_Trigger.Execute(this.Args);
        }
    }
}