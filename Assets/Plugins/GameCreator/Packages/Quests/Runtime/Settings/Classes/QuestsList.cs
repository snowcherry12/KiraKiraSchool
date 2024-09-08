using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [Serializable]
    public class QuestsList
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private Quest[] m_Quests = Array.Empty<Quest>();
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        [NonSerialized] private Dictionary<IdString, Quest> m_QuestsLut;
        [NonSerialized] private Dictionary<int, IdString> m_TasksLut;

        // PROPERTIES: ----------------------------------------------------------------------------

        public Quest[] Quests => this.m_Quests;
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public Quest Get(IdString questId)
        {
            this.RequireInitialize();
            return this.m_QuestsLut.TryGetValue(questId, out Quest quest) ? quest : null;
        }

        public Quest GetFromTaskId(int taskId)
        {
            this.RequireInitialize();
            return this.m_TasksLut.TryGetValue(taskId, out IdString questId) 
                ? this.Get(questId) 
                : null;
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void RequireInitialize()
        {
            if (this.m_QuestsLut != null) return;
            
            this.m_QuestsLut = new Dictionary<IdString, Quest>();
            this.m_TasksLut = new Dictionary<int, IdString>();
            
            foreach (Quest quest in this.m_Quests)
            {
                this.m_QuestsLut[quest.Id] = quest;

                foreach (KeyValuePair<int, TreeNode> entry in quest.Tasks.Nodes)
                {
                    this.m_TasksLut[entry.Key] = quest.Id;
                }
            }
        }
        
        // INTERNAL METHODS: ----------------------------------------------------------------------

        internal void Set(Quest[] quests)
        {
            this.m_Quests = quests;
        }
    }
}