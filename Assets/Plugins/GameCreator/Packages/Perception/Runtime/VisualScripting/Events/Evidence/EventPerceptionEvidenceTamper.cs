using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Title("On Tamper Evidence")]
    [Category("Perception/Evidence/On Tamper Evidence")]
    
    [Description("Executed when an agent tampers with an Evidence component")]

    [Image(typeof(IconEvidence), ColorTheme.Type.Yellow)]
    [Keywords("Switch", "Change")]
    
    [Serializable]
    public class EventPerceptionEvidenceTamper : VisualScripting.Event
    {
        private enum State
        {
            Any,
            OnTamper,
            OnRestore
        }
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Evidence = GetGameObjectEvidence.Create;
        [SerializeField] private State m_Detection = State.Any;
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private Evidence m_SourceEvidence;
        [NonSerialized] private Args m_Args;
        
        // INITIALIZERS: --------------------------------------------------------------------------
        
        protected override void OnEnable(Trigger trigger)
        {
            base.OnEnable(trigger);
            
            this.m_SourceEvidence = this.m_Evidence.Get<Evidence>(trigger);
            if (this.m_SourceEvidence == null) return;
            
            this.m_Args = new Args(this.m_SourceEvidence.gameObject);
            
            this.m_SourceEvidence.EventChange -= this.OnEvidenceChange;
            this.m_SourceEvidence.EventChange += this.OnEvidenceChange;
        }
        
        protected override void OnDisable(Trigger trigger)
        {
            base.OnDisable(trigger);
            
            if (ApplicationManager.IsExiting) return;
            
            if (this.m_SourceEvidence == null) return;
            this.m_SourceEvidence.EventChange -= this.OnEvidenceChange;
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void OnEvidenceChange()
        {
            if (this.m_SourceEvidence == null) return;
            switch (this.m_Detection)
            {
                case State.OnTamper:
                    if (!this.m_SourceEvidence.IsTampered) return;
                    break;
                
                case State.OnRestore:
                    if (this.m_SourceEvidence.IsTampered) return;
                    break;
            }
            
            _ = this.m_Trigger.Execute(this.m_Args);
        }
    }
}