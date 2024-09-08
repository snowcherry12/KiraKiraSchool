using System;

namespace GameCreator.Runtime.Melee
{
    public class Key
    {
        // PROPERTIES: ----------------------------------------------------------------------------

        [field: NonSerialized] public bool HasKey { get; private set; }
        [field: NonSerialized] public MeleeKey Value { get; private set; } = MeleeKey.A;

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public void Unset()
        {
            this.HasKey = false;
        }

        public void Set(MeleeKey key)
        {
            this.HasKey = true;
            this.Value = key;
        }
    }
}