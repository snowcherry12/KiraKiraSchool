using System;
using GameCreator.Runtime.Characters;

namespace GameCreator.Runtime.Shooter
{
    public class Shooting
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private float m_FinishFireAnimationTime;
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        [field: NonSerialized] private Character Character { get; set; }

        public bool IsShootingAnimation => this.m_FinishFireAnimationTime > this.Character.Time.Time;
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public Shooting(Character character)
        {
            this.Character = character;
        }
        
        // INTERNAL METHODS: ----------------------------------------------------------------------

        internal void OnShoot(float duration)
        {
            this.m_FinishFireAnimationTime = Math.Max(
                this.Character.Time.Time + duration,
                this.m_FinishFireAnimationTime
            );
        }
    }
}