using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [Title("Quest Color")]
    [Category("Quests/Quest Color")]
    
    [Image(typeof(IconQuestSolid), ColorTheme.Type.Yellow)]
    [Description("A reference to a Color value of a Quest")]
    
    [Keywords("Task", "Mission", "Icon")]

    [Serializable]
    public class GetQuestQuestColor : PropertyTypeGetColor
    {
        [SerializeField] protected PropertyGetQuest m_Quest = GetQuestInstance.Create();

        public override Color Get(Args args)
        {
            Quest quest = this.m_Quest.Get(args);
            return quest != null ? quest.GetColor(args) : Color.black;
        }

        public static PropertyGetColor Create() => new PropertyGetColor(
            new GetQuestQuestColor()
        );

        public override string String => $"{this.m_Quest} Color";
    }
}