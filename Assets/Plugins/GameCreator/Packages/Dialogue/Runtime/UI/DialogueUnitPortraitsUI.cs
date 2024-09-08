using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Common.UnityUI;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace GameCreator.Runtime.Dialogue.UnityUI
{
    [Icon(EditorPaths.PACKAGES + "Dialogue/Editor/Gizmos/GizmoDialogueUI.png")]
    [AddComponentMenu("Game Creator/UI/Dialogue/Unit Portraits UI")]
    
    public class DialogueUnitPortraitsUI : TDialogueUnitUI
    {
        private const float TIME_FADE = 0.15f;
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private GameObject m_Active;

        [SerializeField] private bool m_ShowListeners = true;
        [SerializeField] private Color m_ListenerTint = new Color(0.5f, 0.5f, 0.5f);
        
        [SerializeField] private GameObject m_ActivePrimary;
        [SerializeField] private Image m_PrimaryPortrait;
        [SerializeField] private TextReference m_PrimaryActorName = new TextReference();
        [SerializeField] private TextReference m_PrimaryActorDescription = new TextReference();
        
        [SerializeField] private GameObject m_ActiveAlternate;
        [SerializeField] private Image m_AlternatePortrait;
        [SerializeField] private TextReference m_AlternateActorName = new TextReference();
        [SerializeField] private TextReference m_AlternateActorDescription = new TextReference();

        [SerializeField] private AnimationClip m_ClipOnChange;

        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private AnimColor m_AnimColorPrimary;
        [NonSerialized] private AnimColor m_AnimColorAlternate;

        [NonSerialized] private int m_PreviousActorPrimaryHash;
        [NonSerialized] private int m_PreviousActorAlternateHash;
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        private bool PrimaryHasSprite => 
            this.m_PrimaryPortrait != null &&
            this.m_PrimaryPortrait.overrideSprite != null &&
            this.m_PrimaryPortrait.overrideSprite != this.m_PrimaryPortrait.sprite;
        
        private bool AlternateHasSprite => 
            this.m_AlternatePortrait != null &&
            this.m_AlternatePortrait.overrideSprite != null &&
            this.m_AlternatePortrait.overrideSprite != this.m_AlternatePortrait.sprite;

        // OVERRIDE METHODS: ----------------------------------------------------------------------

        public override void OnAwake(DialogueUI dialogueUI)
        {
            base.OnAwake(dialogueUI);
            if (this.m_Active != null) this.m_Active.SetActive(false);

            this.m_AnimColorPrimary = new AnimColor(Color.white, TIME_FADE);
            this.m_AnimColorAlternate = new AnimColor(Color.white, TIME_FADE);
        }
        
        public override void OnReset(bool isNew)
        {
            if (!isNew) return;
            
            if (this.m_PrimaryPortrait != null) this.m_PrimaryPortrait.overrideSprite = null;
            if (this.m_AlternatePortrait != null) this.m_AlternatePortrait.overrideSprite = null;

            this.m_AnimColorPrimary.Current = Color.white;
            this.m_AnimColorPrimary.Target = Color.white;
            
            this.m_AnimColorAlternate.Current = Color.white;
            this.m_AnimColorAlternate.Target = Color.white;

            this.m_PreviousActorPrimaryHash = -1;
            this.m_PreviousActorAlternateHash = -1;

            this.m_PrimaryActorName.Text = string.Empty;
            this.m_AlternateActorName.Text = string.Empty;
            this.m_PrimaryActorDescription.Text = string.Empty;
            this.m_AlternateActorDescription.Text = string.Empty;
        }
        
        public override void OnStartNext(Story story, int nodeId, Args args)
        {
            if (ApplicationManager.IsExiting) return;
            
            Node node = story.Content.Get(nodeId);
            if (node == null) return;
            
            if (this.m_Active != null) this.m_Active.SetActive(false);
            
            Expression expression = node.Actor != null 
                ? node.Actor.GetExpressionFromIndex(node.Expression) 
                : null;

            if (node.Actor == null || expression == null)
            {
                this.RefreshPrimaryAsListener();
                this.RefreshAlternateAsListener();
            }
            else
            {
                Portrait portrait = node.Portrait != PortraitMode.ActorDefault
                    ? (Portrait) node.Portrait
                    : node.Actor != null
                        ? node.Actor.Portrait
                        : Portrait.None;
                
                switch (portrait)
                {
                    case Portrait.None:
                        this.RefreshPrimaryAsListener();
                        this.RefreshAlternateAsListener();
                        break;
                    
                    case Portrait.Primary:
                        this.RefreshPrimaryAsSpeaker(node, args);
                        this.RefreshAlternateAsListener();
                        break;
                
                    case Portrait.Alternate:
                        this.RefreshPrimaryAsListener();
                        this.RefreshAlternateAsSpeaker(node, args);
                        break;

                    default: throw new ArgumentOutOfRangeException();
                }
            }
            
            node.EventStartText -= this.OnStartText;
            node.EventFinishType -= this.OnFinishText;
            
            node.EventStartText += this.OnStartText;
            node.EventFinishType += this.OnFinishText;
        }

        public override void OnFinishNext(Story story, int nodeId, Args args)
        {
            Node node = story.Content.Get(nodeId);
            if (node == null) return;
            
            node.EventStartText -= this.OnStartText;
            node.EventFinishType -= this.OnFinishText;
        }
        
        // UPDATE METHODS: ------------------------------------------------------------------------

        private void Update()
        {
            this.m_AnimColorPrimary.UpdateWithDelta(Time.unscaledDeltaTime);
            this.m_AnimColorAlternate.UpdateWithDelta(Time.unscaledDeltaTime);

            if (this.m_PrimaryPortrait != null)
            {
                this.m_PrimaryPortrait.color = this.m_AnimColorPrimary.Current;
            }
            
            if (this.m_AlternatePortrait != null)
            {
                this.m_AlternatePortrait.color = this.m_AnimColorAlternate.Current;
            }
        }

        // CALLBACK METHODS: ----------------------------------------------------------------------

        private void OnStartText(int nodeId)
        {
            if (this.m_Active != null) this.m_Active.SetActive(true);
        }

        private void OnFinishText(int nodeId)
        {
            if (this.m_Active != null) this.m_Active.SetActive(false);
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void RefreshPrimaryAsSpeaker(Node node, Args args)
        {
            Expression expression = node.Actor != null 
                ? node.Actor.GetExpressionFromIndex(node.Expression) 
                : null;
            
            Sprite expressionSprite = expression?.GetSprite(args);
            
            if (this.m_PrimaryPortrait != null)
            {
                this.m_PrimaryPortrait.overrideSprite = expressionSprite;
                this.m_AnimColorPrimary.Target = Color.white;
            }
            
            if (this.m_ActivePrimary != null)
            {
                if (this.m_ShowListeners)
                {
                    this.m_ActivePrimary.SetActive(this.PrimaryHasSprite);
                }
                else
                {
                    this.m_ActivePrimary.SetActive(expressionSprite != null);
                }
            }

            this.m_PrimaryActorName.Text = node.Actor != null 
                ? node.Actor.GetName(args)
                : string.Empty;

            this.m_PrimaryActorDescription.Text = node.Actor != null
                ? node.Actor.GetDescription(args)
                : string.Empty;
            
            int newHash = node.Actor != null ? node.Actor.GetHashCode() : 0;
            Animator animator = this.m_ActivePrimary.Get<Animator>();
            bool differentHash = this.m_PreviousActorPrimaryHash != newHash;

            if (this.m_ClipOnChange != null && animator != null && differentHash)
            {
                AnimationPlayableUtilities.PlayClip(
                    animator, 
                    this.m_ClipOnChange, 
                    out PlayableGraph _
                );
            }

            this.m_PreviousActorPrimaryHash = newHash;
        }

        private void RefreshAlternateAsSpeaker(Node node, Args args)
        {
            Expression expression = node.Actor != null 
                ? node.Actor.GetExpressionFromIndex(node.Expression) 
                : null;

            Sprite expressionSprite = expression?.GetSprite(args);
            
            if (this.m_AlternatePortrait != null)
            {
                this.m_AlternatePortrait.overrideSprite = expressionSprite;
                this.m_AnimColorAlternate.Target = Color.white;
            }
            
            if (this.m_ActiveAlternate != null)
            {
                if (this.m_ShowListeners)
                {
                    this.m_ActiveAlternate.SetActive(this.AlternateHasSprite);
                }
                else
                {
                    this.m_ActiveAlternate.SetActive(expressionSprite != null);
                }
            }

            this.m_AlternateActorName.Text = node.Actor != null
                ? node.Actor.GetName(args) 
                : string.Empty;
            
            this.m_AlternateActorDescription.Text = node.Actor != null 
                ? node.Actor.GetDescription(args) 
                : string.Empty;

            int newHash = node.Actor != null ? node.Actor.GetHashCode() : 0;
            Animator animator = this.m_ActiveAlternate.Get<Animator>();
            bool differentHash = this.m_PreviousActorAlternateHash != newHash;

            if (this.m_ClipOnChange != null && animator != null && differentHash)
            {
                AnimationPlayableUtilities.PlayClip(
                    animator, 
                    this.m_ClipOnChange, 
                    out PlayableGraph _
                );
            }

            this.m_PreviousActorAlternateHash = newHash;
        }
        
        private void RefreshPrimaryAsListener()
        {
            if (this.m_ShowListeners)
            {
                bool hasSprite = this.PrimaryHasSprite;
                
                if (this.m_ActivePrimary != null) this.m_ActivePrimary.SetActive(hasSprite);
                if (this.m_PrimaryPortrait != null && hasSprite)
                {
                    this.m_AnimColorPrimary.Target = this.m_ListenerTint;
                }
            }
            else
            {
                if (this.m_ActivePrimary != null) this.m_ActivePrimary.SetActive(false);
            }
        }
        
        private void RefreshAlternateAsListener()
        {
            if (this.m_ShowListeners)
            {
                bool hasSprite = this.AlternateHasSprite;

                if (this.m_ActiveAlternate != null) this.m_ActiveAlternate.SetActive(hasSprite);
                if (this.m_AlternatePortrait != null && hasSprite)
                {
                    this.m_AnimColorAlternate.Target = this.m_ListenerTint;
                }
            }
            else
            {
                if (this.m_ActiveAlternate != null) this.m_ActiveAlternate.SetActive(false);
            }
        }
    }
}