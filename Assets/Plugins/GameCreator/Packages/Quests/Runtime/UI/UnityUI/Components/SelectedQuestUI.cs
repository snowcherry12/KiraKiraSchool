using System;
using System.Collections;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Quests.UnityUI
{
    [DisallowMultipleComponent]
    
    [AddComponentMenu("Game Creator/UI/Quests/Selected Quest UI")]
    [Icon(RuntimePaths.PACKAGES + "Quests/Editor/Gizmos/GizmoQuestUI.png")]
    
    [DefaultExecutionOrder(ApplicationManager.EXECUTION_ORDER_LAST_LATER)]
    
    public class SelectedQuestUI : TQuestUI
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private GameObject m_ActiveIfSelected;
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private Journal m_PreviousJournal;
        
        // INITIALIZERS: --------------------------------------------------------------------------

        private IEnumerator Start()
        {
            yield return null;
            this.OnSelectQuest(this.Journal, this.Quest);
        }

        private void OnEnable()
        {
            QuestUI.EventSelect -= this.OnSelectQuest;
            QuestUI.EventSelect += this.OnSelectQuest;
        }

        private void OnDisable()
        {
            QuestUI.EventSelect -= this.OnSelectQuest;
        }
        
        // CALLBACK METHODS: ----------------------------------------------------------------------
        
        private void OnSelectQuest(Journal journal, Quest quest)
        {
            if (this.m_ActiveIfSelected != null)
            {
                bool isSelected = quest != null;
                this.m_ActiveIfSelected.SetActive(isSelected);
            }
            
            if (quest == null) return;

            if (this.m_PreviousJournal != null)
            {
                this.m_PreviousJournal.EventQuestChange -= this.OnChangeQuest;
            }

            this.m_PreviousJournal = journal;
            this.m_PreviousJournal.EventQuestChange -= this.OnChangeQuest;
            this.m_PreviousJournal.EventQuestChange += this.OnChangeQuest;
            
            this.Refresh(journal, quest);
        }

        private void OnChangeQuest(Quest quest)
        {
            this.Refresh(this.m_PreviousJournal, quest);
        }
    }
}