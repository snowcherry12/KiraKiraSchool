namespace GameCreator.Runtime.Behavior
{
    public class OutputPortStateMachineMultiple : TOutputPort
    {
        public override PortAllowance Allowance => PortAllowance.Multiple;

        public override WireShape WireShape => WireShape.Bezier;
        
        public override PortPosition Position
        {
            get => PortPosition.Right;
            set => _ = value;
        }
    }
}