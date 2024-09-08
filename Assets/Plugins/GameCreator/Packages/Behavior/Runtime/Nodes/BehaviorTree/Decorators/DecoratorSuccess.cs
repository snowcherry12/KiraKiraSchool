using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Behavior
{
    [Title("Success")]
    [Category("Success")]
    
    [Image(typeof(IconDecoratorSuccess), ColorTheme.Type.TextLight)]
    [Description("Automatically returns Success after its child node finishes")]
    
    [Serializable]
    public class DecoratorSuccess : TDecorator
    {
        // OVERRIDE METHODS: ----------------------------------------------------------------------
        
        public override bool Conditions(TNode node, Processor processor, Graph graph)
        {
            return true;
        }

        public override Status Run(Status status, TNode node, Processor processor, Graph graph)
        {
            return status == Status.Running ? Status.Running : Status.Success;
        }
    }
}