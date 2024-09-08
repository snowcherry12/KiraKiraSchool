using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Game Creator/Quests/Journal")]
    [Icon(RuntimePaths.PACKAGES + "Quests/Editor/Gizmos/GizmoJournal.png")]
    
    [Serializable]
    public class Journal : MonoBehaviour
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private TrackMode m_TrackMode = TrackMode.SingleQuest;
        
        [SerializeField] private Quests m_Quests = new Quests();
        [SerializeField] private Tasks m_Tasks = new Tasks();

        // PROPERTIES: ----------------------------------------------------------------------------

        public TrackMode TrackMode => this.m_TrackMode;

        public QuestEntries QuestEntries => this.m_Quests.Entries;
        public TaskEntries TaskEntries => this.m_Tasks.Entries;
        
        [field: NonSerialized] internal Args Args { get; private set; }
        [field: NonSerialized] private Dictionary<int, Trigger> TriggersLut { get; set; }

        // QUEST EVENTS: --------------------------------------------------------------------------
        
        public event Action<Quest> EventQuestChange;

        public event Action<Quest> EventQuestActivate;
        public event Action<Quest> EventQuestDeactivate;
        public event Action<Quest> EventQuestComplete;
        public event Action<Quest> EventQuestAbandon;
        public event Action<Quest> EventQuestFail;
        
        public event Action<Quest> EventQuestTrack;
        public event Action<Quest> EventQuestUntrack;
        
        // TASK EVENTS: ---------------------------------------------------------------------------
        
        public event Action<Quest> EventTaskChange;
        
        public event Action<Quest, int> EventTaskActivate;
        public event Action<Quest, int> EventTaskDeactivate;
        public event Action<Quest, int> EventTaskComplete;
        public event Action<Quest, int> EventTaskAbandon;
        public event Action<Quest, int> EventTaskFail;
        
        public event Action<Quest, int> EventTaskValueChange;
        
        // INITIALIZE METHODS: --------------------------------------------------------------------

        private void Awake()
        {
            this.Args = new Args(this.gameObject);
            this.TriggersLut = new Dictionary<int, Trigger>();
            
            this.m_Quests.EventTrack += quest => this.EventQuestTrack?.Invoke(quest);
            this.m_Quests.EventUntrack += quest => this.EventQuestUntrack?.Invoke(quest);
            
            this.m_Tasks.EventActivate += (quest, taskId) => this.EventTaskActivate?.Invoke(quest, taskId);
            this.m_Tasks.EventDeactivate += (quest, taskId) => this.EventTaskDeactivate?.Invoke(quest, taskId);
            
            this.m_Tasks.EventComplete += (quest, taskId) => this.EventTaskComplete?.Invoke(quest, taskId);
            this.m_Tasks.EventAbandon += (quest, taskId) => this.EventTaskAbandon?.Invoke(quest, taskId);
            this.m_Tasks.EventFail += (quest, taskId) => this.EventTaskFail?.Invoke(quest, taskId);
            
            this.m_Tasks.EventValueChange += (quest, taskId) => this.EventTaskValueChange?.Invoke(quest, taskId);
            
            this.m_Quests.Setup(this);
            this.m_Tasks.Setup(this);
        }

        internal void OnRemember()
        {
            QuestsList quests = Settings.From<QuestsRepository>().Quests;
            foreach (KeyValuePair<TaskKey, TaskEntry> entry in this.m_Tasks.Entries)
            {
                Quest quest = quests.GetFromTaskId(entry.Key.TaskId);
                if (quest == null || !this.IsTaskActive(quest, entry.Key.TaskId)) continue;
                
                this.OnEnableTask(quest, entry.Key.TaskId);
            }
        }
        
        internal void OnEnableTask(Quest quest, int taskId)
        {
            Task task = quest.GetTask(taskId);
            if (task is not { UseCounter: ProgressType.Property }) return;

            double value = task.ValueFrom.Get(this.gameObject);
            this.m_Tasks.SetValue(quest, taskId, value);

            InstructionList instructions = new InstructionList(
                new InstructionQuestTaskValue(this, quest, taskId, task.ValueFrom)
            );
            
            int triggerId = HashCode.Combine(quest.Id.Hash, taskId);
            this.TriggersLut[triggerId] = task.CreateCheckWhen(instructions);
        }
        
        internal void OnDisableTask(Quest quest, int taskId)
        {
            Task task = quest.GetTask(taskId);
            if (task is not { UseCounter: ProgressType.Property }) return;
            
            int triggerId = HashCode.Combine(quest.Id.Hash, taskId);
            if (this.TriggersLut.TryGetValue(triggerId, out Trigger trigger) && trigger != null)
            {
                Destroy(trigger.gameObject);
            }
        }
        
        // GETTER QUEST METHODS: ------------------------------------------------------------------

        public State GetQuestState(Quest quest)
        {
            return this.m_Quests.GetState(quest);
        }
        
        public bool IsQuestActive(Quest quest)
        {
            return this.m_Quests.IsActive(quest);
        }
        
        public bool IsQuestInactive(Quest quest)
        {
            return this.m_Quests.IsInactive(quest);
        }
        
        public bool IsQuestCompleted(Quest quest)
        {
            return this.m_Quests.IsCompleted(quest);
        }
        
        public bool IsQuestAbandoned(Quest quest)
        {
            return this.m_Quests.IsAbandoned(quest);
        }
        
        public bool IsQuestFailed(Quest quest)
        {
            return this.m_Quests.IsFailed(quest);
        }
        
        public bool IsQuestTracking(Quest quest)
        {
            return this.m_Quests.IsTracking(quest);
        }
        
        // GETTER TASK METHODS: -------------------------------------------------------------------
        
        public State GetTaskState(Quest quest, int taskId)
        {
            return this.m_Tasks.GetState(quest, taskId);
        }
        
        public bool IsTaskActive(Quest quest, int taskId)
        {
            return this.m_Tasks.IsActive(quest, taskId);
        }
        
        public bool IsTaskInactive(Quest quest, int taskId)
        {
            return this.m_Tasks.IsInactive(quest, taskId);
        }
        
        public bool IsTaskCompleted(Quest quest, int taskId)
        {
            return this.m_Tasks.IsCompleted(quest, taskId);
        }
        
        public bool IsTaskAbandoned(Quest quest, int taskId)
        {
            return this.m_Tasks.IsAbandoned(quest, taskId);
        }
        
        public bool IsTaskFailed(Quest quest, int taskId)
        {
            return this.m_Tasks.IsFailed(quest, taskId);
        }

        // SETTER QUEST METHODS: ------------------------------------------------------------------
        
        public async System.Threading.Tasks.Task<bool> ActivateQuest(Quest quest)
        {
            if (!await this.m_Quests.Activate(quest)) return false;
            
            int firstTaskId = quest.Tasks.FirstRootId;
            await this.m_Tasks.Activate(quest, firstTaskId);
            
            this.EventQuestActivate?.Invoke(quest);
            this.EventQuestChange?.Invoke(quest);
            
            return true;
        }

        public async System.Threading.Tasks.Task<bool> DeactivateQuest(Quest quest)
        {
            if (!await this.m_Quests.Deactivate(quest)) return false;
            
            foreach (int taskId in quest.Tasks.RootIds)
            {
                await this.m_Tasks.Deactivate(quest, taskId);
            }
            
            this.EventQuestDeactivate?.Invoke(quest);
            this.EventQuestChange?.Invoke(quest);

            return true;
        }
        
        internal async System.Threading.Tasks.Task<bool> CompleteQuestWithoutNotify(Quest quest)
        {
            return await this.m_Quests.Complete(quest);
        }
        
        internal async System.Threading.Tasks.Task<bool> AbandonQuestWithoutNotify(Quest quest)
        {
            return await this.m_Quests.Abandon(quest);
        }
        
        internal async System.Threading.Tasks.Task<bool> FailQuestWithoutNotify(Quest quest)
        {
            return await this.m_Quests.Fail(quest);
        }

        // SETTER TASK METHODS: -------------------------------------------------------------------
        
        public async System.Threading.Tasks.Task<bool> ActivateTask(Quest quest, int taskId)
        {
            if (!await this.m_Tasks.Activate(quest, taskId)) return false;
            this.EventTaskChange?.Invoke(quest);
            
            if (this.IsQuestCompleted(quest))
            {
                this.EventQuestComplete?.Invoke(quest);
                this.EventQuestChange?.Invoke(quest);
                this.UntrackQuest(quest);
            }
            
            return true;
        }
        
        public async System.Threading.Tasks.Task<bool> DeactivateTask(Quest quest, int taskId)
        {
            if (!await this.m_Tasks.Deactivate(quest, taskId)) return false;
            
            this.EventTaskChange?.Invoke(quest);
            return true;
        }

        public async System.Threading.Tasks.Task<bool> CompleteTask(Quest quest, int taskId)
        {
            if (!await this.m_Tasks.Complete(quest, taskId)) return false;
            this.EventTaskChange?.Invoke(quest);
            
            if (this.IsQuestCompleted(quest))
            {
                this.EventQuestComplete?.Invoke(quest);
                this.EventQuestChange?.Invoke(quest);
                this.UntrackQuest(quest);
            }
            
            return true;
        }
        
        public async System.Threading.Tasks.Task<bool> AbandonTask(Quest quest, int taskId)
        {
            if (!await this.m_Tasks.Abandon(quest, taskId)) return false;
            this.EventTaskChange?.Invoke(quest);
            
            if (this.IsQuestAbandoned(quest))
            {
                this.EventQuestAbandon?.Invoke(quest);
                this.EventQuestChange?.Invoke(quest);
                this.UntrackQuest(quest);
            }
            
            return true;
        }
        
        public async System.Threading.Tasks.Task<bool> FailTask(Quest quest, int taskId)
        {
            if (!await this.m_Tasks.Fail(quest, taskId)) return false;
            this.EventTaskChange?.Invoke(quest);
            
            if (this.IsQuestFailed(quest))
            {
                this.EventQuestFail?.Invoke(quest);
                this.EventQuestChange?.Invoke(quest);
                this.UntrackQuest(quest);
            }
            
            return true;
        }
        
        public double GetTaskValue(Quest quest, int taskId)
        {
            return this.m_Tasks.GetValue(quest, taskId);
        }
        
        public async System.Threading.Tasks.Task<bool> SetTaskValue(Quest quest, int taskId, double value)
        {
            if (!this.m_Tasks.SetValue(quest, taskId, value)) return false;
            
            double taskMax = quest.GetTask(taskId).GetCountTo(this.Args);
            if (value < taskMax) return false;
            
            await this.CompleteTask(quest, taskId);
            return true;
        }
        
        internal async System.Threading.Tasks.Task<bool> ActivateTaskWithoutNotify(Quest quest, int taskId)
        {
            return await this.m_Tasks.Activate(quest, taskId);
        }
        
        internal async System.Threading.Tasks.Task<bool> DeactivateTaskWithoutNotify(Quest quest, int taskId)
        {
            return await this.m_Tasks.Deactivate(quest, taskId);
        }

        internal async System.Threading.Tasks.Task<bool> CompleteTaskWithoutNotify(Quest quest, int taskId)
        {
            return await this.m_Tasks.Complete(quest, taskId);
        }
        
        internal async System.Threading.Tasks.Task<bool> AbandonTaskWithoutNotify(Quest quest, int taskId)
        {
            return await this.m_Tasks.Abandon(quest, taskId);
        }
        
        internal async System.Threading.Tasks.Task<bool> FailTaskWithoutNotify(Quest quest, int taskId)
        {
            return await this.m_Tasks.Fail(quest, taskId);
        }

        // TRACKING METHODS: ----------------------------------------------------------------------

        public bool TrackQuest(Quest quest)
        {
            if (!this.m_Quests.IsActive(quest)) return false;
            if (!this.m_Quests.Track(quest)) return false;
            
            this.EventQuestChange?.Invoke(quest);
            return true;
        }
        
        public bool UntrackQuest(Quest quest)
        {
            if (!this.m_Quests.Untrack(quest)) return false;
            
            this.EventQuestChange?.Invoke(quest);
            return true;
        }

        public void UntrackQuests()
        {
            QuestsList quests = Settings.From<QuestsRepository>().Quests;
            ICollection<IdString> questEntriesKeys = new List<IdString>(this.QuestEntries.Keys);

            foreach (IdString questId in questEntriesKeys)
            {
                if (!this.QuestEntries.TryGetValue(questId, out QuestEntry questEntry)) continue;
                if (!questEntry.IsTracking) continue;
                
                Quest quest = quests.Get(questId);
                if (quest != null) this.UntrackQuest(quest);
            }
        }
    }
}