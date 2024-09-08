using System;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [Serializable]
    public class PickTask
    {
        [SerializeField] private Quest m_Quest;
        [SerializeField] private int m_TaskId;

        // PROPERTIES: ----------------------------------------------------------------------------

        public Quest Quest => this.m_Quest;

        public int TaskId => this.IsValid ? this.m_TaskId : TasksTree.NODE_INVALID;
        public bool IsValid => this.m_Quest != null && this.m_Quest.Contains(this.m_TaskId);

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public PickTask()
        { }

        public PickTask(Quest quest, int taskId)
        {
            this.m_Quest = quest;
            this.m_TaskId = taskId;
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public bool Is(Quest quest, int taskId)
        {
            return
                this.m_Quest != null &&
                this.m_Quest.Id.Hash == quest.Id.Hash &&
                this.m_TaskId == taskId;
        }

        public bool IsNot(Quest quest, int taskId)
        {
            return !this.Is(quest, taskId);
        }
        
        // STRING: --------------------------------------------------------------------------------

        public override string ToString() => this.m_Quest != null && this.m_Quest.Contains(this.TaskId)
             ? this.m_Quest.GetTask(this.m_TaskId).ToString()
             : "(unknown)";
    }
}