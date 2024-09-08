using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    [Serializable]
    public class NodeStateMachineConditions : TNodeStateMachine
    {
        public const string TYPE_ID = "state-machine:conditions";
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private string m_Name = "My Conditions";
        [SerializeField] private RunConditionsList m_Conditions = new RunConditionsList();

        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override PropertyName TypeId => TYPE_ID;

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
            if (source is not NodeStateMachineConditions sourceNode) return;
            
            this.m_Name = sourceNode.m_Name;
            this.m_Conditions = sourceNode.m_Conditions;
        }

        public override bool TryRun(Processor processor, Graph graph, BeforeRunHandle beforeEnter)
        {
            if (!this.m_Conditions.Check(processor.Args)) return false;
            
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
        
        // STRING: --------------------------------------------------------------------------------
        
        public override string ToString() => $"{this.m_Name}:{this.Id}";
    }
}