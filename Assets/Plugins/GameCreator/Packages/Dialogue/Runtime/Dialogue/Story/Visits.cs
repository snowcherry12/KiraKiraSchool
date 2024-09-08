using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Dialogue
{
    [Serializable]
    public class Visits
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private VisitsNodes m_Nodes = new VisitsNodes();
        [SerializeField] private VisitsTags m_Tags = new VisitsTags();
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private bool m_IsVisited;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        public VisitsNodes Nodes => this.m_Nodes;
        
        public VisitsTags Tags => this.m_Tags;
        
        public bool IsVisited
        {
            get => this.m_IsVisited;
            set => this.m_IsVisited = value;
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Clear()
        {
            this.m_Nodes.Clear();
            this.m_Tags.Clear();
        }

        public bool RemoveNode(int nodeId)
        {
            return this.m_Nodes.Remove(nodeId);
        }

        public bool RemoveTag(IdString tag)
        {
            return this.m_Tags.Remove(tag);
        }
    }
}