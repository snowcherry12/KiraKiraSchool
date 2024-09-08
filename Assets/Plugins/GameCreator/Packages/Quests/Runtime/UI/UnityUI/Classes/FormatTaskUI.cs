using System;

namespace GameCreator.Runtime.Quests.UnityUI
{
    [Serializable]
    public class FormatTaskUI : TFormatUI
    {
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Refresh(Journal journal, Quest quest, int taskId)
        {
            State state = journal.GetTaskState(quest, taskId);
            bool isSelected = TaskUI.UI_LastTaskQuestSelected == quest &&
                              TaskUI.UI_LastTaskTaskSelected == taskId;
            
            this.Refresh(state, isSelected);
        }
    }
}