using System;
using System.Collections;
using System.Collections.Generic;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Quests.UnityUI
{
    [AddComponentMenu("Game Creator/UI/Quests/Quest List UI")]
    [Icon(RuntimePaths.PACKAGES + "Quests/Editor/Gizmos/GizmoQuestListUI.png")]
    
    [DefaultExecutionOrder(ApplicationManager.EXECUTION_ORDER_LAST_LATER)]
    
    [Serializable]
    public class QuestListUI : MonoBehaviour
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Journal = GetGameObjectPlayer.Create();
        [SerializeField] private FilterQuests m_Filter = new FilterQuests();
        
        [SerializeField] private RectTransform m_Content;
        [SerializeField] private GameObject m_Prefab;
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private Journal m_CurrentJournal;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public StateFlags Show
        {
            get => this.m_Filter.Show;
            set
            {
                this.m_Filter.Show = value;
                this.RefreshUI();
            }
        }
        
        // EVENTS: --------------------------------------------------------------------------------

        public event Action EventRefreshUI;
        
        // INITIALIZERS: --------------------------------------------------------------------------

        private IEnumerator Start()
        {
            yield return null;
            this.OnEnable();
        }
        
        private void OnEnable()
        {
            this.m_CurrentJournal = this.m_Journal.Get<Journal>(this.gameObject);
            if (this.m_CurrentJournal == null) return;

            this.m_CurrentJournal.EventQuestChange -= this.OnQuestsChange;
            this.m_CurrentJournal.EventQuestChange += this.OnQuestsChange;
            
            this.RefreshUI();
        }

        private void OnDisable()
        {
            if (this.m_CurrentJournal == null) return;
            this.m_CurrentJournal.EventQuestChange -= this.OnQuestsChange;
        }
        
        // CALLBACKS: -----------------------------------------------------------------------------
        
        private void OnQuestsChange(Quest quest)
        {
            this.RefreshUI();
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void RefreshUI()
        {
            if (this.m_Content == null) return;
            if (this.m_Prefab == null) return;
            
            Quest[] quests = this.CollectQuests();
            Array.Sort(quests, this.SortQuests);
            
            RectTransformUtils.RebuildChildren(this.m_Content, this.m_Prefab, quests.Length);

            for (int i = 0; i < quests.Length; ++i)
            {
                Transform child = this.m_Content.GetChild(i);
            
                QuestUI questUI = child.Get<QuestUI>();
                if (questUI != null) questUI.RefreshUI(this.m_CurrentJournal, quests[i]);
            }
            
            this.EventRefreshUI?.Invoke();
        }

        private Quest[] CollectQuests()
        {
            if (this.m_CurrentJournal == null) return Array.Empty<Quest>();

            List<Quest> quests = new List<Quest>();
            foreach (KeyValuePair<IdString, QuestEntry> entry in this.m_CurrentJournal.QuestEntries)
            {
                Quest quest = Settings.From<QuestsRepository>().Quests.Get(entry.Key);
                if (!this.m_Filter.Check(quest, entry.Value)) continue;
                
                quests.Add(quest);
            }

            return quests.ToArray();
        }
        
        private int SortQuests(Quest a, Quest b)
        {
            return a.SortOrder.CompareTo(b.SortOrder);
        }
    }
}