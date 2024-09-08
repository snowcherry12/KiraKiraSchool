using System;
using GameCreator.Runtime.Variables;
using UnityEngine;

namespace GameCreator.Runtime.Quests.UnityUI
{
    [Serializable]
    public class FilterQuests
    {
        private enum Filter
        {
            None = 0,
            InLocalList = 1,
            InGlobalList = 2
        }
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private StateFlags m_Show = StateFlags.Active;
        [SerializeField] private bool m_ShowHidden;
        [SerializeField] private bool m_HideUntracked;
        
        [SerializeField] private Filter m_Filter = Filter.None;
        [SerializeField] private LocalListVariables m_LocalList;
        [SerializeField] private GlobalListVariables m_GlobalList;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public StateFlags Show
        {
            get => this.m_Show;
            set => this.m_Show = value;
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public bool Check(Quest quest, QuestEntry questEntry)
        {
            if (quest == null) return false;
            
            int bitMask = 1 << (int) questEntry.State;
            if (((int) this.m_Show & bitMask) == 0) return false;
            
            if (this.m_HideUntracked && !questEntry.IsTracking) return false;
            if (quest.Type != QuestType.Normal && !this.m_ShowHidden) return false;

            return this.m_Filter switch
            {
                Filter.None => true,
                Filter.InLocalList => this.m_LocalList != null &&
                                      this.m_LocalList.Contains(quest),
                Filter.InGlobalList => this.m_GlobalList != null && 
                                       this.m_GlobalList.Contains(quest),
                _ => false
            };
        }
    }
}