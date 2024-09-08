using System;
using GameCreator.Runtime.VisualScripting;

namespace GameCreator.Runtime.Melee
{
    public class CancelMeleeSequence : ICancellable
    {
        [field: NonSerialized] public bool IsCancelled { get; set; }
    }
}