using System;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [Serializable]
    internal class Quests
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private QuestEntries m_Quests = new QuestEntries();
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private Journal m_Journal;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        public QuestEntries Entries
        {
            get => this.m_Quests;
            set => this.m_Quests = value;
        }
        
        // EVENTS: --------------------------------------------------------------------------------

        public event Action<Quest> EventTrack;
        public event Action<Quest> EventUntrack;

        // INITIALIZE METHODS: --------------------------------------------------------------------

        public void Setup(Journal journal)
        {
            this.m_Journal = journal;
        }
        
        // GETTER METHODS: ------------------------------------------------------------------------

        public State GetState(Quest quest)
        {
            return quest != null 
                ? this.m_Quests.GetState(quest.Id) 
                : State.Inactive;
        }
        
        public bool IsActive(Quest quest)
        {
            return quest != null && this.m_Quests.IsActive(quest.Id);
        }
        
        public bool IsInactive(Quest quest)
        {
            return quest != null && this.m_Quests.IsInactive(quest.Id);
        }
        
        public bool IsCompleted(Quest quest)
        {
            return quest != null && this.m_Quests.IsCompleted(quest.Id);
        }
        
        public bool IsAbandoned(Quest quest)
        {
            return quest != null && this.m_Quests.IsAbandoned(quest.Id);
        }
        
        public bool IsFailed(Quest quest)
        {
            return quest != null && this.m_Quests.IsFailed(quest.Id);
        }

        public bool IsTracking(Quest quest)
        {
            return quest != null && this.m_Quests.IsTracking(quest.Id);
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public async System.Threading.Tasks.Task<bool> Activate(Quest quest)
        {
            if (quest == null) return false;
            bool isTracking = false;
            
            if (this.m_Quests.TryGetValue(quest.Id, out QuestEntry entry))
            {
                if (!entry.State.IsInactive()) return false;
                isTracking = entry.IsTracking;
            }
        
            this.m_Quests[quest.Id] = QuestEntry.NewActive(isTracking);
            await quest.RunOnActivate(this.m_Journal.Args);

            Quest.LastQuestActivated = quest;
            return true;
        }
        
        public async System.Threading.Tasks.Task<bool> Deactivate(Quest quest)
        {
            if (quest == null) return false;
            bool isTracking = false;
            
            if (this.m_Quests.TryGetValue(quest.Id, out QuestEntry entry))
            {
                if (entry.State.IsInactive()) return false;
                isTracking = entry.IsTracking;
            }
        
            this.m_Quests[quest.Id] = QuestEntry.NewInactive(isTracking);
            await quest.RunOnDeactivate(this.m_Journal.Args);
            
            Quest.LastQuestDeactivated = quest;
            return true;
        }
        
        public async System.Threading.Tasks.Task<bool> Complete(Quest quest)
        {
            if (quest == null) return false;
            bool isTracking = false;
            
            if (this.m_Quests.TryGetValue(quest.Id, out QuestEntry entry))
            {
                if (entry.State.IsFinished()) return false;
                isTracking = entry.IsTracking;
            }
            
            this.m_Quests[quest.Id] = QuestEntry.NewCompleted(isTracking);
            await quest.RunOnComplete(this.m_Journal.Args);
            
            Quest.LastQuestCompleted = quest;
            return true;
        }
        
        public async System.Threading.Tasks.Task<bool> Abandon(Quest quest)
        {
            if (quest == null) return false;
            bool isTracking = false;
            
            if (this.m_Quests.TryGetValue(quest.Id, out QuestEntry entry))
            {
                if (entry.State.IsFinished()) return false;
                isTracking = entry.IsTracking;
            }
            
            this.m_Quests[quest.Id] = QuestEntry.NewAbandoned(isTracking);
            await quest.RunOnAbandon(this.m_Journal.Args);
            
            Quest.LastQuestAbandoned = quest;
            return true;
        }
        
        public async System.Threading.Tasks.Task<bool> Fail(Quest quest)
        {
            if (quest == null) return false;
            bool isTracking = false;
            
            if (this.m_Quests.TryGetValue(quest.Id, out QuestEntry entry))
            {
                if (entry.State.IsFinished()) return false;
                isTracking = entry.IsTracking;
            }
            
            this.m_Quests[quest.Id] = QuestEntry.NewFailed(isTracking);
            await quest.RunOnFail(this.m_Journal.Args);
            
            Quest.LastQuestFailed = quest;
            return true;
        }
        
        // TRACKING METHODS: ----------------------------------------------------------------------
        
        public bool Track(Quest quest)
        {
            if (quest == null) return false;
            
            bool exists = this.m_Quests.TryGetValue(quest.Id, out QuestEntry entry);
            if (!exists) this.m_Quests.Add(quest.Id, QuestEntry.NewInactive(false));
        
            if (entry.IsTracking) return false;
        
            if (this.m_Journal.TrackMode == TrackMode.SingleQuest)
            {
                this.m_Journal.UntrackQuests();
            }
            
            entry.IsTracking = true;
            this.m_Quests[quest.Id] = entry;
            
            this.EventTrack?.Invoke(quest);
            return true;
        }
        
        public bool Untrack(Quest quest)
        {
            if (quest == null) return false;
        
            if (!this.m_Quests.TryGetValue(quest.Id, out QuestEntry entry)) return false;
            if (!entry.IsTracking) return false;
            
            entry.IsTracking = false;
            this.m_Quests[quest.Id] = entry;
            
            this.EventUntrack?.Invoke(quest);
            return true;
        }
    }
}