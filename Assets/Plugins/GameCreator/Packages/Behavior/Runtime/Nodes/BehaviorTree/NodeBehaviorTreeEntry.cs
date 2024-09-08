using System;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    [Serializable]
    public class NodeBehaviorTreeEntry : TNodeBehaviorTree
    {
        public const string TYPE_ID = "behavior-tree:entry";
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override PropertyName TypeId => TYPE_ID;

        // INPUT METHODS: -------------------------------------------------------------------------

        protected override TOutputPort[] CreateOutputs() => new TOutputPort[]
        {
            new OutputPortBehaviorTreeDefault()
        };
        
        // PROTECTED METHODS METHODS: -------------------------------------------------------------
        
        protected override Status Update(Processor processor, Graph graph)
        {
            Status status = this.GetStatus(processor);
            if (status == Status.Success || status == Status.Failure) return status;

            if (this.Ports.Outputs.Length == 0) return Status.Failure;
            if (this.Ports.Outputs[0].Connections.Length == 0) return Status.Success;
            
            TNode subNode = graph.GetFromPortId(this.Ports.Outputs[0].Connections[0]);
            return subNode?.Run(processor, graph) ?? Status.Success;
        }

        protected override void Cancel(Processor processor, Graph graph)
        {
            base.Cancel(processor, graph);
            
            if (this.Ports.Outputs.Length == 0) return;
            if (this.Ports.Outputs[0].Connections.Length == 0) return;

            TNode subNode = graph.GetFromPortId(this.Ports.Outputs[0].Connections[0]);
            subNode?.Abort(processor, graph);
        }
    }
}