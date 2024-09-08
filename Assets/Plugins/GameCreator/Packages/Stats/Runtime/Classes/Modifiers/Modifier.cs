using System;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Serializable]
    public struct Modifier
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private int m_StatID;
        [SerializeField] private double m_Value;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public int StatID => this.m_StatID;
        public double Value => this.m_Value;
        
        public Modifier Clone => new Modifier(this.m_StatID, this.m_Value);
        
        // CONSTRUCTORS: --------------------------------------------------------------------------

        public Modifier(int statID, double value)
        {
            this.m_StatID = statID;
            this.m_Value = value;
        }
    }
}