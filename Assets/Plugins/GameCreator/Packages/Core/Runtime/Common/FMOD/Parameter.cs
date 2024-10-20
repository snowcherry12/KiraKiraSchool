using System;
using UnityEngine;

namespace GameCreator.Runtime.Common.FMOD
{
    [Serializable]
    public class Parameter
    {
        // MEMBERS: -------------------------------------------------------------------------------
        [SerializeField] private string m_Name;
        [SerializeField] private float m_Value;

        // PROPERTIES: ----------------------------------------------------------------------------
        public string Name => this.m_Name;
        
        public float Value => this.m_Value;

    }
}