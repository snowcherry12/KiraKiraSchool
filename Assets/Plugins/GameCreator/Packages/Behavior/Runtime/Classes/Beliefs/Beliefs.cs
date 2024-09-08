using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    [Serializable]
    public class Beliefs : TPolymorphicList<Belief>
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeReference] private Belief[] m_List = Array.Empty<Belief>();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public Belief[] List => this.m_List;
        
        public override int Length => this.m_List.Length;

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public bool Resolves(Belief preBelief)
        {
            foreach (Belief belief in this.m_List)
            {
                if (belief.Name == preBelief.Name && belief.Value == preBelief.Value)
                {
                    return true;
                }
            }

            return false;
        }
        
        public bool Resolves(Goal goal)
        {
            foreach (Belief belief in this.m_List)
            {
                if (belief.Name == goal.Name && belief.Value)
                {
                    return true;
                }
            }

            return false;
        }
    }
}