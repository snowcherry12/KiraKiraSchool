using GameCreator.Runtime.Common;
using GameCreator.Runtime.Common.Audio;
using GameCreator.Runtime.Dialogue.UnityUI;
using UnityEngine;

namespace GameCreator.Runtime.Dialogue
{
    [Icon(EditorPaths.PACKAGES + "Dialogue/Editor/Gizmos/GizmoSpeechSkin.png")]

    public class SpeechSkin : TSkin<GameObject>, ISerializationCallbackReceiver
    {
        private const string MSG = "A game object prefab with the Speech UI skin for a speech";
        
        private const string ERR_NO_VALUE = "Prefab value cannot be empty";
        private const string ERR_COMP_UI = "Prefab does not contain a 'SpeechUI' component";
        
        public static readonly int ANIMATOR_OPEN = Animator.StringToHash("Open");
        
        // ENUMS: ---------------------------------------------------------------------------------

        public enum AnimationWhen
        {
            NewSpeaker,
            Always
        }

        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private AnimatorOverrideController m_Controller;
        [SerializeField] private AnimationWhen m_When;
        [SerializeField] private AnimationClip m_Idle;
        [SerializeField] private AnimationClip m_Open;
        
        [SerializeField] private AudioClip m_Start;
        [SerializeField] private AudioClip m_Finish;

        [SerializeField] private GameObject m_Log;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        public RuntimeAnimatorController Controller => this.m_Controller;
        
        public override string Description => MSG;

        public override string HasError
        {
            get
            {
                if (this.Value == null) return ERR_NO_VALUE;
                return !this.Value.Get<SpeechUI>() ? ERR_COMP_UI : string.Empty;
            }
        }

        public AnimationWhen AnimateWhen => this.m_When;
        
        public GameObject OverrideLog => this.m_Log;
        
        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public void PlayClipStart()
        {
            if (this.m_Start == null) return;

            _ = AudioManager.Instance.UserInterface.Play(
                this.m_Start, 
                AudioConfigSoundUI.Default, 
                Args.EMPTY
            );
        }
        
        public void PlayClipFinish()
        {
            if (this.m_Start == null) return;
            
            _ = AudioManager.Instance.UserInterface.Play(
                this.m_Finish, 
                AudioConfigSoundUI.Default, 
                Args.EMPTY
            );
        }
        
        // SERIALIZATION CALLBACKS: ---------------------------------------------------------------
        
        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            #if UNITY_EDITOR
            
            if (AssemblyUtils.IsReloading) return;
            if (this.m_Controller == null) return;
            this.m_Controller["Speech@Idle"] = this.m_Idle;
            this.m_Controller["Speech@Open"] = this.m_Open;

            #endif
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        { }
    }
}