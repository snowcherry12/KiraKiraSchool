using System;

namespace GameCreator.Runtime.Behavior
{
    [Serializable]
    public abstract class TNodeStateMachine : TNode
    {
        public delegate void BeforeRunHandle(Processor processor);
        
        // PROTECTED METHODS: ---------------------------------------------------------------------
        
        protected override void Cancel(Processor processor, Graph graph)
        {
            if (this.GetStatus(processor) != Status.Running) return;
            processor.RuntimeData.SetStatus(this.Id, Status.Success);
        }

        protected void SetAsCurrentIndex(Processor processor, Graph graph)
        {
            ValueStateMachineEnter value = this.GetValue<ValueStateMachineEnter>(processor);
            value ??= new ValueStateMachineEnter();

            value.CurrentNodeId = this.Id;
            
            TNode enterNode = graph.Nodes[StateMachine.INDEX_ENTER];
            processor.RuntimeData.SetValue(enterNode.Id, value);
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public abstract bool TryRun(Processor processor, Graph graph, BeforeRunHandle beforeEnter);
    }
}