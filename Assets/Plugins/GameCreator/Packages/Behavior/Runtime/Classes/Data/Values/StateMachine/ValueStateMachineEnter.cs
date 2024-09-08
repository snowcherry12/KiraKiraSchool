using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Behavior
{
    public class ValueStateMachineEnter : IValue
    {
        [field: NonSerialized] public IdString CurrentNodeId { get; set; }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public void Restart()
        {
            this.CurrentNodeId = default;
        }
    }
}