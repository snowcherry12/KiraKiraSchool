using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [Title("Task Max Value")]
    [Category("Quests/Task Max Value")]
    
    [Image(typeof(IconTaskOutline), ColorTheme.Type.Yellow, typeof(OverlayDot))]
    [Description("The maximum value of a Task")]
    
    [Parameter("Quest", "The Quest asset")]
    [Parameter("Task", "The Task of the Quest")]
    
    [Keywords("Quests", "Journal")]

    [Serializable]
    public class GetDecimalTaskMaxValue : PropertyTypeGetDecimal
    {
        [SerializeField] protected PickTask m_Task = new PickTask();
        
        public override double Get(Args args)
        {
            Quest quest = this.m_Task.Quest;
            int task = this.m_Task.TaskId;
            
            if (quest == null) return 0;
            return quest.GetTask(task).GetCountTo(args);
        }

        public static PropertyGetDecimal Create => new PropertyGetDecimal(
            new GetDecimalTaskMaxValue()
        );

        public override string String => $"Max Value of {this.m_Task}";
    }
}