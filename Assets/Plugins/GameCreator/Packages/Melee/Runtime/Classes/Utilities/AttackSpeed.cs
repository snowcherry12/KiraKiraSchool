using System;

namespace GameCreator.Runtime.Melee
{
    public readonly struct AttackSpeed
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        [field: NonSerialized] public float Anticipation { get; }
        [field: NonSerialized] public float Strike       { get; }
        [field: NonSerialized] public float Recovery     { get; }

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public AttackSpeed(float anticipation, float strike, float recovery)
        {
            this.Anticipation = anticipation;
            this.Strike = strike;
            this.Recovery = recovery;
        }
    }
}