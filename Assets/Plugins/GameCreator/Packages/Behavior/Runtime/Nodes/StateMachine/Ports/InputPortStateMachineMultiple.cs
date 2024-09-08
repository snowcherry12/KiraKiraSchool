using System;

namespace GameCreator.Runtime.Behavior
{
    public class InputPortStateMachineMultiple : TInputPort
    {
        private static readonly Type[] ACCEPT_TYPES = 
        {
            typeof(OutputPortStateMachineMultiple),
            typeof(OutputPortStateMachineSingle)
        };
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override PortAllowance Allowance => PortAllowance.Multiple;

        public override WireShape WireShape => WireShape.Bezier;
        
        protected override Type[] AcceptTypes => ACCEPT_TYPES;
        
        public override PortPosition Position
        {
            get => PortPosition.Left;
            set => _ = value;
        }
    }
}