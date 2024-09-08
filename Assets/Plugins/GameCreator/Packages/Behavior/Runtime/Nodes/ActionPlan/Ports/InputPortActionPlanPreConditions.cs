using System;

namespace GameCreator.Runtime.Behavior
{
    public class InputPortActionPlanPreConditions : TInputPort
    {
        private static readonly Type[] ACCEPT_TYPES = 
        {
            typeof(OutputPortActionPlanPreConditions)
        };
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override PortAllowance Allowance => PortAllowance.Single;

        public override WireShape WireShape => WireShape.Bezier;

        protected override Type[] AcceptTypes => ACCEPT_TYPES;
        
        public override PortPosition Position
        {
            get => PortPosition.Right;
            set => _ = value;
        }
    }
}