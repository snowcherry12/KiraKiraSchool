using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Serializable]
    public class StatusEffectData
    {
        [SerializeField] private StatusEffectType m_Type = StatusEffectType.Positive;
        [SerializeField] private PropertyGetInteger m_MaxStack = new PropertyGetInteger(1);
        
        [SerializeField] private bool m_HasDuration;
        [SerializeField] private bool m_IsHidden;
        [SerializeField] private PropertyGetDecimal m_Duration = new PropertyGetDecimal(60f);

        // PROPERTIES: ----------------------------------------------------------------------------

        public StatusEffectType Type => this.m_Type;
        
        public bool HasDuration => this.m_HasDuration;

        public bool IsHidden => this.m_IsHidden;
        
        // METHODS: -------------------------------------------------------------------------------

        public double GetDuration(Args args)
        {
            return this.HasDuration 
                ? this.m_Duration.Get(args) 
                : -1;
        }

        public int GetMaxStack(Args args)
        {
            float maxStack = (float) this.m_MaxStack.Get(args);
            return Mathf.FloorToInt(maxStack);
        }
    }
}