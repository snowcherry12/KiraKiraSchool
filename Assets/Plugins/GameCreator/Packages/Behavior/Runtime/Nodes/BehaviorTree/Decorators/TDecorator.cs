using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Behavior
{
    [Title("Decorator")]
    
    [Serializable]
    public abstract class TDecorator
    {
        // ABSTRACT METHODS: ----------------------------------------------------------------------

        public abstract bool Conditions(TNode node, Processor processor, Graph graph);
        public abstract Status Run(Status status, TNode node, Processor processor, Graph graph);
    }
}