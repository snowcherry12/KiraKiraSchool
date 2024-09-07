using System;

namespace GameCreator.Runtime.Characters
{
    public abstract class TStance : IStance
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private bool m_IsBlocking;
        [NonSerialized] private float m_BlockStartTime;
        
        [NonSerialized] private float m_Defense;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        public abstract int Id { get; }
        public abstract Character Character { get; set; }

        [field: NonSerialized] protected bool IsEnabled { get; private set; }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public virtual void OnEnable(Character character)
        {
            this.IsEnabled = true;
        }

        public virtual void OnDisable(Character character)
        {
            this.IsEnabled = false;
        }
        
        public abstract void OnUpdate();
    }
}