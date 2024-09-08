using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [Image(typeof(IconJournalOutline), ColorTheme.Type.Green)]
    
    [Title("Journal")]
    [Category("Quests/Journal")]
    
    [Description("Remembers the state of Quests and Tasks from a Journal")]

    [Serializable]
    public class MemoryJournal : Memory
    {
        public override string Title => "Journal";

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public override Token GetToken(GameObject target)
        {
            Journal journal = target.Get<Journal>();
            return journal != null ? new TokenJournal(journal) : null;
        }

        public override void OnRemember(GameObject target, Token token)
        {
            Journal journal = target.Get<Journal>();
            if (journal == null) return;
            
            TokenJournal.OnRemember(journal, token);
        }
    }
}