using System;

namespace GameCreator.Runtime.Behavior
{
    public class ValueUtilityBoardSubgraph : IValueWithScore
    {
        [field: NonSerialized] public float Score { get; set; }
        
        public void Restart()
        {
            this.Score = 0f;
        }
    }
}