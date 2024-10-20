using System;
using UnityEngine;
using FMODUnity;

namespace GameCreator.Runtime.Common
{
    [Serializable]
    public class FMODAudio
    {
        // MEMBERS: -------------------------------------------------------------------------------
        [SerializeField] private EventReference m_Audio;
        [SerializeField] private FMOD.Parameter[] m_Params;

        // PROPERTIES: ----------------------------------------------------------------------------
        public EventReference Audio => this.m_Audio;
        
        public FMOD.Parameter[] Params => this.m_Params;
    }
}