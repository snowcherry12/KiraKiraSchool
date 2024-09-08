using System;
using GameCreator.Runtime.VisualScripting;

namespace GameCreator.Runtime.Melee
{
    [Serializable]
    public class ClipMeleePhases : Clip
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override float MinDuration => 0.01f;

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public ClipMeleePhases() : this(DEFAULT_TIME, DEFAULT_DURATION)
        { }

        public ClipMeleePhases(float time, float duration) : base(time, duration)
        { }
    }
}