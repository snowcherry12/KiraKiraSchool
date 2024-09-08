using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    [Title("Remove Goal")]
    [Description("Removes an existing Goal from the specified Action Plan")]
    
    [Image(typeof(IconActionPlanOutline), ColorTheme.Type.Red, typeof(OverlayMinus))]

    [Category("Behavior/Action Plan/Remove Goal")]
    
    [Parameter("Processor", "The targeted Processor component")]
    [Parameter("Action Plan", "The Action Plan asset to remove the goal")]
    [Parameter("Name", "Name identifier of the goal")]
    [Parameter("Weight", "The weight the goal has when calculating the plan")]

    [Keywords("AI", "Action", "Goal", "Plan", "GOAP")]
    [Serializable]
    public class InstructionActionPlanRemoveGoal : Instruction
    {
        [SerializeField] private ActionPlan m_ActionPlan;
        
        [SerializeField] 
        private PropertyGetGameObject m_Processor = GetGameObjectInstance.Create();

        [SerializeField] private IdString m_Name;

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => string.Format(
            "Remove Goal from {0} = {1}",
            this.m_ActionPlan != null ? this.m_ActionPlan.name : "(none)",
            this.m_Name
        );

        // RUN METHOD: ----------------------------------------------------------------------------
        
        protected override Task Run(Args args)
        {
            if (this.m_ActionPlan == null) return DefaultResult;

            Processor processor = this.m_Processor.Get<Processor>(args);
            if (processor == null) return DefaultResult;

            if (this.m_Name == IdString.EMPTY) return DefaultResult;

            Goal goal = new Goal(this.m_Name, 0f);
            this.m_ActionPlan.RemoveGoal(goal, processor);
            
            return DefaultResult;
        }
    }
}