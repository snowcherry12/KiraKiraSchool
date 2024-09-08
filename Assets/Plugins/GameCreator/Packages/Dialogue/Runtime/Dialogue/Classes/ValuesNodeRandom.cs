using System;
using UnityEngine;

namespace GameCreator.Runtime.Dialogue
{
    [Serializable]
    public class ValuesNodeRandom
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private bool m_AllowRepeat = true;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public bool AllowRepeat => this.m_AllowRepeat;
    }
}