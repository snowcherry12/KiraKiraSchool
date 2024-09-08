using System;
using GameCreator.Runtime.VisualScripting;

namespace GameCreator.Runtime.Melee
{
    [Serializable]
    public class ClipMeleeCancel : Clip
    {
        // CONSTRUCTORS: --------------------------------------------------------------------------

        public ClipMeleeCancel() : this(DEFAULT_TIME - DEFAULT_PAD, DEFAULT_DURATION + DEFAULT_PAD * 2f)
        { }

        public ClipMeleeCancel(float time) : base(time, 0f)
        { }
        
        public ClipMeleeCancel(float time, float duration) : base(time, duration)
        { }
    }
}