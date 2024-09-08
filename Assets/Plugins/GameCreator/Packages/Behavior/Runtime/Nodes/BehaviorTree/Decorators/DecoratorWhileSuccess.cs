using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Behavior
{
    [Title("While Success")]
    [Category("While Success")]
    
    [Image(typeof(IconDecoratorWhileSuccess), ColorTheme.Type.TextLight)]
    [Description("Allows to execute its child node while it keeps returning Success")]
    
    [Serializable]
    public class DecoratorWhileSuccess : TDecorator
    {
        // OVERRIDE METHODS: ----------------------------------------------------------------------
        
        public override bool Conditions(TNode node, Processor processor, Graph graph)
        {
            Status status = node.GetStatus(processor);
            return status != Status.Failure;
        }

        public override Status Run(Status status, TNode node, Processor processor, Graph graph)
        {
            if (status != Status.Success) return status;
            
            node.ClearNodes(processor, graph);
            return Status.Running;
        }
    }
}