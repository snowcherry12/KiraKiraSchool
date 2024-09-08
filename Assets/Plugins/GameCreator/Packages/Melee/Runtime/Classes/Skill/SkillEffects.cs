using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Serializable]
    public class SkillEffects
    {
        internal static readonly Vector2 PITCH = new Vector2(0.9f, 1.1f);
        
        public const int POOL_COUNT = 5;
        public const float POOL_DURATION = 5f;

        private enum SkillTimeMode
        {
            Unscaled,
            CasterTime
        }
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetAudio m_SoundUse = GetAudioNone.Create;
        [SerializeField] private PropertyGetAudio m_SoundStrike = GetAudioNone.Create;
        [SerializeField] private PropertyGetAudio m_SoundHit = GetAudioNone.Create;
        [SerializeField] private PropertyGetAudio m_SoundBlocked = GetAudioNone.Create;
        [SerializeField] private PropertyGetAudio m_SoundParried = GetAudioNone.Create;
        
        [SerializeField] private PropertyGetGameObject m_HitEffect = GetGameObjectNone.Create();
        
        [SerializeField] private bool m_HitPause = true;
        [SerializeField] private float m_HitPauseTimeScale = 0.05f;
        [SerializeField] private float m_HitPauseDelay = 0.1f;
        [SerializeField] private float m_HitPauseDuration = 0.1f;
        [SerializeField] private SkillTimeMode m_TimeScale = SkillTimeMode.Unscaled;

        // PROPERTIES: ----------------------------------------------------------------------------

        public bool HitPause => this.m_HitPause;
        public float HitPauseTimeScale => Math.Max(this.m_HitPauseTimeScale, 0f);
        public float HitPauseDelay => Math.Max(this.m_HitPauseDelay, 0f);
        public float HitPauseDuration => Math.Max(this.m_HitPauseDuration, 0f);

        public bool UseUnscaledTime => this.m_TimeScale == SkillTimeMode.Unscaled;
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public AudioClip GetSoundUse(Args args) => this.m_SoundUse.Get(args);
        public AudioClip GetSoundStrike(Args args) => this.m_SoundStrike.Get(args);
        public AudioClip GetSoundHit(Args args) => this.m_SoundHit.Get(args);
        public AudioClip GetSoundBlocked(Args args) => this.m_SoundBlocked.Get(args);
        public AudioClip GetSoundParried(Args args) => this.m_SoundParried.Get(args);
        
        public GameObject GetHitEffect(Args args) => this.m_HitEffect.Get(args);
        
        // PUBLIC STATIC METHODS: -----------------------------------------------------------------

        internal static Quaternion GetRotation(Vector3 direction)
        {
            return direction.magnitude > 0f
                ? Quaternion.LookRotation(
                    -direction, 
                    direction.normalized != Vector3.up
                        ? Vector3.up
                        : Vector3.right
                )
                : Quaternion.identity;
        }
    }
}