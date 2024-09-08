using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Behavior
{
    [Title("Fail")]
    [Category("Fail")]
    
    [Image(typeof(IconDecoratorFail), ColorTheme.Type.TextLight)]
    [Description("Automatically returns Failure after its child node finishes")]
    
    [Serializable]
    public class DecoratorFail : TDecorator
    {
        // OVERRIDE METHODS: ----------------------------------------------------------------------
        
        public override bool Conditions(TNode node, Processor processor, Graph graph)
        {
            return true;
        }

        public override Status Run(Status status, TNode node, Processor processor, Graph graph)
        {
            return status == Status.Running ? Status.Running : Status.Failure;
        }
    }
}