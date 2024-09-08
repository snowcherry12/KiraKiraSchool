using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Behavior
{
    [Title("Composite")]
    
    [Serializable]
    public abstract class TComposite
    {
        // ABSTRACT METHODS: ----------------------------------------------------------------------
        
        public abstract Status Run(TNode node, Processor processor, Graph graph);
    }
}