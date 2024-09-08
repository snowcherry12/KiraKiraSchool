using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Serializable]
    public class SkillStrike
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private MeleeDirection m_Direction = MeleeDirection.Forward;
        [SerializeField] private int m_Predictions = 1;
        
        [SerializeField] private MeleeStrikers m_UseStrikers = MeleeStrikers.All;
        [SerializeField] private IdString m_Id = new IdString("striker-id");
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public MeleeDirection Direction => this.m_Direction;
        public int Predictions => this.m_Predictions;
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public bool UseStriker(Striker striker)
        {
            return this.m_UseStrikers switch
            {
                MeleeStrikers.All => true,
                MeleeStrikers.None => false,
                MeleeStrikers.ById => striker.Id == this.m_Id.Hash,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}