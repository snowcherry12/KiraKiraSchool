using System;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    [Serializable]
    public class NodeStateMachineElbow : TNodeStateMachine
    {
        public const string TYPE_ID = "state-machine:elbow";

        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PortPosition m_Direction = PortPosition.Right;
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override PropertyName TypeId => TYPE_ID;

        public PortPosition Direction
        {
            get => this.m_Direction;
            set => this.m_Direction = value;
        }

        // INPUT METHODS: -------------------------------------------------------------------------

        protected override TInputPort[] CreateInputs() => new TInputPort[]
        {
            new InputPortStateMachineSingle()
        };

        protected override TOutputPort[] CreateOutputs() => new TOutputPort[]
        {
            new OutputPortStateMachineSingle()
        };

        public override void SetPortDirection(Vector2 direction)
        {
            base.SetPortDirection(direction);
            if (direction == default) return;
            
            PortPosition outputDirection = Math.Abs(direction.x) > Math.Abs(direction.y)
                ? direction.x > 0 ? PortPosition.Right : PortPosition.Left
                : direction.y > 0 ? PortPosition.Top : PortPosition.Bottom;

            PortPosition inputDirection = outputDirection switch
            {
                PortPosition.Top => PortPosition.Bottom,
                PortPosition.Right => PortPosition.Left,
                PortPosition.Bottom => PortPosition.Top,
                PortPosition.Left => PortPosition.Right,
                _ => throw new ArgumentOutOfRangeException()
            };
            
            this.Direction = outputDirection;
            
            foreach (TInputPort input in this.Ports.Inputs)
            {
                input.Position = inputDirection;
            }
            
            foreach (TOutputPort output in this.Ports.Outputs)
            {
                output.Position = outputDirection;
            }
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public override void CopyFrom(object source)
        {
            base.CopyFrom(source);
            if (source is not NodeStateMachineElbow sourceNode) return;
            
            this.m_Direction = sourceNode.m_Direction;
        }

        public override bool TryRun(Processor processor, Graph graph, BeforeRunHandle beforeEnter)
        {
            TOutputPort outputs = this.Ports.Outputs[0]; 
            if (outputs.Connections.Length == 0) return false;

            foreach (Connection connection in outputs.Connections)
            {
                if (graph.GetFromPortId(connection) is not TNodeStateMachine candidate) continue;
                if (candidate.TryRun(processor, graph, beforeEnter)) return true;
            }

            return false;
        }

        // PROTECTED METHODS: ---------------------------------------------------------------------
        
        protected override Status Update(Processor processor, Graph graph)
        {
            return Status.Ready;
        }
    }
}