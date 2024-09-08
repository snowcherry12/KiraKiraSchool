using System;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    [Serializable]
    public class NodeBehaviorTreeDecorator : TNodeBehaviorTree
    {
        public const string TYPE_ID = "behavior-tree:decorator";
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeReference] private TDecorator m_Decorator = new DecoratorInvert();
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override PropertyName TypeId => TYPE_ID;

        // INPUT METHODS: -------------------------------------------------------------------------

        protected override TInputPort[] CreateInputs() => new TInputPort[]
        {
            new InputPortBehaviorTreeDefault()
        };

        protected override TOutputPort[] CreateOutputs() => new TOutputPort[]
        {
            new OutputPortBehaviorTreeDefault()
        };
        
        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public override void CopyFrom(object source)
        {
            base.CopyFrom(source);
            if (source is not NodeBehaviorTreeDecorator sourceNode) return;
            
            this.m_Decorator = sourceNode.m_Decorator;
        }
        
        // PROTECTED METHODS: ---------------------------------------------------------------------
        
        protected override Status Update(Processor processor, Graph graph)
        {
            if (this.m_Decorator == null) return Status.Failure;
            Status status = Status.Ready;

            if (this.m_Decorator.Conditions(this, processor, graph))
            {
                if (this.Ports.Outputs.Length == 0) return Status.Failure;
                if (this.Ports.Outputs[0].Connections.Length == 0) return Status.Success;

                TNode subNode = graph.GetFromPortId(this.Ports.Outputs[0].Connections[0]);
                status = subNode?.Run(processor, graph) ?? Status.Success;
            }
            
            return this.m_Decorator.Run(status, this, processor, graph);
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