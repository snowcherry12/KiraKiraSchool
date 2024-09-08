using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    [Serializable]
    public class NodeStateMachineSubgraph : TNodeStateMachine
    {
        public const string TYPE_ID = "state-machine:subgraph";
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private Check m_Check = Check.EveryCycle;
        [SerializeField] private RunConditionsList m_Conditions = new RunConditionsList();

        [SerializeField] private Graph m_Graph;
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override PropertyName TypeId => TYPE_ID;

        public override Graph Subgraph => this.m_Graph;

        // INPUT METHODS: -------------------------------------------------------------------------

        protected override TInputPort[] CreateInputs() => new TInputPort[]
        {
            new InputPortStateMachineMultiple()
        };

        protected override TOutputPort[] CreateOutputs() => new TOutputPort[]
        {
            new OutputPortStateMachineMultiple()
        };

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public override void CopyFrom(object source)
        {
            base.CopyFrom(source);
            if (source is not NodeStateMachineSubgraph sourceNode) return;
            
            this.m_Check = sourceNode.m_Check;
            this.m_Conditions = sourceNode.m_Conditions;
            this.m_Graph = sourceNode.m_Graph;
        }
        
        public override bool TryRun(Processor processor, Graph graph, BeforeRunHandle beforeEnter)
        {
            if (!this.m_Conditions.Check(processor.Args)) return false;
            
            beforeEnter?.Invoke(processor);
            this.Run(processor, graph);
            
            return true;
        }

        // PROTECTED METHODS: ---------------------------------------------------------------------
        
        protected override Status Update(Processor processor, Graph graph)
        {
            this.SetAsCurrentIndex(processor, graph);
            
            Status status = this.GetStatus(processor);
            Status subgraphStatus = this.m_Graph.Run(processor);

            if (status == Status.Running && subgraphStatus != Status.Running)
            {
                if (this.m_Check == Check.EveryCycle)
                {
                    TOutputPort outputs = this.Ports.Outputs[0];
                    foreach (Connection connection in outputs.Connections)
                    {
                        if (graph.GetFromPortId(connection) is not TNodeStateMachine candidate) continue;
                        if (candidate.TryRun(processor, graph, this.OnExit))
                        {
                            this.Abort(processor, graph);
                            return Status.Ready;
                        }
                    }
                }
            }

            if (subgraphStatus == Status.Running)
            {
                if (this.m_Check == Check.EveryFrame)
                {
                    TOutputPort outputs = this.Ports.Outputs[0]; 
                    foreach (Connection connection in outputs.Connections)
                    {
                        if (graph.GetFromPortId(connection) is not TNodeStateMachine candidate) continue;
                        if (candidate.TryRun(processor, graph, this.OnExit))
                        {
                            this.Abort(processor, graph);
                            return Status.Ready;
                        }
                    }
                }
                
                return Status.Running;
            }
            
            this.m_Graph.Abort(processor);
            return Status.Running;
        }

        protected override void Cancel(Processor processor, Graph graph)
        {
            base.Cancel(processor, graph);
            
            if (this.m_Graph == null) return;
            this.m_Graph.Abort(processor);
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void OnExit(Processor processor)
        {
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