using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Dialogue
{
    [Title("On Start Dialogue Line")]
    [Category("Dialogue/On Start Dialogue Line")]
    [Description("Executed when any or a specific Dialogue starts playing a new line")]

    [Image(typeof(IconNodeText), ColorTheme.Type.Green)]
    
    [Keywords("Node", "Conversation", "Speech", "Text")]
    [Keywords("Play", "New", "Next")]

    [Serializable]
    public class EventDialogueStartLine : VisualScripting.Event
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

            Dialogue.EventStartLine -= this.OnDialogueStartLine;
            if (this.Dialogue != null) this.Dialogue.EventStartNext -= this.OnDialogueStartLine;

            if (this.m_Dialogue.Any)
            {
                Dialogue.EventStartLine += this.OnDialogueStartLine;
            }
            else
            {
                this.Dialogue = this.m_Dialogue.Get<Dialogue>(trigger.gameObject);
                if (this.Dialogue != null)
                {
                    this.Args.ChangeTarget(this.Dialogue);
                    this.Dialogue.EventStartNext += this.OnDialogueStartLine;
                }
            }
        }

        protected override void OnDisable(Trigger trigger)
        {
            base.OnDisable(trigger);
            
            Dialogue.EventStartLine -= this.OnDialogueStartLine;
            if (this.Dialogue != null) this.Dialogue.EventStartNext -= this.OnDialogueStartLine;
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private void OnDialogueStartLine(int nodeId)
        {
            _ = this.m_Trigger.Execute(this.Args);
        }
        
        private void OnDialogueStartLine(Dialogue dialogue)
        {
            this.Dialogue = dialogue;
            this.Args.ChangeTarget(dialogue);
            
            _ = this.m_Trigger.Execute(this.Args);
        }
    }
}