using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [Title("Is Quest Failed")]
    [Description("Returns true if a Quest from a Journal is failed")]

    [Category("Quests/Is Quest Failed")]

    [Keywords("Journal", "Mission")]
    
    [Image(typeof(IconQuestSolid), ColorTheme.Type.Red)]
    [Serializable]
    public class ConditionQuestsIsQuestFailed : Condition
    {
        [SerializeField] private PropertyGetGameObject m_Journal = GetGameObjectPlayer.Create();
        [SerializeField] private PropertyGetQuest m_Quest = GetQuestInstance.Create();
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"is {this.m_Quest} Failed";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Journal journal = this.m_Journal.Get<Journal>(args);
            if (journal == null) return false;

            Quest quest = this.m_Quest.Get(args);
            return quest != null && journal.IsQuestFailed(quest);
        }
    }
}
