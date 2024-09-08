using System;

namespace GameCreator.Runtime.Melee
{
    public readonly struct ChargeMatch
    {
        // PROPERTIES: ----------------------------------------------------------------------------

        [field: NonSerialized] public int PreviousComboId { get; }
        [field: NonSerialized] public int ChargeId { get; }

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public ChargeMatch(int previousComboId, int chargeId)
        {
            this.PreviousComboId = previousComboId;
            this.ChargeId = chargeId;
        }
    }
}