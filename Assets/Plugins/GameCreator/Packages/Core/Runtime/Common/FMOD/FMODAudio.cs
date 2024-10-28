using System;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

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
        public int Length
        {
            get
            {
                EventDescription eventDescription = RuntimeManager.GetEventDescription(m_Audio.Path);
                if (eventDescription.isValid())
                {
                    int length;
                    eventDescription.getLength(out length);
                    eventDescription.releaseAllInstances();
                    return length;
                }
                else
                {
                    eventDescription.releaseAllInstances();
                    return 0;
                }
            }
        }
    }
}