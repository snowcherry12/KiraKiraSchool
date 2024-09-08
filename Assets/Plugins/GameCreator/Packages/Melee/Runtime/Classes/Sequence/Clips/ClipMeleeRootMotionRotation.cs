using System;
using GameCreator.Runtime.VisualScripting;

namespace GameCreator.Runtime.Melee
{
    [Serializable]
    public class ClipMeleeRootMotionRotation : Clip
    {
        // CONSTRUCTORS: --------------------------------------------------------------------------

        public ClipMeleeRootMotionRotation() : this(DEFAULT_TIME)
        { }

        public ClipMeleeRootMotionRotation(float time) : base(time, 0f)
        { }
    }
}