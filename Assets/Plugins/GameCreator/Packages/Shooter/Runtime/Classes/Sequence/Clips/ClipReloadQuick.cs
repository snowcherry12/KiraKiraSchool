using System;
using GameCreator.Runtime.VisualScripting;

namespace GameCreator.Runtime.Shooter
{
    [Serializable]
    public class ClipReloadQuick : Clip
    {
        // CONSTRUCTORS: --------------------------------------------------------------------------

        public ClipReloadQuick() : this(DEFAULT_TIME - DEFAULT_PAD, DEFAULT_DURATION + DEFAULT_PAD * 2f)
        { }

        public ClipReloadQuick(float time) : base(time, 0f)
        { }
        
        public ClipReloadQuick(float time, float duration) : base(time, duration)
        { }
    }
}