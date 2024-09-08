using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    [Serializable]
    public class NodeBehaviorTreeComposite : TNodeBehaviorTree
    {
        public const string TYPE_ID = "behavior-tree:composite";
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private Stop m_Stop = Stop.Immediately;
        [SerializeField] private RunConditionsList m_Conditions = new RunConditionsList();
        
        [SerializeReference] private TComposite m_Composite = new CompositeSelector();
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override PropertyName TypeId => TYPE_ID;

        // INPUT METHODS: -------------------------------------------------------------------------

        protected override TInputPort[] CreateInputs() => new TInputPort[]
        {
            new InputPortBehaviorTreeDefault()
        };
        
        protected override TOutputPort[] CreateOutputs() => new TOutputPort[]
        {
            new OutputPortBehaviorTreeComposite()
        };
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public override void CopyFrom(object source)
        {
            base.CopyFrom(source);
            if (source is not NodeBehaviorTreeComposite sourceNode) return;
            
            this.m_Stop = sourceNode.m_Stop;
            this.m_Conditions = sourceNode.m_Conditions;
            this.m_Composite = sourceNode.m_Composite;
        }
        
        // PROTECTED METHODS: ---------------------------------------------------------------------
        
        protected override Status Update(Processor processor, Graph graph)
        {
            if (this.m_Composite == null) return Status.Failure;

            if (this.m_Conditions.Check(processor.Args))
            {
                return this.m_Composite.Run(this, processor, graph);
            }

            if (this.m_Stop == Stop.WhenComplete && this.GetStatus(processor) == Status.Running)
            {
                Status status = this.m_Composite.Run(this, processor, graph);
                if (status == Status.Running) return Status.Running;
            }

            this.Abort(processor, graph);
            return Status.Failure;
        }

        protected override void Cancel(Processor processor, Graph graph)
        {
            base.Cancel(processor, graph);
            if (this.Ports.Outputs.Length == 0) return;

            Connection[] connections = this.Ports.Outputs[0].Connections; 
            if (connections.Length == 0) return;

            foreach (Connection inputPortId in connections)
            {
                TNode subNode = graph.GetFromPortId(inputPortId);
                subNode?.Abort(processor, graph);
            }
        }
    }
}