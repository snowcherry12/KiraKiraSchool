using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Common.UnityUI;
using UnityEngine;
using UnityEngine.UI;

namespace GameCreator.Runtime.Dialogue.UnityUI
{
    [Icon(EditorPaths.PACKAGES + "Dialogue/Editor/Gizmos/GizmoSpeechUI.png")]
    [AddComponentMenu("Game Creator/UI/Dialogue/Speech UI")]
    
    public class SpeechUI : TDialogueUnitUI
    {
        #if UNITY_EDITOR

        [UnityEditor.InitializeOnEnterPlayMode]
        private static void OnEnterPlayMode()
        {
            Current = null;
        }
        
        #endif
        
        private const float TIME_SAFEGUARD = 0.25f;
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private GameObject m_Active;
        
        [SerializeField] private GameObject m_ActiveActor;
        [SerializeField] private TextReference m_ActorName = new TextReference();
        [SerializeField] private TextReference m_ActorDescription = new TextReference();
        
        [SerializeField] private GameObject m_ActivePortrait;
        [SerializeField] private Image m_Portrait;
        
        [SerializeField] private TextReference m_Text = new TextReference();
        [SerializeField] private GameObject m_Skip;

        // MEMBERS: -------------------------------------------------------------------------------
        
        [NonSerialized] private Story m_Story;
        [NonSerialized] private int m_NodeId;
        [NonSerialized] private Args m_Args;

        [NonSerialized] private bool m_IsActive;
        [NonSerialized] private float m_StartTime;

        [NonSerialized] private int m_PreviousActorHash;
        [NonSerialized] private bool m_RunningChoice;
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        [field: NonSerialized] public static SpeechUI Current { get; private set; }
        
        // INITIALIZE METHODS: --------------------------------------------------------------------

        private void Awake()
        {
            Current = this;
        }
        
        private void OnDestroy()
        {
            Current = null;
        }

        // OVERRIDE METHODS: ----------------------------------------------------------------------

        public override void OnReset(bool isNew)
        {
            if (!isNew) return;
            this.m_PreviousActorHash = -1;
            this.m_RunningChoice = false;
        }

        public override void OnStartNext(Story story, int nodeId, Args args)
        {
            if (ApplicationManager.IsExiting) return;

            this.m_Story = story;
            this.m_NodeId = nodeId;
            this.m_Args = args;

            this.m_IsActive = false;
            if (this.m_Active != null) this.m_Active.SetActive(false);
            
            Node node = story.Content.Get(nodeId);

            if (node == null) return;

            node.EventStartText -= this.OnStartText;
            node.EventStartChoice -= this.OnStartChoice;
            node.EventFinishType -= this.OnFinishText;

            node.EventStartText += this.OnStartText;
            node.EventStartChoice += this.OnStartChoice;
            node.EventFinishType += this.OnFinishText;
        }

        public override void OnFinishNext(Story story, int nodeId, Args args)
        {
            if (ApplicationManager.IsExiting) return;
            
            Node node = story.Content.Get(nodeId);
            if (node == null) return;
            
            node.EventStartText -= this.OnStartText;
            node.EventStartChoice -= this.OnStartChoice;
            node.EventFinishType -= this.OnFinishText;
        }

        // UPDATE: --------------------------------------------------------------------------------

        private void Update()
        {
            if (!this.m_IsActive) return;
            
            Node node = this.m_Story?.Content.Get(this.m_NodeId);
            if (node == null) return;

            int visibleCharacters = node.Actor != null
                ? node.Actor.Typewriter.GetCharactersVisible(this.m_StartTime, this.m_Story.Time)
                : int.MaxValue;
            
            this.m_Text.CharactersVisible = visibleCharacters;
            if (this.m_Skip != null)
            {
                bool skipActive = node.Duration == NodeDuration.UntilInteraction &&
                                  this.m_Text.AreAllCharactersVisible &&
                                  this.m_StartTime + TIME_SAFEGUARD < this.m_Story.Time.Time &&
                                  !this.m_RunningChoice;
                
                this.m_Skip.SetActive(skipActive);
            }
        }

        // CALLBACK METHODS: ----------------------------------------------------------------------

        private void OnStartText(int nodeId)
        {
            if (ApplicationManager.IsExiting) return;
            
            Node node = this.m_Story.Content.Get(nodeId);
            if (node == null) return;
            
            if (this.m_DialogueUI.SpeechSkin != null)
            {
                this.m_DialogueUI.SpeechSkin.PlayClipStart();
            }

            if (this.m_ActiveActor != null)
            {
                this.m_ActiveActor.gameObject.SetActive(node.Actor != null);
            
                if (node.Actor != null)
                {
                    this.m_ActorName.Text = node.Actor.GetName(this.m_Args);
                    this.m_ActorDescription.Text = node.Actor.GetDescription(this.m_Args);   
                }
            }
            
            Expression expression = node.Actor != null 
                ? node.Actor.GetExpressionFromIndex(node.Expression) 
                : null;

            Sprite expressionSprite = expression?.GetSprite(this.m_Args);
            
            if (this.m_ActivePortrait != null)
            {
                Portrait portrait = node.Portrait != PortraitMode.ActorDefault
                    ? (Portrait) node.Portrait
                    : node.Actor != null
                        ? node.Actor.Portrait
                        : Portrait.None;
                
                bool showPortrait = expressionSprite != null && portrait != Portrait.None;
                this.m_ActivePortrait.SetActive(showPortrait);
            }

            if (this.m_Portrait != null)
            {
                this.m_Portrait.overrideSprite = expressionSprite;
            }

            this.m_Text.Text = node.Text;
            this.m_Text.CharactersVisible = 0;
            
            if (this.m_Skip != null) this.m_Skip.SetActive(false);
            if (this.m_Active != null) this.m_Active.SetActive(true);
            
            this.m_StartTime = this.m_Story.Time.Time;
            this.m_IsActive = true;
            
            Animator animator = this.Get<Animator>();
            bool animatorCorrect = animator != null && animator.runtimeAnimatorController != null;
            int actorHash = node.Actor != null ? node.Actor.GetHashCode() : 0;
            
            if (animatorCorrect)
            {
                bool animate = this.m_DialogueUI.SpeechSkin.AnimateWhen switch
                {
                    SpeechSkin.AnimationWhen.NewSpeaker => this.m_PreviousActorHash != actorHash,
                    SpeechSkin.AnimationWhen.Always => true,
                    _ => throw new ArgumentOutOfRangeException()
                };
                
                if (animate) animator.SetTrigger(SpeechSkin.ANIMATOR_OPEN);
            }

            this.m_PreviousActorHash = actorHash;
            this.m_RunningChoice = false;
        }
        
        private void OnStartChoice(int nodeId)
        {
            this.m_RunningChoice = true;
        }

        private void OnFinishText(int nodeId)
        {
            if (ApplicationManager.IsExiting) return;
            
            if (this.m_DialogueUI.SpeechSkin != null)
            {
                this.m_DialogueUI.SpeechSkin.PlayClipFinish();
            }
            
            if (this.m_Active != null) this.m_Active.SetActive(false);
            this.m_IsActive = false;
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Skip()
        {
            if (this.m_StartTime + TIME_SAFEGUARD > this.m_Story.Time.Time) return;

            Node node = this.m_Story?.Content.Get(this.m_NodeId);
            if (node == null) return;
            
            if (node.Actor != null)
            {
                float duration = node.Actor.Typewriter.GetDuration(node.Text);
                if (this.m_Story.Time.Time < this.m_StartTime + duration)
                {
                    this.m_StartTime = -9999f;
                    this.m_Text.CharactersVisible = node.Text.Length;

                    this.m_Story.StopTypewriter();
                    return;
                }
            }

            this.m_Story.Continue();
        }
    }
}