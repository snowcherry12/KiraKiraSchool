using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    [Serializable]
    public class NodeBehaviorTreeSubgraph : TNodeBehaviorTree
    {
        public const string TYPE_ID = "behavior-tree:subgraph";
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private Stop m_Stop = Stop.Immediately;
        [SerializeField] private RunConditionsList m_Conditions = new RunConditionsList();

        [SerializeField] private Graph m_Graph;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override PropertyName TypeId => TYPE_ID;
        
        public override Graph Subgraph => this.m_Graph;

        // INPUT METHODS: -------------------------------------------------------------------------

        protected override TInputPort[] CreateInputs() => new TInputPort[]
        {
            new InputPortBehaviorTreeDefault()
        };
        
        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public override void CopyFrom(object source)
        {
            base.CopyFrom(source);
            if (source is not NodeBehaviorTreeSubgraph sourceNode) return;
            
            this.m_Stop = sourceNode.m_Stop;
            this.m_Conditions = sourceNode.m_Conditions;
            this.m_Graph = sourceNode.m_Graph;
        }
        
        // PROTECTED METHODS: ---------------------------------------------------------------------
        
        protected override Status Update(Processor processor, Graph graph)
        {
            if (this.Subgraph == null) return Status.Success;
            
            Status status = this.GetStatus(processor); 
            if (status == Status.Success || status == Status.Failure) return status;
            
            if (this.m_Conditions.Check(processor.Args))
            {
                return this.m_Graph.Run(processor);
            }
            
            if (this.m_Stop == Stop.WhenComplete && status == Status.Running)
            {
                Status subStatus = this.m_Graph.Run(processor);
                if (subStatus == Status.Running) return Status.Running;
            }

            this.Abort(processor, graph);
            return Status.Failure;
        }

        protected override void Cancel(Processor processor, Graph graph)
        {
            base.Cancel(processor, graph);
            
            if (this.m_Graph == null) return;
            this.m_Graph.Abort(processor);
        }
        
        // STRING: --------------------------------------------------------------------------------

        public override string ToString()
        {
            string graph = this.m_Graph != null ? this.m_Graph.name : "(none)";
            return $"{graph}:{this.Id}";
        }
    }
}