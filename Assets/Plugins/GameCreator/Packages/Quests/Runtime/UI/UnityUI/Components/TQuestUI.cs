using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Common.UnityUI;
using UnityEngine;
using UnityEngine.UI;

namespace GameCreator.Runtime.Quests.UnityUI
{
    [DisallowMultipleComponent]
    
    [DefaultExecutionOrder(ApplicationManager.EXECUTION_ORDER_LAST_LATER)]
    
    [Serializable]
    public abstract class TQuestUI : MonoBehaviour
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private TextReference m_Title = new TextReference();
        [SerializeField] private TextReference m_Description = new TextReference();
        [SerializeField] private Graphic m_Color;
        [SerializeField] private Image m_Sprite;

        [SerializeField] private FormatQuestUI m_StyleGraphics = new FormatQuestUI();
        [SerializeField] private ActiveQuestUI m_ActiveElements = new ActiveQuestUI();
        [SerializeField] private InteractionQuestUI m_Interactions = new InteractionQuestUI();

        [SerializeField] private StateFlags m_Show = StateFlags.Active;
        [SerializeField] private bool m_ShowHidden;

        [SerializeField] private RectTransform m_TasksContent;
        [SerializeField] private GameObject m_TaskPrefab;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        [field: NonSerialized] public Journal Journal { get; private set; }
        [field: NonSerialized] public Quest Quest { get; private set; }
        
        // INITIALIZERS: --------------------------------------------------------------------------

        private void OnEnable()
        {
            this.m_ActiveElements.OnEnable();
        }
        
        private void OnDisable()
        {
            this.m_ActiveElements.OnDisable();
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void ToggleTracking()
        {
            QuestUI.ToggleTracking(this.Journal, this.Quest);
        }

        public void Select()
        {
            QuestUI.SelectQuestUI(this.Journal, this.Quest);
        }
        
        public void Deselect()
        {
            if (QuestUI.UI_LastQuestSelected != this.Quest) return;
            QuestUI.DeselectQuestUI();
        }
        
        // PROTECTED METHODS: ---------------------------------------------------------------------
        
        protected virtual void Refresh(Journal journal, Quest quest)
        {
            if (journal == null) return;
            if (quest == null) return;

            if (this.Journal != null)
            {
                this.Journal.EventTaskChange -= this.OnTaskChange;
            }
            
            this.Journal = journal;
            this.Quest = quest;

            Args args = new Args(this.gameObject, journal.gameObject);
            this.m_Interactions.Setup(this);

            this.m_Title.Text = quest.GetTitle(args);
            this.m_Description.Text = quest.GetDescription(args);
            
            if (this.m_Color != null) this.m_Color.color = quest.GetColor(args);
            if (this.m_Sprite != null) this.m_Sprite.overrideSprite = quest.GetSprite(args);

            this.Journal.EventTaskChange -= this.OnTaskChange;
            this.Journal.EventTaskChange += this.OnTaskChange;

            this.RefreshTasks();
        }

        // CALLBACK METHODS: ----------------------------------------------------------------------

        private void OnDestroy()
        {
            if (this.Journal == null) return;
            this.Journal.EventTaskChange -= this.OnTaskChange;
        }
        
        private void OnTaskChange(Quest quest)
        {
            this.RefreshTasks();
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void RefreshTasks()
        {
            this.m_StyleGraphics.Refresh(this.Journal, this.Quest);
            this.m_ActiveElements.Refresh(this.Journal, this.Quest);
            
            if (this.m_TasksContent == null) return;
            if (this.m_TaskPrefab == null) return;
            
            int[] tasks = this.CollectTaskIds();
            this.m_TasksContent.gameObject.SetActive(tasks.Length != 0);
            RectTransformUtils.RebuildChildren(
                this.m_TasksContent, 
                this.m_TaskPrefab,
                tasks.Length
            );

            for (int i = 0; i < tasks.Length; ++i)
            {
                Transform child = this.m_TasksContent.GetChild(i);
                
                TaskUI taskUI = child.Get<TaskUI>();
                if (taskUI != null) taskUI.RefreshUI(this.Journal, this.Quest, tasks[i]);
            }
        }

        private int[] CollectTaskIds()
        {
            if (this.Journal == null || this.Quest == null)
            {
                return Array.Empty<int>();
            }

            List<int> tasksResult = new List<int>();
            int[] taskIds = this.Quest.Tasks.RootIds;

            foreach (int taskId in taskIds)
            {
                State taskState = this.Journal.GetTaskState(this.Quest, taskId);
                
                int bitMask = 1 << (int) taskState;
                if (((int) this.m_Show & bitMask) == 0) continue;
                
                if (this.Quest.GetTask(taskId).IsHidden && !this.m_ShowHidden) continue;
                tasksResult.Add(taskId);   
            }

            return tasksResult.ToArray();
        }
    }
}