using System;

namespace GameCreator.Runtime.Melee
{
    [Flags]
    public enum MeleePhaseMask
    {
        None         = MeleePhase.None,
        Reaction     = MeleePhase.Reaction,
        Charge       = MeleePhase.Charge,
        Anticipation = MeleePhase.Anticipation,
        Strike       = MeleePhase.Strike,
        Recovery     = MeleePhase.Recovery
    }
}