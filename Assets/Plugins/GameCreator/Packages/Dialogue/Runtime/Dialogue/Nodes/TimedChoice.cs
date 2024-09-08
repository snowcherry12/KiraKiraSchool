using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Dialogue
{
    [Serializable]
    public class TimedChoice
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private bool m_TimedChoice = false;
        
        [SerializeField] private PropertyGetDecimal m_Duration = GetDecimalDecimal.Create(10f);
        [SerializeField] private TimeoutBehavior m_Timeout = TimeoutBehavior.ChooseRandom;
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public bool IsTimed => this.m_TimedChoice;

        public TimeoutBehavior Timeout => this.m_Timeout;
        
        // GETTER METHODS: ------------------------------------------------------------------------

        public float GetDuration(Args args) => (float) this.m_Duration.Get(args);
    }
}