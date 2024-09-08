using System;

namespace GameCreator.Runtime.Melee
{
    public readonly struct AttackDuration
    {
        // PROPERTIES: ----------------------------------------------------------------------------

        [field: NonSerialized] public float Anticipation { get; }
        [field: NonSerialized] public float Strike       { get; }
        [field: NonSerialized] public float Recovery     { get; }

        public float Total => this.Anticipation + this.Strike + this.Recovery;
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public AttackDuration(float anticipation, float strike, float recovery)
        {
            this.Anticipation = anticipation;
            this.Strike = strike;
            this.Recovery = recovery;
        }
    }
}