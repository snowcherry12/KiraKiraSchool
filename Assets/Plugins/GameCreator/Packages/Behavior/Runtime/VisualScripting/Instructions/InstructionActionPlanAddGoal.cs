using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    [Title("Add Goal")]
    [Description("Adds a new Goal to the specified Action Plan")]
    
    [Image(typeof(IconActionPlanOutline), ColorTheme.Type.Green, typeof(OverlayPlus))]

    [Category("Behavior/Action Plan/Add Goal")]
    
    [Parameter("Processor", "The targeted Processor component")]
    [Parameter("Action Plan", "The Action Plan asset to set the goal")]
    [Parameter("Name", "Name identifier of the goal")]
    [Parameter("Weight", "The weight the goal has when calculating the plan")]

    [Keywords("AI", "Action", "Goal", "Plan", "GOAP")]
    [Serializable]
    public class InstructionActionPlanAddGoal : Instruction
    {
        [SerializeField] private ActionPlan m_ActionPlan;
        
        [SerializeField] 
        private PropertyGetGameObject m_Processor = GetGameObjectInstance.Create();

        [SerializeField] private IdString m_Name;
        [SerializeField] private PropertyGetDecimal m_Weight = GetDecimalConstantOne.Create;

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => string.Format(
            "Add Goal to {0} = {1}",
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

            Goal goal = new Goal(this.m_Name, (float) this.m_Weight.Get(args));
            this.m_ActionPlan.AddGoal(goal, processor);
            
            return DefaultResult;
        }
    }
}