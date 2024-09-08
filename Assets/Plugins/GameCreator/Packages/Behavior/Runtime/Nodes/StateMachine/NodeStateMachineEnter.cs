using System;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    [Serializable]
    public class NodeStateMachineEnter : TNodeStateMachine
    {
        public const string TYPE_ID = "state-machine:enter";
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override PropertyName TypeId => TYPE_ID;

        // INPUT METHODS: -------------------------------------------------------------------------

        protected override TOutputPort[] CreateOutputs() => new TOutputPort[]
        {
            new OutputPortStateMachineMultiple()
        };
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public override bool TryRun(Processor processor, Graph graph, BeforeRunHandle beforeEnter)
        {
            beforeEnter?.Invoke(processor);
            this.Run(processor, graph);
            
            return true;
        }

        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected override Status Update(Processor processor, Graph graph)
        {
            if (this.Ports.Outputs.Length == 0) return Status.Success;

            TOutputPort outputs = this.Ports.Outputs[0]; 
            if (outputs.Connections.Length == 0) return Status.Success;

            foreach (Connection connection in outputs.Connections)
            {
                if (graph.GetFromPortId(connection) is not TNodeStateMachine candidate) continue;
                if (candidate.TryRun(processor, graph, null)) return Status.Ready;
            }
            
            return Status.Running;
        }
    }
}