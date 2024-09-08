using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [Title("Task Description")]
    [Category("Quests/Task Description")]
    
    [Image(typeof(IconTaskOutline), ColorTheme.Type.Yellow)]
    [Description("The description of a particular Task")]

    [Serializable] [HideLabelsInEditor]
    public class GetStringTaskDescription : PropertyTypeGetString
    {
        [SerializeField] protected PickTask m_PickTask = new PickTask();

        public override string Get(Args args)
        {
            return this.m_PickTask.IsValid 
                ? this.m_PickTask.Quest.GetTask(this.m_PickTask.TaskId).GetDescription(args) 
                : string.Empty;
        }

        public static PropertyGetString Create => new PropertyGetString(
            new GetStringTaskDescription()
        );

        public override string String => $"{this.m_PickTask} Description";
    }
}