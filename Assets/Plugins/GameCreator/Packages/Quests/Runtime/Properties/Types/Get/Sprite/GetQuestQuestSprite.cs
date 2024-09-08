using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [Title("Quest Sprite")]
    [Category("Quests/Quest Sprite")]
    
    [Image(typeof(IconQuestSolid), ColorTheme.Type.Yellow)]
    [Description("A reference to a Sprite texture of a Quest")]
    
    [Keywords("Task", "Mission", "Icon")]

    [Serializable]
    public class GetQuestQuestSprite : PropertyTypeGetSprite
    {
        [SerializeField] protected PropertyGetQuest m_Quest = GetQuestInstance.Create();

        public override Sprite Get(Args args)
        {
            Quest quest = this.m_Quest.Get(args);
            return quest != null ? quest.GetSprite(args) : null;
        }

        public static PropertyGetSprite Create() => new PropertyGetSprite(
            new GetQuestQuestSprite()
        );

        public override string String => $"{this.m_Quest} Sprite";
    }
}