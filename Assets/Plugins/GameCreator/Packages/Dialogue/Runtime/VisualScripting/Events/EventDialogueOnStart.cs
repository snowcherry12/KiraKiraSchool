using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Dialogue
{
    [Title("On Start Dialogue")]
    [Category("Dialogue/On Start Dialogue")]
    [Description("Executed when a specific Dialogue component starts to play")]

    [Image(typeof(IconDialogue), ColorTheme.Type.Purple, typeof(OverlayArrowRight))]
    
    [Keywords("Node", "Conversation", "Speech", "Text")]
    [Keywords("Begin", "Play")]

    [Serializable]
    public class EventDialogueOnStart : VisualScripting.Event
    {
        [SerializeField] private PropertyGetGameObject m_Dialogue = GetGameObjectDialogue.Create();

        // PROPERTIES: ----------------------------------------------------------------------------
        
        private Dialogue Dialogue { get; set; }
        private Args Args { get; set; }
        
        // INITIALIZERS: --------------------------------------------------------------------------
        
        protected override void OnEnable(Trigger trigger)
        {
            base.OnEnable(trigger);
            
            this.Dialogue = this.m_Dialogue.Get<Dialogue>(trigger);
            if (this.Dialogue == null) return;

            this.Args = new Args(this.Self, this.Dialogue.gameObject);
            
            this.Dialogue.EventStart -= this.OnDialogueStart;
            this.Dialogue.EventStart += this.OnDialogueStart;
        }

        protected override void OnDisable(Trigger trigger)
        {
            base.OnDisable(trigger);
            
            if (this.Dialogue == null) return;
            this.Dialogue.EventStart -= this.OnDialogueStart;
        }

        private void OnDialogueStart()
        {
            _ = this.m_Trigger.Execute(this.Args);
        }
    }
}