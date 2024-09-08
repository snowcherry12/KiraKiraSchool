using System;
using GameCreator.Runtime.Cameras;
using UnityEngine;

namespace GameCreator.Runtime.Dialogue
{
    [Serializable]
    public class ShotCameraEntry
    {
        [SerializeField] private ShotCamera m_ShotCamera;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public ShotCamera Get => this.m_ShotCamera;
    }
}