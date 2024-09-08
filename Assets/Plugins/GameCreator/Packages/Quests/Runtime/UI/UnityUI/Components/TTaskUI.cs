using System;
using System.Collections.Generic;
using System.Globalization;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Common.UnityUI;
using UnityEngine;
using UnityEngine.UI;

namespace GameCreator.Runtime.Quests.UnityUI
{
    [DisallowMultipleComponent]
    
    [DefaultExecutionOrder(ApplicationManager.EXECUTION_ORDER_LAST_LATER)]
    
    [Serializable]
    public abstract class TTaskUI : MonoBehaviour
    {
        private static readonly CultureInfo CULTURE = CultureInfo.InvariantCulture;

        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private TextReference m_Title = new TextReference();
        [SerializeField] private TextReference m_Description = new TextReference();

        [SerializeField] private Graphic m_Color;
        [SerializeField] private Image m_Sprite;

        [SerializeField] private TextReference m_CountTo = new TextReference();
        [SerializeField] private TextReference m_CurrentCount = new TextReference();
        [SerializeField] private Image m_FillCounter;

        [SerializeField] private FormatTaskUI m_StyleGraphics = new FormatTaskUI();
        [SerializeField] private ActiveTaskUI m_ActiveElements = new ActiveTaskUI();
        [SerializeField] private InteractionsTaskUI m_Interactions = new InteractionsTaskUI();

        [SerializeField] private StateFlags m_Show = StateFlags.Active;
        [SerializeField] private bool m_ShowHidden;

        [SerializeField] private RectTransform m_SubtasksContent;
        [SerializeField] private GameObject m_SubtaskPrefab;
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private Args m_Args;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        [field: NonSerialized] public Journal Journal { get; private set; }
        [field: NonSerialized] public Quest Quest { get; private set; }
        [field: NonSerialized] public int TaskId { get; private set; }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Select()
        {
            TaskUI.SelectTaskUI(this.Journal, this.Quest, this.TaskId);
        }
        
        public void Deselect()
        {
            if (TaskUI.UI_LastTaskQuestSelected != this.Quest) return;
            if (TaskUI.UI_LastTaskTaskSelected != this.TaskId) return;
            
            TaskUI.DeselectTaskUI();
        }
        
        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected virtual void Refresh(Journal journal, Quest quest, int taskId)
        {
            if (journal == null) return;
            if (quest == null) return;

            if (this.Journal != null)
            {
                journal.EventTaskValueChange -= this.OnValueChange;
            }
            
            this.Journal = journal;
            this.Quest = quest;
            this.TaskId = taskId;
            
            this.m_Args = new Args(this.gameObject, journal.gameObject);
            this.m_Interactions.Setup(this);

            journal.EventTaskValueChange -= this.OnValueChange;
            journal.EventTaskValueChange += this.OnValueChange;
            
            Task task = quest.GetTask(taskId);
            
            this.m_Title.Text = task.GetName(this.m_Args);
            this.m_Description.Text = task.GetDescription(this.m_Args);

            if (this.m_Color != null) this.m_Color.color = task.GetColor(this.m_Args);
            if (this.m_Sprite != null) this.m_Sprite.overrideSprite = task.GetSprite(this.m_Args);

            this.RefreshCounters();
            this.RefreshTasks();
        }
        
        // CALLBACK METHODS: ----------------------------------------------------------------------

        private void OnValueChange(Quest quest, int taskId)
        {
            if (this.Quest != quest || this.TaskId != taskId) return;
            this.RefreshCounters();
        }

        private void OnDestroy()
        {
            if (this.Journal == null) return;
            this.Journal.EventTaskValueChange -= this.OnValueChange;
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void RefreshTasks()
        {
            this.m_StyleGraphics.Refresh(this.Journal, this.Quest, this.TaskId);
            this.m_ActiveElements.Refresh(this.Journal, this.Quest, this.TaskId);
            
            if (this.m_SubtasksContent == null) return;
            if (this.m_SubtaskPrefab == null) return;
            
            int[] tasks = this.CollectTaskIds();
            RectTransformUtils.RebuildChildren(
                this.m_SubtasksContent, 
                this.m_SubtaskPrefab,
                tasks.Length
            );

            for (int i = 0; i < tasks.Length; ++i)
            {
                Transform child = this.m_SubtasksContent.GetChild(i);
            
                TaskUI taskUI = child.Get<TaskUI>();
                if (taskUI != null) taskUI.RefreshUI(this.Journal, this.Quest, tasks[i]);
            }
        }
        
        private void RefreshCounters()
        {
            Task task = this.Quest.GetTask(this.TaskId);
            
            if (task.UseCounter != ProgressType.None)
            {
                double maximum = task.GetCountTo(this.m_Args);
                double current = this.Journal.GetTaskValue(this.Quest, this.TaskId);
                
                this.m_CountTo.Text = maximum.ToString(CULTURE);
                this.m_CurrentCount.Text = current.ToString(CULTURE);

                float ratio = (float) current / (float) maximum;
                if (this.m_FillCounter != null) this.m_FillCounter.fillAmount = ratio;
            }
        }

        private int[] CollectTaskIds()
        {
            if (this.Journal == null || this.Quest == null)
            {
                return Array.Empty<int>();
            }

            List<int> taskResults = new List<int>();
            List<int> taskIds = this.Quest.Tasks.Children(this.TaskId);

            foreach (int taskId in taskIds)
            {
                State taskState = this.Journal.GetTaskState(this.Quest, taskId);
                
                int bitMask = 1 << (int) taskState;
                if (((int) this.m_Show & bitMask) == 0) continue;
                
                if (this.Quest.GetTask(taskId).IsHidden && !this.m_ShowHidden) continue;

                taskResults.Add(taskId);   
            }

            return taskResults.ToArray();
        }
    }
}