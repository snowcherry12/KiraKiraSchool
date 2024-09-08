using System;
using GameCreator.Runtime.Characters;
using UnityEngine;

namespace GameCreator.Runtime.Dialogue
{
    [Serializable]
    public class NodeAnimation
    {
        [SerializeField] private AvatarMask m_AvatarMask = null;
        [SerializeField] private BlendMode m_BlendMode = BlendMode.Blend;
        
        [SerializeField] private float m_Delay = 0f;
        [SerializeField] private float m_Speed = 1f;
        [SerializeField] private bool m_UseRootMotion = false;
        [SerializeField] private float m_TransitionIn = 0.1f;
        [SerializeField] private float m_TransitionOut = 0.1f;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public AvatarMask AvatarMask => this.m_AvatarMask;
        public BlendMode BlendMode => this.m_BlendMode;
        
        public float Delay => this.m_Delay;
        public float Speed => this.m_Speed;
        public bool UseRootMotion => this.m_UseRootMotion;
        public float TransitionIn => this.m_TransitionIn;
        public float TransitionOut => this.m_TransitionOut;
    }
}