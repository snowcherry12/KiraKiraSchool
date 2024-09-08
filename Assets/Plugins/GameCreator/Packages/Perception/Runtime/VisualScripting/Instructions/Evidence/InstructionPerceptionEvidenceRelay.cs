using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Version(0, 0, 1)]
    
    [Title("Relay Evidence Knowledge")]
    [Description("Relays the Evidence knowledge of a Perception component to another")]

    [Category("Perception/Evidence/Relay Evidence Knowledge")]
    
    [Parameter("Source", "The Perception component that transmits its Evidences")]
    [Parameter("Target", "The Perception component that receives the Evidence knowledge")]

    [Keywords("Communicate", "Shout", "Tell", "Inform", "Transmit", "Propagate")]
    [Image(typeof(IconEvidence), ColorTheme.Type.Blue, typeof(OverlayArrowRight))]
    
    [Serializable]
    public class InstructionPerceptionEvidenceRelay : Instruction
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField]
        private PropertyGetGameObject m_Source = GetGameObjectPerception.Create;
        
        [SerializeField]
        private PropertyGetGameObject m_Target = GetGameObjectPerception.Create;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Relay Evidences {this.m_Source} -> {this.m_Target}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            Perception source = this.m_Source.Get<Perception>(args);
            Perception target = this.m_Target.Get<Perception>(args);

            if (source == null) return DefaultResult;
            if (target == null) return DefaultResult;
            
            source.RelayEvidence(target);
            return DefaultResult;
        }
    }
}