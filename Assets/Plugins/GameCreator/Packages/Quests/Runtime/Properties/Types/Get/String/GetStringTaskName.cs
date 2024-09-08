using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [Title("Task Name")]
    [Category("Quests/Task Name")]
    
    [Image(typeof(IconTaskOutline), ColorTheme.Type.Yellow)]
    [Description("The name of a particular Task")]

    [Serializable] [HideLabelsInEditor]
    public class GetStringTaskName : PropertyTypeGetString
    {
        [SerializeField] protected PickTask m_PickTask = new PickTask();

        public override string Get(Args args)
        {
            return this.m_PickTask.IsValid 
                ? this.m_PickTask.Quest.GetTask(this.m_PickTask.TaskId).GetName(args) 
                : string.Empty;
        }

        public static PropertyGetString Create => new PropertyGetString(
            new GetStringTaskName()
        );

        public override string String => $"{this.m_PickTask} Name";
    }
}