using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [Title("Is Task Abandoned")]
    [Description("Returns true if a Task from a Journal is abandoned")]

    [Category("Quests/Is Task Abandoned")]

    [Keywords("Journal", "Mission")]
    
    [Image(typeof(IconTaskSolid), ColorTheme.Type.Yellow)]
    [Serializable]
    public class ConditionQuestsIsTaskAbandoned : Condition
    {
        [SerializeField] private PropertyGetGameObject m_Journal = GetGameObjectPlayer.Create();
        [SerializeField] private PickTask m_Task = new PickTask();
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"is {this.m_Task} Abandoned";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Journal journal = this.m_Journal.Get<Journal>(args);
            if (journal == null) return false;

            if (!this.m_Task.IsValid) return false;
            Quest quest = this.m_Task.Quest;
            int taskId = this.m_Task.TaskId;
            
            return quest != null && journal.IsTaskAbandoned(quest, taskId);
        }
    }
}
