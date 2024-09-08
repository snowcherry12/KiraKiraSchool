using System;

namespace GameCreator.Runtime.Melee
{
    public readonly struct ComboItemParent
    {
        // PROPERTIES: ----------------------------------------------------------------------------

        [field: NonSerialized] public MeleeKey Key { get; }
        [field: NonSerialized] public MeleeMode Mode { get; }
        [field: NonSerialized] public MeleeExecute When { get; }

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public ComboItemParent(MeleeKey key, MeleeMode mode, MeleeExecute when)
        {
            this.Key = key;
            this.Mode = mode;
            this.When = when;
        }
    }
}