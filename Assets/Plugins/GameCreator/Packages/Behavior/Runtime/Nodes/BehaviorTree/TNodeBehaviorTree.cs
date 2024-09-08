using System;

namespace GameCreator.Runtime.Behavior
{
    [Serializable]
    public abstract class TNodeBehaviorTree : TNode
    {
        protected override void Cancel(Processor processor, Graph graph)
        {
            if (this.GetStatus(processor) != Status.Running) return;
            processor.RuntimeData.SetStatus(this.Id, Status.Failure);
        }
    }
}