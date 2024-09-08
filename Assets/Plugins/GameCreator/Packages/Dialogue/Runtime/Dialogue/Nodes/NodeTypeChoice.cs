using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Dialogue
{
    [Title("Choice")]
    [Category("Choice")]
    
    [Image(typeof(IconNodeChoice), ColorTheme.Type.TextLight)]
    [Description("Lets the user choose an option from its children")]

    [Serializable]
    public class NodeTypeChoice : TNodeType
    {
        public static readonly string NAME_SKIP_CHOICE = nameof(m_SkipChoice);
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private bool m_HideUnavailable;
        [SerializeField] private bool m_HideVisited;
        [SerializeField] private bool m_SkipChoice;
        [SerializeField] private bool m_ShuffleChoices;
        
        [SerializeField] private TimedChoice m_TimedChoice = new TimedChoice();
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private int m_ChosenId;
        
        [NonSerialized] private float m_CurrentDuration;
        [NonSerialized] private float m_CurrentElapsed;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override bool IsBranch => true;

        public float CurrentDuration => this.m_CurrentDuration;
        public float CurrentElapsed => this.m_CurrentElapsed;

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public bool GetHideUnavailable(DialogueSkin skin) => this.m_Options switch
        {
            NodeTypeData.FromSkin => skin != null && skin.ValuesChoices.HideUnavailable,
            NodeTypeData.FromNode => this.m_HideUnavailable,
            _ => throw new ArgumentOutOfRangeException()
        };

        public bool GetHideVisited(DialogueSkin skin) => this.m_Options switch
        {
            NodeTypeData.FromSkin => skin != null && skin.ValuesChoices.HideVisited,
            NodeTypeData.FromNode => this.m_HideVisited,
            _ => throw new ArgumentOutOfRangeException()
        };

        public bool GetShuffleChoices(DialogueSkin skin) => this.m_Options switch
        {
            NodeTypeData.FromSkin => skin != null && skin.ValuesChoices.ShuffleChoices,
            NodeTypeData.FromNode => this.m_ShuffleChoices,
            _ => throw new ArgumentOutOfRangeException()
        };

        public bool GetTimedChoice(DialogueSkin skin) => this.m_Options switch
        {
            NodeTypeData.FromSkin => skin != null && skin.ValuesChoices.TimedChoice.IsTimed,
            NodeTypeData.FromNode => this.m_TimedChoice.IsTimed,
            _ => throw new ArgumentOutOfRangeException()
        };
        
        public TimeoutBehavior GetTimeout(DialogueSkin skin) => this.m_Options switch
        {
            NodeTypeData.FromSkin => skin != null ? skin.ValuesChoices.TimedChoice.Timeout : 0f,
            NodeTypeData.FromNode => this.m_TimedChoice.Timeout,
            _ => throw new ArgumentOutOfRangeException()
        };
        
        public float GetDuration(DialogueSkin skin, Args args) => this.m_Options switch
        {
            NodeTypeData.FromSkin => skin != null ? skin.ValuesChoices.TimedChoice.GetDuration(args) : 0f,
            NodeTypeData.FromNode => this.m_TimedChoice.GetDuration(args),
            _ => throw new ArgumentOutOfRangeException()
        };

        public List<int> GetChoices(Story story, int nodeId, Args args, bool removeUnavailable)
        {
            List<int> children = story.Content.Children(nodeId);
            DialogueSkin skin = story.Content.DialogueSkin;
            
            bool skipUnavailable = this.GetHideUnavailable(skin) || removeUnavailable;

            for (int i = children.Count - 1; i >= 0; --i)
            {
                int childId = children[i];
                Node child = story.Content.Get(childId);

                if (this.GetHideVisited(skin) && story.Visits.Nodes.Contains(childId))
                {
                    children.RemoveAt(i);
                }
                else if (skipUnavailable && !child.CanRun(args))
                {
                    children.RemoveAt(i);
                }
            }
            
            if (this.GetShuffleChoices(skin)) children.Shuffle();
            return children;
        }

        public void Choose(int nodeId)
        {
            if (this.m_ChosenId != Content.NODE_INVALID) return;
            this.m_ChosenId = nodeId;
        }

        // OVERRIDE METHODS: ----------------------------------------------------------------------

        public override async Task Run(int id, Story story, Args args)
        {
            DialogueSkin skin = story.Content.DialogueSkin;
            
            this.m_ChosenId = Content.NODE_INVALID;
            this.m_CurrentDuration = this.GetDuration(skin, args);
            this.m_CurrentElapsed = 0f;
            
            this.InvokeEventStartChoice(id);
            
            List<int> choices = GetChoices(story, id, args, true);
            if (choices.Count == 1 && story.Content.DialogueSkin.ValuesChoices.AutoOneChoice)
            {
                this.Choose(choices[0]);
            }

            float startTime = story.Time.Time;

            while (PendingChoice(story, args) && !story.IsCanceled)
            {
                await Task.Yield();
                if (!this.GetTimedChoice(skin) || story.Time.Time < startTime + this.m_CurrentDuration)
                {
                    this.m_CurrentElapsed = story.Time.Time - startTime;
                    continue;
                }

                this.m_CurrentElapsed = this.m_CurrentDuration;
                
                choices = GetChoices(story, id, args, true);
                if (choices.Count == 0) Debug.LogError("There cannot be zero choices");
                
                this.Choose(choices[this.GetTimeout(skin) switch
                {
                    TimeoutBehavior.ChooseRandom => UnityEngine.Random.Range(0, choices.Count),
                    TimeoutBehavior.ChooseFirst => 0,
                    TimeoutBehavior.ChooseLast => choices.Count - 1,
                    _ => throw new ArgumentOutOfRangeException()
                }]);
            }
            
            this.InvokeEventFinishChoice(id);
        }

        public override List<int> GetNext(int id, Story story, Args args)
        {
            if (ApplicationManager.IsExiting) return new List<int>();
            if (story.IsCanceled) return new List<int>();
            
            story.Visits.Nodes.Add(this.m_ChosenId);
            story.Visits.Tags.Add(story.Content.Get(this.m_ChosenId).Tag);

            DialogueSkin skin = story.Content.DialogueSkin;
            
            bool skipChoice = this.m_Options switch
            {
                NodeTypeData.FromSkin => skin != null && skin.ValuesChoices.SkipChoice,
                NodeTypeData.FromNode => this.m_SkipChoice,
                _ => throw new ArgumentOutOfRangeException()
            };
            
            return skipChoice 
                ? story.Content.Children(this.m_ChosenId)
                : new List<int> { this.m_ChosenId };
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private bool PendingChoice(Story story, Args args)
        {
            if (this.m_ChosenId == Content.NODE_INVALID) return true;
            
            Node choice = story.Content.Get(this.m_ChosenId);
            if (choice.CanRun(args)) return false;
            
            this.m_ChosenId = Content.NODE_INVALID;
            return true;
        }
    }
}