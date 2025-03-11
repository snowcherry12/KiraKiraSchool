using System;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Serializable]
    public class Animations
    {
        [field: SerializeField] private AnimationClip m_Idle;
        [field: SerializeField] private AnimationClip m_Empty;
         
        [field: SerializeField] private AnimationClip m_Shoot;
         
        [field: SerializeField] private AnimationClip m_ReloadQuick;
        [field: SerializeField] private AnimationClip m_ReloadDry;
         
        [field: SerializeField] private AnimationClip m_JamEnter;
        [field: SerializeField] private AnimationClip m_JamExit;
        [field: SerializeField] private AnimationClip m_Jammed;
        
        [field: SerializeField] private AnimationClip m_ChargeProgressMin;
        [field: SerializeField] private AnimationClip m_ChargeProgressMax;
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public AnimationClip Idle => this.m_Idle;
        public AnimationClip Empty => this.m_Empty;
        
        public AnimationClip Shoot => this.m_Shoot;
        
        public AnimationClip ReloadQuick => this.m_ReloadQuick;
        public AnimationClip ReloadDry => this.m_ReloadDry;
        
        public AnimationClip JamEnter => this.m_JamEnter;
        public AnimationClip JamExit => this.m_JamExit;
        public AnimationClip Jammed => this.m_Jammed;
        
        public AnimationClip ChargeProgressMin => this.m_ChargeProgressMin;
        public AnimationClip ChargeProgressMax => this.m_ChargeProgressMax;
    }
}