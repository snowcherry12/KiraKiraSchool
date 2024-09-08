using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [Title("Quest Name")]
    [Category("Quests/Quest Name")]
    
    [Image(typeof(IconQuestSolid), ColorTheme.Type.Yellow)]
    [Description("The name of a particular Quest")]

    [Serializable] [HideLabelsInEditor]
    public class GetStringQuestName : PropertyTypeGetString
    {
        [SerializeField] protected PropertyGetQuest m_Quest = GetQuestInstance.Create();

        public override string Get(Args args)
        {
            Quest quest = this.m_Quest.Get(args);
            return quest != null ? quest.GetTitle(args) : string.Empty;
        }

        public static PropertyGetString Create => new PropertyGetString(
            new GetStringQuestName()
        );

        public override string String => $"{this.m_Quest} Name";
    }
}