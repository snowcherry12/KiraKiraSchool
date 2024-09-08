using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Serializable]
    public struct RuntimeStatusEffectValue
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private IdString m_ID;
        
        [SerializeField] private float m_TimeElapsed;
        [SerializeField] private float m_TimeRemaining;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public RuntimeStatusEffectValue(string id, float timeElapsed, float timeRemaining)
        {
            this.m_ID = new IdString(id);
            
            this.m_TimeElapsed = timeElapsed;
            this.m_TimeRemaining = timeRemaining;
        }

        // PROPERTIES: ----------------------------------------------------------------------------

        public float TimeElapsed => this.m_TimeElapsed;
        public float TimeRemaining => this.m_TimeRemaining;

        public bool HasDuration => this.TimeRemaining >= 0f;
        public float GetDuration => this.TimeElapsed + this.TimeRemaining;
        
        public float Progress => this.TimeElapsed / (this.TimeElapsed + this.TimeRemaining);
    }
}