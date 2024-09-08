using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [Title("Quest Description")]
    [Category("Quests/Quest Description")]
    
    [Image(typeof(IconQuestSolid), ColorTheme.Type.Yellow)]
    [Description("The description of a particular Quest")]

    [Serializable] [HideLabelsInEditor]
    public class GetStringQuestDescription : PropertyTypeGetString
    {
        [SerializeField] protected PropertyGetQuest m_Quest = GetQuestInstance.Create();

        public override string Get(Args args)
        {
            Quest quest = this.m_Quest.Get(args);
            return quest != null ? quest.GetDescription(args) : string.Empty;
        }

        public static PropertyGetString Create => new PropertyGetString(
            new GetStringQuestDescription()
        );

        public override string String => $"{this.m_Quest} Description";
    }
}