using System;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class PropertyOverride
    {
        [SerializeField] private bool m_Override;
        [SerializeField] private float m_Number;
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public bool Override => this.m_Override;
        public float Number => this.m_Number;
    }
}