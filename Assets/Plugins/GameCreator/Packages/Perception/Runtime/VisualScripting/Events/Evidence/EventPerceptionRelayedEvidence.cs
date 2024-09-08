using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Title("On Relayed Evidence")]
    [Category("Perception/Evidence/On Relayed Evidence")]
    
    [Description(
        "Executed when an agent with Perception receives new Evidence information from " +
        "another agent"
    )]

    [Image(typeof(IconEvidence), ColorTheme.Type.Green, typeof(OverlayArrowLeft))]
    [Keywords("Detect", "Bark", "Info", "Receive", "Propagate", "Transmit", "Communicate")]
    
    [Serializable]
    public class EventPerceptionRelayedEvidence : VisualScripting.Event
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField]
        private PropertyGetGameObject m_Perception = GetGameObjectPerception.Create;
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private GameObject m_Source;
        [NonSerialized] private Args m_Args;
        
        // INITIALIZERS: --------------------------------------------------------------------------
        
        protected override void OnEnable(Trigger trigger)
        {
            base.OnEnable(trigger);
            
            Perception perception = this.m_Perception.Get<Perception>(trigger);
            if (perception == null) return;

            this.m_Source = perception.gameObject;
            this.m_Args = new Args(this.m_Source);
            
            perception.EventRelayedEvidence -= this.OnRelayedEvidence;
            perception.EventRelayedEvidence += this.OnRelayedEvidence;
        }
        
        protected override void OnDisable(Trigger trigger)
        {
            base.OnDisable(trigger);
            
            if (ApplicationManager.IsExiting) return;

            Perception perception = this.m_Source != null ? this.m_Source.Get<Perception>() : null;
            if (perception == null) return;

            perception.EventRelayedEvidence -= this.OnRelayedEvidence;
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void OnRelayedEvidence(GameObject target)
        {
            if (this.m_Args.Target != target)
            {
                this.m_Args.ChangeTarget(target);
            }
            
            _ = this.m_Trigger.Execute(this.m_Args);
        }
    }
}