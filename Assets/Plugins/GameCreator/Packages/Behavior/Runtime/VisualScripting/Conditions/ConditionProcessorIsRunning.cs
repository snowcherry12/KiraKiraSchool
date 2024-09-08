using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    [Title("Is Processor Running")]
    [Description("Returns true if the Processor is in a Running state")]

    [Category("Behavior/Is Processor Running")]
    
    [Parameter("Processor", "The reference to the Processor component")]

    [Keywords("AI", "Behavior Tree", "State Machine", "Utility", "Need", "Goal", "Plan", "GOAP")]
    [Image(typeof(IconProcessor), ColorTheme.Type.Blue)]
    
    [Serializable]
    public class ConditionProcessorIsRunning : Condition
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Processor = GetGameObjectInstance.Create();

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"{this.m_Processor} is Running";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Processor processor = this.m_Processor.Get<Processor>(args);
            return processor != null && processor.Status == Status.Running;
        }
    }
}
