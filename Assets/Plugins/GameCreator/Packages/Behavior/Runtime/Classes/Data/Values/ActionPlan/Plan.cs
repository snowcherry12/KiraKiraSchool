using System;
using System.Collections.Generic;
using System.Text;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Behavior
{
    public class Plan
    {
        public static readonly Plan NONE = new Plan();

        private const float MIN_COST = 0f;
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private float m_Cost;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        [field: NonSerialized] public List<IdString> Sequence { get; }
        [field: NonSerialized] public State CompleteState { get; set; }

        public float Cost
        {
            set => this.m_Cost = Math.Max(MIN_COST, value);
        }
        
        public bool Exists => this.Sequence.Count != 0;
        
        public float Weight { get; private set; }

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public Plan()
        {
            this.Sequence = new List<IdString>();
            this.Cost = MIN_COST;
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Insert(IdString nodeId)
        {
            this.Sequence.Insert(0, nodeId);
        }

        public void Reset()
        {
            this.Sequence.Clear();
            this.Cost = 0f;
        }
        
        public void GenerateWeight(IEnumerable<Goal> goals)
        {
            this.Weight = 0f;
            if (!this.Exists) return;

            foreach (Goal goal in goals)
            {
                if (!this.CompleteState.Get(goal.Name.String)) continue;
                this.Weight += this.m_Cost > float.Epsilon ? goal.Weight / this.m_Cost : 0f;
            }
        }

        // STRING: --------------------------------------------------------------------------------

        public override string ToString()
        {
            StringBuilder text = new StringBuilder();
            foreach (IdString entry in this.Sequence)
            {
                text.AppendLine($"- {entry.String}");
            }

            return text.ToString();
        }
    }
}