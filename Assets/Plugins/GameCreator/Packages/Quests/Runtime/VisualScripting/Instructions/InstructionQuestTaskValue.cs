using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [Title("Task Value")]
    [Description("Sets, Adds or Subtracts a value from a Task")]

    [Category("Quests/Task Value")]
    
    [Parameter("Journal", "The Journal component that changes the state of the Task")]
    [Parameter("Quest", "The Quest asset reference")]
    [Parameter("Task", "The Task identifier from the Quest")]
    
    [Image(typeof(IconTaskOutline), ColorTheme.Type.Green, typeof(OverlayPlus))]
    
    [Keywords("Mission", "Increment", "Change", "Add", "Set", "Progress")]
    [Serializable]
    public class InstructionQuestTaskValue : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private PropertyGetGameObject m_Journal = GetGameObjectPlayer.Create();

        [SerializeField] private PickTask m_Task = new PickTask();
        [SerializeField] private ChangeDecimal m_Value = new ChangeDecimal(1);
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"{this.m_Task} {this.m_Value}";
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public InstructionQuestTaskValue()
        {
            
        }

        public InstructionQuestTaskValue(Journal journal, Quest quest, int taskId, PropertyGetDecimal value)
        {
            this.m_Journal = GetGameObjectInstance.Create(journal.gameObject);
            this.m_Task = new PickTask(quest, taskId);
            this.m_Value = new ChangeDecimal(value);
        }

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override async System.Threading.Tasks.Task Run(Args args)
        {
            Journal journal = this.m_Journal.Get<Journal>(args);
            if (journal == null) return;

            Quest quest = this.m_Task.Quest;
            if (quest == null) return;

            int taskId = this.m_Task.TaskId;
            if (taskId == TasksTree.NODE_INVALID) return;

            double value = this.m_Value.Get(journal.GetTaskValue(quest, taskId), args);
            await journal.SetTaskValue(quest, taskId, value);
        }
    }
}