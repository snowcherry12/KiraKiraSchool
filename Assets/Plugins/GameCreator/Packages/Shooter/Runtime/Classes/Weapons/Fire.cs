using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Serializable]
    public class Fire
    {
        [SerializeField]
        private PropertyGetInteger m_ProjectilesPerShot = new PropertyGetInteger(
            new GetDecimalConstantOne()
        );
        
        [SerializeField]
        private PropertyGetInteger m_CartridgesPerShot = new PropertyGetInteger(
            new GetDecimalConstantOne()
        );
        
        [SerializeField]
        private ShootMode m_Mode = ShootMode.Single;

        [SerializeField]
        private FullAutoLoading m_AutoLoading = FullAutoLoading.Instant;

        [SerializeField]
        private PropertyGetDecimal m_AutoLoadDuration = GetDecimalConstantOne.Create;
        
        [SerializeField]
        private PropertyGetDecimal m_FireRate = GetDecimalDecimal.Create(10f);
        
        [SerializeField]
        private PropertyGetInteger m_Burst = GetDecimalInteger.Create(3);
        
        [SerializeField]
        private PropertyGetDecimal m_MinChargeTime = GetDecimalConstantZero.Create;

        [SerializeField]
        private PropertyGetDecimal m_MaxChargeTime = GetDecimalConstantOne.Create;

        [SerializeField]
        private PropertyGetBool m_AutoRelease = GetBoolFalse.Create;

        [SerializeField]
        private PropertyGetDecimal m_Duration = GetDecimalDecimal.Create(2f);

        [SerializeField]
        private PropertyGetAnimation m_FireAnimation = GetAnimationNone.Create;
        
        [SerializeField]
        private AvatarMask m_FireAvatarMask;

        [SerializeField] private float m_TransitionIn = 0.1f;
        [SerializeField] private float m_TransitionOut = 0.25f;
        [SerializeField] private bool m_RootMotion;

        [SerializeField] private PropertyGetAudio m_FireAudio = GetAudioClip.Create;
        [SerializeField] private PropertyGetAudio m_EmptyAudio = GetAudioClip.Create;
        
        [SerializeField] private PropertyGetAudio m_LoadStartAudio = GetAudioNone.Create;
        [SerializeField] private PropertyGetAudio m_LoadLoopAudio = GetAudioNone.Create;
        [SerializeField] private PropertyGetDecimal m_LoadMinPitch = GetDecimalConstantOne.Create;
        [SerializeField] private PropertyGetDecimal m_LoadMaxPitch = GetDecimalConstantOnePointFive.Create;

        [SerializeField] private PropertyGetInstantiate m_MuzzleEffect = new PropertyGetInstantiate();

        [SerializeField] private EnablerFloat m_Force = new EnablerFloat(false, 10f);
        [SerializeField] private PropertyGetDecimal m_Power = GetDecimalConstantOne.Create;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public ShootMode Mode => this.m_Mode;

        public FullAutoLoading AutoLoading => this.m_AutoLoading;

        public AvatarMask FireAvatarMask => this.m_FireAvatarMask;

        public bool ForceEnabled => this.m_Force.IsEnabled;
        
        public float Force => this.m_Force.IsEnabled ? this.m_Force.Value : 0f;
        
        // GETTER METHODS: ------------------------------------------------------------------------

        public int ProjectilesPerShot(Args args) => (int) this.m_ProjectilesPerShot.Get(args);
        
        public int CartridgesPerShot(Args args) => (int) this.m_CartridgesPerShot.Get(args);
        
        public float AutoLoadDuration(Args args) => (float) this.m_AutoLoadDuration.Get(args);
        
        public float FireRate(Args args) => (float) this.m_FireRate.Get(args);

        public int Burst(Args args) => (int) this.m_Burst.Get(args);
        
        public float MinChargeTime(Args args) => (float) this.m_MinChargeTime.Get(args);
        
        public float MaxChargeTime(Args args) => (float) this.m_MaxChargeTime.Get(args);
        
        public bool AutoRelease(Args args) => this.m_AutoRelease.Get(args);
        
        public float ReleaseDuration(Args args) => (float) this.m_Duration.Get(args);

        public AnimationClip FireAnimation(Args args) => this.m_FireAnimation.Get(args);

        public ConfigGesture FireConfig(AnimationClip animationClip) => new ConfigGesture(
            0f,
            animationClip != null ? animationClip.length : 0f,
            1f, 
            this.m_RootMotion,
            this.m_TransitionIn,
            this.m_TransitionOut
        );
        
        public AudioClip FireAudio(Args args) => this.m_FireAudio.Get(args);
        
        public AudioClip EmptyAudio(Args args) => this.m_EmptyAudio.Get(args);
        
        public AudioClip LoadStartAudio(Args args) => this.m_LoadStartAudio.Get(args);
        public AudioClip LoadLoopAudio(Args args) => this.m_LoadLoopAudio.Get(args);
        
        public float LoadMinPitch(Args args) => (float) this.m_LoadMinPitch.Get(args);
        public float LoadMaxPitch(Args args) => (float) this.m_LoadMaxPitch.Get(args);
        
        public GameObject MuzzleEffect(Args args) => this.m_MuzzleEffect.Get(args);

        public double Power(Args args) => this.m_Power.Get(args);
        
        // GIZMOS: --------------------------------------------------------------------------------
        
        public void StageGizmos(StagingGizmos stagingGizmos)
        { }
    }
}