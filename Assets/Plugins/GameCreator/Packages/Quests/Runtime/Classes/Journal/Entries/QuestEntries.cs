using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Quests
{
    [Serializable]
    public class QuestEntries : TSerializableDictionary<IdString, QuestEntry>
    {
        // GETTER METHODS: ------------------------------------------------------------------------
        
        public State GetState(IdString questId)
        {
            return this.TryGetValue(questId, out QuestEntry entry) 
                ? entry.State
                : State.Inactive;
        }
        
        public bool IsActive(IdString questId)
        {
            return this.TryGetValue(questId, out QuestEntry entry) && entry.State.IsActive();
        }
        
        public bool IsInactive(IdString questId)
        {
            return !this.TryGetValue(questId, out QuestEntry entry) || entry.State.IsInactive();
        }
        
        public bool IsCompleted(IdString questId)
        {
            return this.TryGetValue(questId, out QuestEntry entry) && entry.State.IsCompleted();
        }
        
        public bool IsAbandoned(IdString questId)
        {
            return this.TryGetValue(questId, out QuestEntry entry) && entry.State.IsAbandoned();
        }
        
        public bool IsFailed(IdString questId)
        {
            return this.TryGetValue(questId, out QuestEntry entry) && entry.State.IsFailed();
        }

        public bool IsTracking(IdString questId)
        {
            return this.TryGetValue(questId, out QuestEntry entry) && entry.IsTracking;
        }
    }
}