using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    [Title("Processor Update")]
    [Description("Manually executes a new iteration on a Processor")]
    
    [Image(typeof(IconProcessor), ColorTheme.Type.Blue)]

    [Category("Behavior/Processor Update")]
    [Parameter("Processor", "The targeted Processor component")]

    [Keywords("AI", "Behavior Tree", "State Machine", "Utility", "Need", "Goal", "Plan", "GOAP")]
    [Serializable]
    public class InstructionProcessorUpdate : Instruction
    {
        [SerializeField] 
        private PropertyGetGameObject m_Processor = GetGameObjectInstance.Create();

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Update {this.m_Processor}";

        // RUN METHOD: ----------------------------------------------------------------------------
        
        protected override Task Run(Args args)
        {
            Processor processor = this.m_Processor.Get<Processor>(args);
            if (processor != null) processor.Tick();
            
            return DefaultResult;
        }
    }
}