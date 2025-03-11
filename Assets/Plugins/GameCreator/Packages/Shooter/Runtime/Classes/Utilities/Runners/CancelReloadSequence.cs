using System;
using GameCreator.Runtime.VisualScripting;

namespace GameCreator.Runtime.Shooter
{
    public class CancelReloadSequence : ICancellable
    {
        [field: NonSerialized] public CancelReason CancelReason { get; set; }
        public bool IsCancelled => this.CancelReason == CancelReason.ForceStop ||
                                   this.CancelReason == CancelReason.QuickReload;
    }
}