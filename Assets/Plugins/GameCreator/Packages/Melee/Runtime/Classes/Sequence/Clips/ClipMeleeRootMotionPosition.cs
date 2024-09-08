using System;
using GameCreator.Runtime.VisualScripting;

namespace GameCreator.Runtime.Melee
{
    [Serializable]
    public class ClipMeleeRootMotionPosition : Clip
    {
        // CONSTRUCTORS: --------------------------------------------------------------------------

        public ClipMeleeRootMotionPosition() : this(DEFAULT_TIME)
        { }

        public ClipMeleeRootMotionPosition(float time) : base(time, 0f)
        { }
    }
}