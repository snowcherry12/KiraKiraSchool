using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Behavior
{
    [Title("While Fail")]
    [Category("While Fail")]
    
    [Image(typeof(IconDecoratorWhileFail), ColorTheme.Type.TextLight)]
    [Description("Allows to execute its child node while it keeps returning Failure")]
    
    [Serializable]
    public class DecoratorWhileFail : TDecorator
    {
        // OVERRIDE METHODS: ----------------------------------------------------------------------
        
        public override bool Conditions(TNode node, Processor processor, Graph graph)
        {
            Status status = node.GetStatus(processor);
            return status != Status.Success;
        }

        public override Status Run(Status status, TNode node, Processor processor, Graph graph)
        {
            if (status != Status.Failure) return status;
            
            node.ClearNodes(processor, graph);
            return Status.Running;
        }
    }
}