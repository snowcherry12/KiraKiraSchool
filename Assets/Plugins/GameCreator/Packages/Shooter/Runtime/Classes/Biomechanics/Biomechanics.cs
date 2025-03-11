using System;
using GameCreator.Runtime.Characters;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Serializable]
    public class Biomechanics
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeReference] private TBiomechanics m_Value = new BiomechanicsHumanIK();

        // PROPERTIES: ----------------------------------------------------------------------------
        
        public TBiomechanics Value => this.m_Value;
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Enter(Character character, ShooterWeapon weapon, float enterDuration)
        {
            this.m_Value.Enter(character, weapon, enterDuration);
        }

        public void Exit(Character character, ShooterWeapon weapon, float exitDuration)
        {
            this.m_Value.Exit(character, weapon, exitDuration);
        }
    }
}