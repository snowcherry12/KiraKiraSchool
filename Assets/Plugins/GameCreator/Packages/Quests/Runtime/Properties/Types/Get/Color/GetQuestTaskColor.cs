using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [Title("Task Color")]
    [Category("Quests/Task Color")]
    
    [Image(typeof(IconTaskSolid), ColorTheme.Type.Yellow)]
    [Description("A reference to a Color value of a Task")]
    
    [Keywords("Task", "Mission", "Icon")]

    [Serializable]
    public class GetQuestTaskColor : PropertyTypeGetColor
    {
        [SerializeField] protected PickTask m_Task = new PickTask();

        public override Color Get(Args args)
        {
            Quest quest = this.m_Task.Quest;
            int taskId = this.m_Task.TaskId;
            
            if (quest == null) return Color.black;
            
            Task task = quest.GetTask(taskId);
            return task?.GetColor(args) ?? Color.black;
        }

        public static PropertyGetColor Create() => new PropertyGetColor(
            new GetQuestTaskColor()
        );

        public override string String => $"{this.m_Task} Color";
    }
}