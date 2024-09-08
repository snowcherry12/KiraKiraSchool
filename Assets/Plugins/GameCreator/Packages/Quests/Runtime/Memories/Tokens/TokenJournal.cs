using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [Serializable]
    public class TokenJournal : Token
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private QuestEntries m_Quests;
        [SerializeField] private TaskEntries m_Tasks;

        // CONSTRUCTORS: --------------------------------------------------------------------------
        
        public TokenJournal(Journal journal) : base()
        {
            this.m_Quests = new QuestEntries();
            this.m_Tasks = new TaskEntries();
            
            foreach (KeyValuePair<IdString, QuestEntry> entry in journal.QuestEntries)
            {
                this.m_Quests.Add(entry.Key, entry.Value);
            }
            
            foreach (KeyValuePair<TaskKey, TaskEntry> entry in journal.TaskEntries)
            {
                this.m_Tasks.Add(entry.Key, entry.Value);
            }
        }
        
        // PUBLIC STATIC METHODS: -----------------------------------------------------------------

        public static void OnRemember(Journal journal, Token token)
        {
            if (token is not TokenJournal tokenJournal) return;
            
            journal.QuestEntries.Clear();
            journal.TaskEntries.Clear();

            foreach (KeyValuePair<IdString, QuestEntry> entry in tokenJournal.m_Quests)
            {
                journal.QuestEntries.Add(entry.Key, entry.Value);
            }
            
            foreach (KeyValuePair<TaskKey, TaskEntry> entry in tokenJournal.m_Tasks)
            {
                journal.TaskEntries.Add(entry.Key, entry.Value);
            }

            journal.OnRemember();
        }
    }
}