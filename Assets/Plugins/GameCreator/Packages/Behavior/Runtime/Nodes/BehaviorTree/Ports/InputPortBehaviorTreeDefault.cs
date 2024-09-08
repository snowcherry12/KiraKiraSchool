using System;

namespace GameCreator.Runtime.Behavior
{
    public class InputPortBehaviorTreeDefault : TInputPort
    {
        private static readonly Type[] ACCEPT_TYPES = 
        {
            typeof(OutputPortBehaviorTreeDefault),
            typeof(OutputPortBehaviorTreeComposite)
        };
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override PortAllowance Allowance => PortAllowance.Single;

        public override WireShape WireShape => WireShape.Linear;

        protected override Type[] AcceptTypes => ACCEPT_TYPES;

        public override PortPosition Position
        {
            get => PortPosition.Top;
            set => _ = value;
        }
    }
}