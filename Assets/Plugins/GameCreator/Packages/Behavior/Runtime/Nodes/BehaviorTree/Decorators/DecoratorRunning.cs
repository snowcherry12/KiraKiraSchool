using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Behavior
{
    [Title("Running")]
    [Category("Running")]
    
    [Image(typeof(IconDecoratorRunning), ColorTheme.Type.TextLight)]
    [Description("Allows to execute its child node always")]
    
    [Serializable]
    public class DecoratorRunning : TDecorator
    {
        // OVERRIDE METHODS: ----------------------------------------------------------------------
        
        public override bool Conditions(TNode node, Processor processor, Graph graph)
        {
            return true;
        }

        public override Status Run(Status status, TNode node, Processor processor, Graph graph)
        {
            if (status != Status.Success && status != Status.Failure) return status;
            
            node.ClearNodes(processor, graph);
            return Status.Running;
        }
    }
}