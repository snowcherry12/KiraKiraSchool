using System;
using GameCreator.Runtime.Characters;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Serializable]
    public class ShooterMunition : TMunitionValue
    {
        [SerializeField] private int m_InMagazine;
        [SerializeField] private int m_Total;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public int InMagazine
        {
            get => this.m_InMagazine;
            set
            {
                this.m_InMagazine = Math.Max(value, 0);
                this.ExecuteEventChange();
            }
        }

        public int Total
        {
            get => this.m_Total;
            set
            {
                this.m_Total = Math.Max(value, 0);
                this.ExecuteEventChange();
            }
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public override object Clone()
        {
            return new ShooterMunition
            {
                m_InMagazine = this.m_InMagazine,
                m_Total = this.m_Total
            };
        }
        
        // TO STRING: -----------------------------------------------------------------------------
        
        public override string ToString()
        {
            return $"{this.m_InMagazine} / {this.m_Total}";
        }
    }
}