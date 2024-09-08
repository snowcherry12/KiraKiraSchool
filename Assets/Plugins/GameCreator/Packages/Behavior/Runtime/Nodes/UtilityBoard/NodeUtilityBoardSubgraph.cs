using System;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    [Serializable]
    public class NodeUtilityBoardSubgraph : TNodeUtilityBoard
    {
        public const string TYPE_ID = "utility-board:subgraph";
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private Graph m_Graph;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override PropertyName TypeId => TYPE_ID;
        
        public override Graph Subgraph => this.m_Graph;

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public override void CopyFrom(object source)
        {
            base.CopyFrom(source);
            if (source is not NodeUtilityBoardSubgraph sourceNode) return;
            
            this.m_Graph = sourceNode.m_Graph;
        }
        
        // PROTECTED METHODS: ---------------------------------------------------------------------
        
        protected override IValueWithScore RequireData(Processor processor)
        {
            IValueWithScore nodeData = this.GetValue<IValueWithScore>(processor);
            if (nodeData != null) return nodeData;

            nodeData = new ValueUtilityBoardSubgraph();
            processor.RuntimeData.SetValue(this.Id, nodeData);
            
            return nodeData;
        }
        
        protected override Status Update(Processor processor, Graph graph)
        {
            Status status = this.GetStatus(processor);
            return status switch
            {
                Status.Success => Status.Success,
                Status.Failure => Status.Failure,
                _ => this.m_Graph.Run(processor)
            };
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