using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Behavior
{
    [Serializable]
    public abstract class TNodeActionPlan : TNode
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public abstract Beliefs Beliefs { get; }
        
        // ABSTRACT METHODS: ----------------------------------------------------------------------

        public abstract float GetCost(Args args);

        // PROTECTED METHODS: ---------------------------------------------------------------------
        
        protected override void Cancel(Processor processor, Graph graph)
        {
            if (this.GetStatus(processor) != Status.Running) return;
            processor.RuntimeData.SetStatus(this.Id, Status.Failure);
        }
    }
}