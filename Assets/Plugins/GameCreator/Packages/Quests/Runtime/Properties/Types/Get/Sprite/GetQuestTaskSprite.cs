using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [Title("Task Sprite")]
    [Category("Quests/Task Sprite")]
    
    [Image(typeof(IconTaskSolid), ColorTheme.Type.Yellow)]
    [Description("A reference to a Sprite texture of a Task")]
    
    [Keywords("Task", "Mission", "Icon")]

    [Serializable]
    public class GetQuestTaskSprite : PropertyTypeGetSprite
    {
        [SerializeField] protected PickTask m_Task = new PickTask();

        public override Sprite Get(Args args)
        {
            Quest quest = this.m_Task.Quest;
            int taskId = this.m_Task.TaskId;
            
            if (quest == null) return null;
            
            Task task = quest.GetTask(taskId);
            return task?.GetSprite(args);
        }

        public static PropertyGetSprite Create() => new PropertyGetSprite(
            new GetQuestTaskSprite()
        );

        public override string String => $"{this.m_Task} Sprite";
    }
}