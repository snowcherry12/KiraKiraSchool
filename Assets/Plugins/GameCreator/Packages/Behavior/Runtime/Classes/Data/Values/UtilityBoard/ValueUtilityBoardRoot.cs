using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Behavior
{
    public class ValueUtilityBoardRoot : IValue
    {
        [field: NonSerialized] public IdString RunningNodeId { get; set; }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public void Restart()
        {
            this.RunningNodeId = IdString.EMPTY;
        }
    }
}