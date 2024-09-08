namespace GameCreator.Runtime.Behavior
{
    public class OutputPortActionPlanPreConditions : TOutputPort
    {
        public override PortAllowance Allowance => PortAllowance.Single;

        public override WireShape WireShape => WireShape.Bezier;
        
        public override PortPosition Position
        {
            get => PortPosition.Left;
            set => _ = value;
        }
    }
}