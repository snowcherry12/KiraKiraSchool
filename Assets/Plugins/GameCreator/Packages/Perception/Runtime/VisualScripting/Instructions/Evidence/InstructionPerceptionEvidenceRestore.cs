using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Version(0, 0, 1)]
    
    [Title("Restore Evidence")]
    [Description("Restores the state of the evidence so that other agents do not notice it")]

    [Category("Perception/Evidence/Restore Evidence")]
    
    [Parameter("Evidence", "The Evidence reference")]

    [Keywords("Change", "Modify", "Manipulate", "Clue")]
    [Image(typeof(IconEvidence), ColorTheme.Type.Green)]
    
    [Serializable]
    public class InstructionPerceptionEvidenceRestore : Instruction
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Evidence = GetGameObjectEvidence.Create;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Restore {this.m_Evidence}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            Evidence evidence = this.m_Evidence.Get<Evidence>(args);
            if (evidence == null) return DefaultResult;
            
            evidence.IsTampered = false;
            return DefaultResult;
        }
    }
}