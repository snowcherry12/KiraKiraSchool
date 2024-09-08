using System;
using UnityEngine;

namespace GameCreator.Runtime.Quests.UnityUI
{
    [Serializable]
    public class ActiveQuestUI : TActiveUI
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private GameObject m_ActiveIfTracking;
        [SerializeField] private GameObject m_ActiveIfSelected;
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        [NonSerialized] private Journal m_Journal;
        [NonSerialized] private Quest m_Quest;

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void OnEnable()
        {
            QuestUI.EventSelect -= this.OnSelect;
            QuestUI.EventSelect += this.OnSelect;

            this.OnSelect(null, QuestUI.UI_LastQuestSelected);
        }
        
        public void OnDisable()
        {
            QuestUI.EventSelect -= this.OnSelect;
        }
        
        public void Refresh(Journal journal, Quest quest)
        {
            this.m_Quest = quest;

            State state = journal.GetQuestState(quest);
            this.Refresh(state);
            
            if (this.m_ActiveIfTracking != null)
            {
                bool isTracking = journal.IsQuestTracking(quest);
                this.m_ActiveIfTracking.SetActive(isTracking);
            }

            this.OnSelect(null, QuestUI.UI_LastQuestSelected);
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void OnSelect(Journal journal, Quest quest)
        {
            if (this.m_ActiveIfSelected == null) return;
            
            this.m_ActiveIfSelected.SetActive(
                quest != null && 
                this.m_Quest != null &&
                this.m_Quest.Id.Hash == quest.Id.Hash
            );
        }
    }
}