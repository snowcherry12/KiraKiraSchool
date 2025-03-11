using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [CreateAssetMenu(
        fileName = "Reload", 
        menuName = "Game Creator/Shooter/Reload",
        order    = 50
    )]
    
    [Icon(EditorPaths.PACKAGES + "Shooter/Editor/Gizmos/GizmoReload.png")]
    
    [Serializable]
    public class Reload : ScriptableObject, IStageGizmos
    {
        private enum ReloadMode
        {
            Maximum,
            Value
        }
        
        private const int INFINITY_AMMO = 9999999;
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private PropertyGetString m_Title = GetStringString.Create;
        [SerializeField] private PropertyGetString m_Description = GetStringEmpty.Create;

        [SerializeField] private PropertyGetSprite m_Icon = GetSpriteNone.Create;
        [SerializeField] private PropertyGetColor m_Color = GetColorColorsWhite.Create;
        
        [SerializeField] private AnimationClip m_Animation;
        [SerializeField] private AvatarMask m_Mask;
        
        [SerializeField] private float m_TransitionIn = 0.1f;
        [SerializeField] private float m_TransitionOut = 0.25f;
        [SerializeField] private bool m_UseRootMotion;
        
        [SerializeField] private RunReloadSequence m_ReloadSequence = new RunReloadSequence();
        [SerializeField] private PropertyGetDecimal m_Speed = GetDecimalConstantOne.Create;

        [SerializeField] private PropertyGetBool m_DiscardMagazineAmmo = GetBoolFalse.Create;
        [SerializeField] private ReloadMode m_Reload = ReloadMode.Maximum;
        [SerializeField] private PropertyGetInteger m_ReloadAmount = new PropertyGetInteger(
            new GetDecimalConstantOne()
        );
        
        [SerializeField] private RunInstructionsList m_OnStart = new RunInstructionsList();
        [SerializeField] private RunInstructionsList m_OnFinish = new RunInstructionsList();
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public AnimationClip Animation => this.m_Animation;
        public AvatarMask Mask => this.m_Mask;
        
        public float TransitionIn => this.m_TransitionIn;
        public float TransitionOut => this.m_TransitionOut;

        public float Duration => this.m_Animation != null ? this.m_Animation.length : 0f;
        
        [field: SerializeField] public string EditorModelPath { get; set; }
        [field: SerializeField] public string EditorWeaponPath { get; set; }
        [field: SerializeField] public string EditorWeaponBone { get; set; }
        [field: SerializeField] public Vector3 EditorWeaponLocalPosition { get; set; }
        [field: SerializeField] public Quaternion EditorWeaponLocalRotation { get; set; }

        // GETTERS: -------------------------------------------------------------------------------
        
        public string GetName(Args args) => this.m_Title.Get(args);
        public string GetDescription(Args args) => this.m_Description.Get(args);
        
        public Sprite GetSprite(Args args) => this.m_Icon.Get(args);
        public Color GetColor(Args args) => this.m_Color.Get(args);
        
        public float GetSpeed(Args args) => (float) this.m_Speed.Get(args);
        
        public bool GetDiscardMagazineAmmo(Args args) => this.m_DiscardMagazineAmmo.Get(args);
        
        public int GetReloadAmount(Args args) => this.m_Reload switch
        {
            ReloadMode.Maximum => INFINITY_AMMO,
            ReloadMode.Value => (int) this.m_ReloadAmount.Get(args),
            _ => throw new ArgumentOutOfRangeException()
        };

        // INTERNAL METHODS: ----------------------------------------------------------------------

        internal async Task<bool> Play(Character character, float speed, CancelReloadSequence cancel, Args args)
        {
            if (this.m_Animation == null) return false;
            if (ApplicationManager.IsExiting) return false;
            
            _ = this.m_OnStart.Run(args);
            
            float duration = this.m_Animation.length;
            
            ConfigGesture gestureConfig = new ConfigGesture(
                0f, duration, speed, this.m_UseRootMotion,
                this.m_TransitionIn, this.m_TransitionOut
            );

            _ = character.Gestures.CrossFade(
                this.m_Animation, this.m_Mask, BlendMode.Blend, 
                gestureConfig, true
            );
            
            await this.m_ReloadSequence.Run(
                TextUtils.Humanize(this.name),
                character.Time, 
                speed,
                this.m_Animation, 
                cancel,
                args
            );
            
            _ = this.m_OnFinish.Run(args);

            return true;
        }

        internal bool CanQuickReload(float t)
        {
            TrackReloadQuick track = this.m_ReloadSequence.GetTrack<TrackReloadQuick>();
            
            if (track == null || track.Clips.Length == 0) return false;
            if (track.Clips[0] is not ClipReloadQuick quick) return false;
            
            return t >= quick.TimeStart && t <= quick.TimeEnd;
        }

        public Vector2 GetQuickReload()
        {
            TrackReloadQuick track = this.m_ReloadSequence.GetTrack<TrackReloadQuick>();
            
            if (track == null || track.Clips.Length == 0) return Vector2.zero;
            return track.Clips[0] is ClipReloadQuick quick 
                ? new Vector2(quick.TimeStart, quick.TimeEnd)
                : Vector2.zero;
        }
        
        internal void Stop(Character character, CancelReloadSequence cancel, Args args, CancelReason reason)
        {
            cancel.CancelReason = reason;
            if (reason == CancelReason.PartialReload) return;
            
            character.Gestures.Stop(this.m_Animation, 0f, this.m_TransitionOut);
        }
        
        // STAGE GIZMOS: --------------------------------------------------------------------------

        void IStageGizmos.StageGizmos(StagingGizmos stagingGizmos)
        { }
    }
}