using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Serializable]
    public class AttributeList : TPolymorphicList<AttributeItem>
    {
        [SerializeReference] private AttributeItem[] m_Attributes = Array.Empty<AttributeItem>();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override int Length => this.m_Attributes.Length;

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public AttributeItem Get(int index) => this.m_Attributes[index];
    }
}