using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Version(0, 0, 1)]
    
    [Title("Tamper Evidence")]
    [Description("Tampers the evidence so that other agents notice it")]

    [Category("Perception/Evidence/Tamper Evidence")]
    
    [Parameter("Evidence", "The Evidence reference")]
    
    [Example(
        "If a door is closed and the player opens it, the door can be considered as tampered " +
        "and enemy agents will be able to notice the change"
    )]

    [Keywords("Change", "Modify", "Manipulate", "Clue")]
    [Image(typeof(IconEvidenceTamper), ColorTheme.Type.Red)]
    
    [Serializable]
    public class InstructionPerceptionEvidenceTamper : Instruction
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Evidence = GetGameObjectEvidence.Create;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Tamper {this.m_Evidence}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            Evidence evidence = this.m_Evidence.Get<Evidence>(args);
            if (evidence == null) return DefaultResult;
            
            evidence.IsTampered = true;
            return DefaultResult;
        }
    }
}