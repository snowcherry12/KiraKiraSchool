using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Quests
{
    [Serializable]
    public class TaskEntries : TSerializableDictionary<TaskKey, TaskEntry>
    {
        // GETTER METHODS: ------------------------------------------------------------------------

        public State GetState(Quest quest, int taskId)
        {
            TaskKey taskKey = new TaskKey(quest, taskId);
            return this.TryGetValue(taskKey, out TaskEntry entry) 
                ? entry.State
                : State.Inactive;
        }
        
        public bool IsActive(Quest quest, int taskId)
        {
            TaskKey taskKey = new TaskKey(quest, taskId);
            return this.TryGetValue(taskKey, out TaskEntry entry) && entry.State.IsActive();
        }
        
        public bool IsInactive(Quest quest, int taskId)
        {
            TaskKey taskKey = new TaskKey(quest, taskId);
            return !this.TryGetValue(taskKey, out TaskEntry entry) || entry.State.IsInactive();
        }
        
        public bool IsCompleted(Quest quest, int taskId)
        {
            TaskKey taskKey = new TaskKey(quest, taskId);
            return this.TryGetValue(taskKey, out TaskEntry entry) && entry.State.IsCompleted();
        }
        
        public bool IsAbandoned(Quest quest, int taskId)
        {
            TaskKey taskKey = new TaskKey(quest, taskId);
            return this.TryGetValue(taskKey, out TaskEntry entry) && entry.State.IsAbandoned();
        }
        
        public bool IsFailed(Quest quest, int taskId)
        {
            TaskKey taskKey = new TaskKey(quest, taskId);
            return this.TryGetValue(taskKey, out TaskEntry entry) && entry.State.IsFailed();
        }
    }
}