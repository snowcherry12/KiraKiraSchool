using System;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    [Serializable]
    public class NodeUtilityBoardRoot : TNodeUtilityBoard
    {
        public const string TYPE_ID = "utility-board:root";

        // PROPERTIES: ----------------------------------------------------------------------------

        public override PropertyName TypeId => TYPE_ID;

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public override float RecalculateScore(Processor processor, Graph graph)
        {
            return float.MinValue;
        }
        
        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected override IValueWithScore RequireData(Processor processor) => null;

        protected override Status Update(Processor processor, Graph graph)
        {
            return Status.Success;
        }

        protected override void Cancel(Processor processor, Graph graph)
        { }

        // STRING: --------------------------------------------------------------------------------

        public override string ToString() => $"Root:{this.Id}";
    }
}