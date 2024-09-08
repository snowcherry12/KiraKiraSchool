using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Dialogue.UnityUI
{
    [Icon(EditorPaths.PACKAGES + "Dialogue/Editor/Gizmos/GizmoDialogueUI.png")]
    [AddComponentMenu("Game Creator/UI/Dialogue/Dialogue UI")]
    
    public class DialogueUI : MonoBehaviour
    {
        #if UNITY_EDITOR

        [UnityEditor.InitializeOnEnterPlayMode]
        private static void OnEnterPlayMode()
        {
            Current = null;
            IsOpen = false;
            EventStart = null;
            EventFinish = null;
        }
        
        #endif

        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private RectTransform m_SpeechContainer;
        [SerializeField] private SpeechSkin m_DefaultSpeech;

        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private Dialogue m_Dialogue;
        [NonSerialized] private SpeechUI m_SpeechUI;

        [NonSerialized] private Args m_Args;
        [NonSerialized] private bool m_IsClosing;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        [field: NonSerialized] public static DialogueUI Current { get; private set; }
        [field: NonSerialized] public static bool IsOpen { get; private set; }

        [field: NonSerialized] public DialogueSkin DialogueSkin { get; private set; }
        [field: NonSerialized] public SpeechSkin SpeechSkin { get; private set; }

        // EVENTS: --------------------------------------------------------------------------------

        public static event Action EventStart;
        public static event Action EventFinish;
        
        public event Action<Story, int, Args> EventOnStartNext;
        public event Action<Story, int, Args> EventOnFinishNext;
        
        // INITIALIZERS: --------------------------------------------------------------------------

        private void Awake()
        {
            TDialogueUnitUI[] units = this.GetComponentsInChildren<TDialogueUnitUI>(true);
            foreach (TDialogueUnitUI unit in units)
            {
                unit.OnAwake(this);
            }
        }

        // PUBLIC STATIC METHODS: -----------------------------------------------------------------

        public static async Task Open(DialogueSkin dialogueSkin, Dialogue dialogue, bool isNew)
        {
            if (dialogueSkin == null) return;
            if (dialogue == null) return;

            if (Current != null)
            {
                if (IsOpen) Current.m_Dialogue.Stop();
                while (!ApplicationManager.IsExiting && Current.m_IsClosing)
                {
                    await Task.Yield();
                }
            }

            Current = dialogueSkin.RequireSkin();
            Current.DialogueSkin = dialogueSkin;
            Current.m_Dialogue = dialogue;

            dialogue.EventFinish += Current.OnStop;
            dialogue.EventStartNext += Current.OnStartNext;
            dialogue.EventFinishNext += Current.OnFinishNext;

            TDialogueUnitUI[] units = Current.GetComponentsInChildren<TDialogueUnitUI>(true);
            foreach (TDialogueUnitUI unit in units)
            {
                unit.OnReset(isNew);
            }

            IsOpen = true;

            Current.m_Args = new Args(dialogue.gameObject, Current.gameObject);
            Current.gameObject.SetActive(true);
            
            dialogueSkin.PlayClipStart();
            
            Animator animator = Current.Get<Animator>();
            if (animator != null)
            {
                animator.runtimeAnimatorController = dialogueSkin.Controller;
                animator.SetTrigger(DialogueSkin.ANIMATOR_OPEN);

                TimeMode time = Current.m_Dialogue.Story.Time;
                float timeout = time.Time + dialogueSkin.DurationOpen;
                
                while (!ApplicationManager.IsExiting && time.Time < timeout)
                {
                    await Task.Yield();
                }
            }
            
            EventStart?.Invoke();
        }

        // CALLBACKS: -----------------------------------------------------------------------------

        private void OnStop()
        {
            if (ApplicationManager.IsExiting) return;

            if (this.m_Dialogue != null)
            {
                this.m_Dialogue.EventFinish -= this.OnStop;
                this.m_Dialogue.EventStartNext -= this.OnStartNext;
                this.m_Dialogue.EventFinishNext -= this.OnFinishNext;
            }
            
            IsOpen = false;

            Animator animator = this.Get<Animator>();
            if (animator != null) animator.SetTrigger(DialogueSkin.ANIMATOR_CLOSE);
            
            this.DialogueSkin.PlayClipFinish();

            EventFinish?.Invoke();
            
            float duration = this.DialogueSkin.DurationClose;
            _ = this.CloseUI(duration);
        }

        private async Task CloseUI(float duration)
        {
            this.m_IsClosing = true;

            TimeMode time = this.m_Dialogue.Story.Time;
            float timeout = time.Time + duration;
            
            while (!ApplicationManager.IsExiting && time.Time < timeout)
            {
                await Task.Yield();
            }
            
            this.gameObject.SetActive(false);
            this.m_IsClosing = false;
        }

        private void OnStartNext(int nodeId)
        {
            if (ApplicationManager.IsExiting) return;

            Node node = this.m_Dialogue.Story.Content.Get(nodeId);

            SpeechSkin speechSkin = this.m_DefaultSpeech;

            if (node?.Actor != null)
            {
                if (node.Actor.OverrideSpeechSkin != null)
                {
                    speechSkin = node.Actor.OverrideSpeechSkin;
                }

                Expression expression = node.Actor.GetExpressionFromIndex(node.Expression);
                if (expression?.OverrideSpeechSkin != null)
                {
                    speechSkin = expression.OverrideSpeechSkin;
                }
            }

            if (this.SpeechSkin != speechSkin)
            {
                for (int i = this.m_SpeechContainer.childCount - 1; i >= 0; --i)
                {
                    Destroy(this.m_SpeechContainer.GetChild(i).gameObject);
                }

                GameObject instance = UIUtils.Instantiate(speechSkin.Value, this.m_SpeechContainer);

                this.SpeechSkin = speechSkin;
                this.m_SpeechUI = instance.Get<SpeechUI>();

                if (this.m_SpeechUI != null) this.m_SpeechUI.OnAwake(this);

                RuntimeAnimatorController controller = speechSkin.Controller; 
                Animator animator = instance.Get<Animator>();
                
                if (animator != null && animator.runtimeAnimatorController != controller)
                {
                    animator.runtimeAnimatorController = controller;
                }
            }

            TDialogueUnitUI[] units = this.GetComponentsInChildren<TDialogueUnitUI>(true);
            foreach (TDialogueUnitUI unit in units)
            {
                unit.OnStartNext(this.m_Dialogue.Story, nodeId, this.m_Args);
            }
            
            this.EventOnStartNext?.Invoke(this.m_Dialogue.Story, nodeId, this.m_Args);
        }

        private void OnFinishNext(int nodeId)
        {
            if (ApplicationManager.IsExiting) return;
            
            TDialogueUnitUI[] units = this.GetComponentsInChildren<TDialogueUnitUI>(true);
            foreach (TDialogueUnitUI unit in units)
            {
                unit.OnFinishNext(this.m_Dialogue.Story, nodeId, this.m_Args);
            }
            
            this.EventOnFinishNext?.Invoke(this.m_Dialogue.Story, nodeId, this.m_Args);
        }
    }
}