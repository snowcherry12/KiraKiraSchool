using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Serializable]
    public class Jam
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetDecimal m_Chance = GetDecimalConstantZero.Create;
        
        [SerializeField] private PropertyGetAnimation m_Animation = GetAnimationNone.Create;
        [SerializeField] private AvatarMask m_Mask;
        
        [SerializeField] private PropertyGetDecimal m_Speed = GetDecimalConstantOne.Create;
        
        [SerializeField] private float m_TransitionIn = 0.1f;
        [SerializeField] private float m_TransitionOut = 0.25f;

        [SerializeField] private PropertyGetAudio m_Audio = GetAudioClip.Create;
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public AvatarMask Mask => this.m_Mask;
        
        public float TransitionIn => this.m_TransitionIn;
        public float TransitionOut => this.m_TransitionOut;
        
        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public AnimationClip GetAnimation(Args args)
        {
            return this.m_Animation.Get(args);
        }
        
        public float GetSpeed(Args args)
        {
            return (float) this.m_Speed.Get(args);
        }
        
        public AudioClip GetAudio(Args args)
        {
            return this.m_Audio.Get(args);
        }

        public bool Run(Args args, bool isJammed)
        {
            if (isJammed) return true;
            
            float jamChance = (float) this.m_Chance.Get(args);
            if (jamChance <= float.Epsilon) return false;
            
            return UnityEngine.Random.value < jamChance;
        }
        
        // GIZMOS: --------------------------------------------------------------------------------
        
        public void StageGizmos(StagingGizmos stagingGizmos)
        { }
    }
}