using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Dialogue
{
    [Serializable]
    public class Values : TPolymorphicList<Value>
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeReference] private Value[] m_Values = Array.Empty<Value>();
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override int Length => this.m_Values.Length;

        public Value[] Get => this.m_Values;
    }
}