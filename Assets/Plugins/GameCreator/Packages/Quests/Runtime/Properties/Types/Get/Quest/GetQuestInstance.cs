using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [Title("Quest")]
    [Category("Quest")]
    
    [Image(typeof(IconQuestSolid), ColorTheme.Type.Green)]
    [Description("A reference to an Quest asset")]

    [Serializable] [HideLabelsInEditor]
    public class GetQuestInstance : PropertyTypeGetQuest
    {
        [SerializeField] protected Quest m_Quest;

        public override Quest Get(Args args) => this.m_Quest;
        public override Quest Get(GameObject gameObject) => this.m_Quest;

        public static PropertyGetQuest Create(Quest Quest = null)
        {
            GetQuestInstance instance = new GetQuestInstance
            {
                m_Quest = Quest
            };
            
            return new PropertyGetQuest(instance);
        }

        public override string String => this.m_Quest != null
            ? this.m_Quest.name
            : "(none)";
    }
}