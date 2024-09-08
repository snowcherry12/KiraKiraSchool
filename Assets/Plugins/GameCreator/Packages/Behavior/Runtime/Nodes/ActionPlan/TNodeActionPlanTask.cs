using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    [Serializable]
    public abstract class TNodeActionPlanTask : TNodeActionPlan
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] protected PropertyGetDecimal m_Cost = GetDecimalDecimal.Create(1f);
        
        [SerializeField] protected RunConditionsList m_Conditions = new RunConditionsList();

        // PROPERTIES: ----------------------------------------------------------------------------

        public override Beliefs Beliefs => null;

        public string Title => $"{this.Name} = {this.m_Cost}";
        
        protected abstract string Name { get; }

        // PORT METHODS: --------------------------------------------------------------------------

        protected override TOutputPort[] CreateOutputs() => new TOutputPort[]
        {
            new OutputPortActionPlanPreConditions(),
            new OutputPortActionPlanPostConditions()
        };

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public override void CopyFrom(object source)
        {
            base.CopyFrom(source);
            if (source is not TNodeActionPlanTask sourceNode) return;
            
            this.m_Cost = sourceNode.m_Cost;
            this.m_Conditions = sourceNode.m_Conditions;
        }
        
        public override float GetCost(Args args) => (float) this.m_Cost.Get(args);

        public bool CheckConditions(Args args) => this.m_Conditions.Check(args);

        public Beliefs GetPreConditions(Graph graph)
        {
            Connection[] preConnections = this.Ports.Outputs[0].Connections;
            NodeActionPlanPreConditions nodePreConditions = preConnections.Length != 0
                ? graph.GetFromPortId(preConnections[0]) as NodeActionPlanPreConditions
                : null;

            return nodePreConditions?.Beliefs;
        }
        
        public Beliefs GetPostConditions(Graph graph)
        {
            Connection[] postConnections = this.Ports.Outputs[1].Connections;
            NodeActionPlanPostConditions nodePostConditions = postConnections.Length != 0
                ? graph.GetFromPortId(postConnections[0]) as NodeActionPlanPostConditions
                : null;

            return nodePostConditions?.Beliefs;
        }
        
        // INTERNAL METHODS: ----------------------------------------------------------------------

        internal bool ResolveBy(State conditions, Graph graph) {

            Belief[] beliefs = this.GetPreConditions(graph)?.List ?? Array.Empty<Belief>();
            foreach (Belief belief in beliefs) 
            {
                if (belief.Value)
                {
                    if (!conditions.Get(belief.Name.String)) 
                    {
                        return false;
                    }
                }
                else
                {
                    if (conditions.Get(belief.Name.String)) 
                    {
                        return false;
                    }
                }
            }
            
            return true;
        }
        
        // PROTECTED METHODS: ---------------------------------------------------------------------
        
        protected void SetAsCurrentIndex(Processor processor, Graph graph)
        {
            ValueStateMachineEnter value = this.GetValue<ValueStateMachineEnter>(processor);
            value ??= new ValueStateMachineEnter();
            
            value.CurrentNodeId = this.Id;
            
            TNode enterNode = graph.Nodes[StateMachine.INDEX_ENTER];
            processor.RuntimeData.SetValue(enterNode.Id, value);
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private bool HasBelief(Beliefs beliefs, string name)
        {
            foreach (Belief belief in beliefs.List)
            {
                if (belief.Name.String == name && belief.Value) return true;
            }

            return false;
        }
    }
}