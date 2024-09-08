using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    [Serializable]
    public class NodeActionPlanTaskSubgraph : TNodeActionPlanTask
    {
        public const string TYPE_ID = "action-plan:subgraph";
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private Graph m_Graph;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override PropertyName TypeId => TYPE_ID;

        public override Graph Subgraph => this.m_Graph;
        
        protected override string Name => this.m_Graph != null 
            ? TextUtils.Humanize(this.m_Graph.name) 
            : "Graph";

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public override void CopyFrom(object source)
        {
            base.CopyFrom(source);
            if (source is not NodeActionPlanTaskSubgraph sourceNode) return;
            
            this.m_Graph = sourceNode.m_Graph;
        }
        
        // PROTECTED METHODS: ---------------------------------------------------------------------
        
        protected override Status Update(Processor processor, Graph graph)
        {
            if (this.Subgraph == null) return Status.Failure;
            Status status = this.GetStatus(processor);

            if (status == Status.Success) return Status.Success;
            if (status == Status.Failure) return Status.Failure;

            if (status == Status.Ready)
            {
                return this.CheckConditions(processor.Args) 
                    ? this.m_Graph.Run(processor) 
                    : Status.Failure;
            }

            return this.m_Graph.Run(processor);
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