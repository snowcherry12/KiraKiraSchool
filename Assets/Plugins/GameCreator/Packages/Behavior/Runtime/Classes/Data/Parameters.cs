using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    [Serializable]
    public class Parameters : TPolymorphicList<Parameter>
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeReference] private Parameter[] m_List = Array.Empty<Parameter>();

        // PROPERTIES: ----------------------------------------------------------------------------

        public Parameter[] List => this.m_List;
        
        public override int Length => this.m_List.Length;
    }
}