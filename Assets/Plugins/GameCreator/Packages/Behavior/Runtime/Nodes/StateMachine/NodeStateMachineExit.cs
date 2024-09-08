using System;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    [Serializable]
    public class NodeStateMachineExit : TNodeStateMachine
    {
        public const string TYPE_ID = "state-machine:exit";
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override PropertyName TypeId => TYPE_ID;

        // INPUT METHODS: -------------------------------------------------------------------------

        protected override TInputPort[] CreateInputs() => new TInputPort[]
        {
            new InputPortStateMachineMultiple()
        };
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public override bool TryRun(Processor processor, Graph graph, BeforeRunHandle beforeEnter)
        {
            beforeEnter?.Invoke(processor);
            this.Run(processor, graph);
            
            return true;
        }

        // PROTECTED METHODS: ---------------------------------------------------------------------
        
        protected override Status Update(Processor processor, Graph graph)
        {
            this.SetAsCurrentIndex(processor, graph);
            return Status.Success;
        }
    }
}