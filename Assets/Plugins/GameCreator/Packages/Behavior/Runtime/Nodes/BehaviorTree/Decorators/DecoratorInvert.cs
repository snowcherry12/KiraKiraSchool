using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Behavior
{
    [Title("Invert")]
    [Category("Invert")]
    
    [Image(typeof(IconDecoratorInvert), ColorTheme.Type.TextLight)]
    [Description("Returns the opposite Success/Failure result when its child node finishes")]
    
    [Serializable]
    public class DecoratorInvert : TDecorator
    {
        // OVERRIDE METHODS: ----------------------------------------------------------------------
        
        public override bool Conditions(TNode node, Processor processor, Graph graph)
        {
            return true;
        }

        public override Status Run(Status status, TNode node, Processor processor, Graph graph)
        {
            return status switch
            {
                Status.Ready => Status.Ready,
                Status.Running => Status.Running,
                Status.Success => Status.Failure,
                Status.Failure => Status.Success,
                _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
            };
        }
    }
}