using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [Title("Task Complete")]
    [Description("Completes the state of an active Task on a Journal component")]

    [Category("Quests/Task Complete")]
    
    [Parameter("Journal", "The Journal component that changes the state of the Task")]
    [Parameter("Quest", "The Quest asset reference")]
    [Parameter("Task", "The Task identifier from the Quest")]
    [Parameter("Wait to Complete", "Whether to wait until the Task finishes running its Instructions")]

    [Image(typeof(IconTaskOutline), ColorTheme.Type.Green)]
    
    [Keywords("Mission", "Finish", "Finalize")]
    [Serializable]
    public class InstructionQuestsTaskComplete : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private PropertyGetGameObject m_Journal = GetGameObjectPlayer.Create();

        [SerializeField] private PickTask m_Task = new PickTask();
        [SerializeField] private bool m_WaitToComplete;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Complete {this.m_Task}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override async System.Threading.Tasks.Task Run(Args args)
        {
            Journal journal = this.m_Journal.Get<Journal>(args);
            if (journal == null) return;

            Quest quest = this.m_Task.Quest;
            if (quest == null) return;

            int taskId = this.m_Task.TaskId;
            if (taskId == TasksTree.NODE_INVALID) return;

            if (this.m_WaitToComplete) await journal.CompleteTask(quest, taskId);
            else _ = journal.CompleteTask(quest, taskId);
        }
    }
}