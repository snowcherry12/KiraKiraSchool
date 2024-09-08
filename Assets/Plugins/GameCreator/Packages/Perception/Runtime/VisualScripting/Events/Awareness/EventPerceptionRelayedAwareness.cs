using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Title("On Relayed Awareness")]
    [Category("Perception/Awareness/On Relayed Awareness")]
    
    [Description(
        "Executed when an agent with Perception receives new Awareness information from " +
        "another agent"
    )]

    [Image(typeof(IconAwareness), ColorTheme.Type.Green, typeof(OverlayArrowLeft))]
    [Keywords("Detect", "Bark", "Info", "Receive", "Propagate", "Transmit", "Communicate")]
    
    [Serializable]
    public class EventPerceptionRelayedAwareness : VisualScripting.Event
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
            
            perception.EventRelayedAwareness -= this.OnRelayedAwareness;
            perception.EventRelayedAwareness += this.OnRelayedAwareness;
        }
        
        protected override void OnDisable(Trigger trigger)
        {
            base.OnDisable(trigger);
            
            if (ApplicationManager.IsExiting) return;

            Perception perception = this.m_Source != null ? this.m_Source.Get<Perception>() : null;
            if (perception == null) return;

            perception.EventRelayedAwareness -= this.OnRelayedAwareness;
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void OnRelayedAwareness(GameObject target)
        {
            if (this.m_Args.Target != target)
            {
                this.m_Args.ChangeTarget(target);
            }
            
            _ = this.m_Trigger.Execute(this.m_Args);
        }
    }
}