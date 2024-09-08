using System;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [Serializable]
    public struct TaskKey : IEquatable<TaskKey>
    {
        [SerializeField] private int m_QuestHash;
        [SerializeField] private int m_TaskId;
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public int TaskId => this.m_TaskId;
        
        // CONSTRUCTORS: --------------------------------------------------------------------------

        public TaskKey(Quest quest, int taskId)
        {
            this.m_QuestHash = quest != null ? quest.Id.Hash : 0;
            this.m_TaskId = taskId;
        }
        
        public TaskKey(int questHash, int taskId)
        {
            this.m_QuestHash = questHash;
            this.m_TaskId = taskId;
        }

        // EQUATABLE IMPLEMENTATION: --------------------------------------------------------------
        
        public override bool Equals(object other)
        {
            return other is TaskKey taskKey && Equals(taskKey);
        }
        
        public bool Equals(TaskKey other)
        {
            return this.m_QuestHash == other.m_QuestHash && this.m_TaskId == other.m_TaskId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.m_QuestHash, this.m_TaskId);
        }
    }
}