using System;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    public class InputPortStateMachineSingle : TInputPort
    {
        private static readonly Type[] ACCEPT_TYPES = 
        {
            typeof(OutputPortStateMachineMultiple),
            typeof(OutputPortStateMachineSingle)
        };
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private PortPosition m_Position = PortPosition.Right;
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override PortAllowance Allowance => PortAllowance.Single;

        public override WireShape WireShape => WireShape.Bezier;
        
        protected override Type[] AcceptTypes => ACCEPT_TYPES;
        
        public override PortPosition Position
        {
            get => this.m_Position;
            set => this.m_Position = value;
        }
    }
}