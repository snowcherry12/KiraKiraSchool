using System;

namespace GameCreator.Runtime.Behavior
{
    public class InputPortActionPlanPostConditions : TInputPort
    {
        private static readonly Type[] ACCEPT_TYPES = 
        {
            typeof(OutputPortActionPlanPostConditions)
        };
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override PortAllowance Allowance => PortAllowance.Single;

        public override WireShape WireShape => WireShape.Bezier;

        protected override Type[] AcceptTypes => ACCEPT_TYPES;
        
        public override PortPosition Position
        {
            get => PortPosition.Left;
            set => _ = value;
        }
    }
}