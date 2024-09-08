using System;
using UnityEngine;

namespace GameCreator.Runtime.Quests.UnityUI
{
    [Serializable]
    public class ActiveTaskUI : TActiveUI
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private GameObject m_ActiveIfIsCounter;
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Refresh(Journal journal, Quest quest, int taskId)
        {
            State state = journal.GetTaskState(quest, taskId);
            this.Refresh(state);

            Task task = quest.GetTask(taskId);
            if (this.m_ActiveIfIsCounter != null)
            {
                bool isCounter = task.UseCounter != ProgressType.None;
                bool isInactive = state == State.Inactive;
                
                this.m_ActiveIfIsCounter.SetActive(isCounter && !isInactive);
            }
        }
    }
}