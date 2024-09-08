using System;

namespace GameCreator.Runtime.Stats
{
    public enum StatusEffectType
    {
        Positive = 1, // Mask 0x001
        Negative = 2, // Mask 0x010
        Neutral  = 4  // Mask 0x100
    }
    
    [Flags]
    public enum StatusEffectTypeMask
    {
        Positive = StatusEffectType.Positive, // Mask 0x001
        Negative = StatusEffectType.Negative, // Mask 0x010
        Neutral  = StatusEffectType.Neutral   // Mask 0x100
    }
}