using System;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Serializable]
    public class AttributeData
    {
        [SerializeField] private double m_MinValue;
        [SerializeField] private Stat m_MaxValue;
        
        [SerializeField] [Range(0f, 1f)] private float m_StartPercent = 1f;
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public double MinValue => this.m_MinValue;
        public Stat MaxValue => this.m_MaxValue;
        
        public float StartPercent => this.m_StartPercent;
    }
}