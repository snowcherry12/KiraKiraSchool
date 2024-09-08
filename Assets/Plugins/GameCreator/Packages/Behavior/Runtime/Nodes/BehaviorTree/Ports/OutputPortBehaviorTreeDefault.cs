namespace GameCreator.Runtime.Behavior
{
    public class OutputPortBehaviorTreeDefault : TOutputPort
    {
        public override PortAllowance Allowance => PortAllowance.Single;

        public override WireShape WireShape => WireShape.Linear;
        
        public override PortPosition Position
        {
            get => PortPosition.Bottom;
            set => _ = value;
        }
    }
}