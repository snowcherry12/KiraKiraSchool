namespace GameCreator.Runtime.Behavior
{
    public class OutputPortBehaviorTreeComposite : TOutputPort
    {
        public override PortAllowance Allowance => PortAllowance.Multiple;

        public override WireShape WireShape => WireShape.Linear;
        
        public override PortPosition Position
        {
            get => PortPosition.Bottom;
            set => _ = value;
        }
    }
}