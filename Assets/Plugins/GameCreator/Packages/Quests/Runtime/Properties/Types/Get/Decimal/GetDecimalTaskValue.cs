using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [Title("Task Value from Journal")]
    [Category("Quests/Task Value from Journal")]
    
    [Image(typeof(IconTaskOutline), ColorTheme.Type.Yellow)]
    [Description("The value of a Task from the Journal of a game object")]

    [Parameter("Journal", "The game object with the Journal component")]
    [Parameter("Quest", "The Quest asset")]
    [Parameter("Task", "The Task of the Quest")]
    
    [Keywords("Quests", "Journal")]

    [Serializable]
    public class GetDecimalTaskValue : PropertyTypeGetDecimal
    {
        [SerializeField] protected PropertyGetGameObject m_Journal = GetGameObjectPlayer.Create();
        [SerializeField] protected PickTask m_Task = new PickTask();
        
        public override double Get(Args args)
        {
            Journal journal = this.m_Journal.Get<Journal>(args);
            if (journal == null) return 0;

            Quest quest = this.m_Task.Quest;
            int task = this.m_Task.TaskId;
            
            if (quest == null) return 0;
            return journal.GetTaskValue(quest, task);
        }

        public static PropertyGetDecimal Create => new PropertyGetDecimal(
            new GetDecimalTaskValue()
        );

        public override string String => $"Value of {this.m_Task}";
    }
}