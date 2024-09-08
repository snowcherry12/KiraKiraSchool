using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Dialogue
{
    [Serializable]
    public class TokenDialogue : Token
    {
        [Serializable]
        private class VisitsNodes : TSerializableHashSet<int>
        { }
        
        [Serializable]
        private class VisitsTags : TSerializableHashSet<IdString>
        { }
        
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private bool m_IsVisited;
        [SerializeField] private VisitsNodes m_VisitsNodes;
        [SerializeField] private VisitsTags m_VisitsTags;

        // CONSTRUCTORS: --------------------------------------------------------------------------
        
        public TokenDialogue(Dialogue dialogue) : base()
        {
            this.m_IsVisited = dialogue.Story.Visits.IsVisited;
            
            this.m_VisitsNodes = new VisitsNodes();
            this.m_VisitsTags = new VisitsTags();

            foreach (int id in dialogue.Story.Visits.Nodes)
            {
                this.m_VisitsNodes.Add(id);
            }
            
            foreach (IdString tag in dialogue.Story.Visits.Tags)
            {
                this.m_VisitsTags.Add(tag);
            }
        }
        
        // PUBLIC STATIC METHODS: -----------------------------------------------------------------

        public static void OnRemember(Dialogue dialogue, Token token)
        {
            if (token is not TokenDialogue tokenDialogue) return;
            
            dialogue.Story.Visits.Nodes.Clear();
            dialogue.Story.Visits.Tags.Clear();
            
            dialogue.Story.Visits.IsVisited = tokenDialogue.m_IsVisited;
                
            foreach (int id in tokenDialogue.m_VisitsNodes)
            {
                dialogue.Story.Visits.Nodes.Add(id);
            }
            
            foreach (IdString id in tokenDialogue.m_VisitsTags)
            {
                dialogue.Story.Visits.Tags.Add(id);
            }
        }
    }
}