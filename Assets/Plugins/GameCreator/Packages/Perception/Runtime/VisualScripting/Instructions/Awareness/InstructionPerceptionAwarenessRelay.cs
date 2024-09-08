using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Version(0, 0, 1)]
    
    [Title("Relay Awareness Knowledge")]
    [Description("Relays the Awareness knowledge of a game object to another Perception agent")]

    [Category("Perception/Awareness/Relay Awareness Knowledge")]
    
    [Parameter("Perception", "The Perception component that transmits its Awareness knowledge")]
    [Parameter("Target", "The Perception component that receives the Awareness knowledge")]
    
    [Keywords("Communicate", "Shout", "Tell", "Inform", "Transmit", "Propagate")]
    [Image(typeof(IconAwareness), ColorTheme.Type.Blue, typeof(OverlayArrowRight))]
    
    [Serializable]
    public class InstructionPerceptionAwarenessRelay : TInstructionPerceptionAwareness
    {
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Relay Awareness {this.m_Perception} -> {this.m_Target}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            Perception perception = this.m_Perception.Get<Perception>(args);
            if (perception == null) return DefaultResult;
            
            Perception target = this.m_Target.Get<Perception>(args);
            if (target == null) return DefaultResult;
            
            perception.RelayAwareness(target);
            return DefaultResult;
        }
    }
}