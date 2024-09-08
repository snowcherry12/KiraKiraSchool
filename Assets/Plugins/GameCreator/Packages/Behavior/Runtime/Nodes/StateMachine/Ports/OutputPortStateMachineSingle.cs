using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    public class OutputPortStateMachineSingle : TOutputPort
    {
        [SerializeField] private PortPosition m_Position = PortPosition.Right;
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override PortAllowance Allowance => PortAllowance.Single;

        public override WireShape WireShape => WireShape.Bezier;
        
        public override PortPosition Position
        {
            get => this.m_Position;
            set => this.m_Position = value;
        }
    }
}