using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    [Serializable]
    public class NodeActionPlanPreConditions : TNodeActionPlan
    {
        public const string TYPE_ID = "action-plan:pre-conditions";
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private Beliefs m_Beliefs = new Beliefs();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override PropertyName TypeId => TYPE_ID;
        
        public override Beliefs Beliefs => this.m_Beliefs;

        // PORT METHODS: --------------------------------------------------------------------------
        
        protected override TInputPort[] CreateInputs() => new TInputPort[]
        {
            new InputPortActionPlanPreConditions()
        };
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public override float GetCost(Args args) => 0f;
        
        protected override Status Update(Processor processor, Graph graph)
        {
            return Status.Success;
        }
        
        public override void CopyFrom(object source)
        {
            base.CopyFrom(source);
            if (source is not NodeActionPlanPreConditions sourceNode) return;
            
            this.m_Beliefs = sourceNode.m_Beliefs;
        }
    }
}