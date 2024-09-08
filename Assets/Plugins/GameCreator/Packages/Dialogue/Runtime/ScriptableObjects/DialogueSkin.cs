using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Common.Audio;
using GameCreator.Runtime.Dialogue.UnityUI;
using UnityEngine;

namespace GameCreator.Runtime.Dialogue
{
    [Icon(EditorPaths.PACKAGES + "Dialogue/Editor/Gizmos/GizmoDialogueSkin.png")]
    public class DialogueSkin : TSkin<GameObject>, ISerializationCallbackReceiver
    {
        private const string MSG = "A game object prefab with the Dialogue UI Skin";
        
        private const string ERR_NO_VALUE = "Prefab value cannot be empty";
        private const string ERR_COMP_UI = "Prefab does not contain a 'DialogueUI' component";

        public static readonly int ANIMATOR_OPEN = Animator.StringToHash("Open");
        public static readonly int ANIMATOR_CLOSE = Animator.StringToHash("Close");

        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private AnimatorOverrideController m_Controller;
        [SerializeField] private AnimationClip m_Idle;
        [SerializeField] private AnimationClip m_Open;
        [SerializeField] private AnimationClip m_Close;

        [SerializeField] private AudioClip m_Start;
        [SerializeField] private AudioClip m_Finish;
        [SerializeField] private AudioClip m_Select;
        [SerializeField] private AudioClip m_Submit;
        
        [SerializeField] private ValuesNodeChoices m_ValuesChoices = new ValuesNodeChoices();
        [SerializeField] private ValuesNodeRandom m_ValuesRandom = new ValuesNodeRandom();

        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private DialogueUI m_Instance;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override string Description => MSG;

        public override string HasError
        {
            get
            {
                if (this.Value == null) return ERR_NO_VALUE;
                return !this.Value.Get<DialogueUI>() ? ERR_COMP_UI : string.Empty;
            }
        }
        
        public RuntimeAnimatorController Controller => this.m_Controller;
        
        public float DurationOpen => this.m_Open != null ? this.m_Open.length : 0f;
        public float DurationClose => this.m_Close != null ? this.m_Close.length : 0f;
        
        public ValuesNodeChoices ValuesChoices => this.m_ValuesChoices;
        public ValuesNodeRandom ValuesRandom => this.m_ValuesRandom;
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public DialogueUI RequireSkin()
        {
            if (this.m_Instance != null) return this.m_Instance;

            GameObject instance = Instantiate(this.Value);
            this.m_Instance = instance.Get<DialogueUI>();
            
            if (this.m_Instance == null)
            {
                Debug.LogError(ERR_COMP_UI);
                return null;
            }
            
            instance.SetActive(false);
            return this.m_Instance;
        }

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
        
        public void PlayClipSelect()
        {
            if (this.m_Start == null) return;

            _ = AudioManager.Instance.UserInterface.Play(
                this.m_Select, 
                AudioConfigSoundUI.Default, 
                Args.EMPTY
            );
        }
        
        public void PlayClipSubmit()
        {
            if (this.m_Start == null) return;

            _ = AudioManager.Instance.UserInterface.Play(
                this.m_Submit, 
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
            this.m_Controller["Dialogue@Idle"] = this.m_Idle;
            this.m_Controller["Dialogue@Open"] = this.m_Open;
            this.m_Controller["Dialogue@Close"] = this.m_Close;
            
            #endif
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        { }
    }
}