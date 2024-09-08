using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Serializable]
    public class OverrideStatData
    {
        #pragma warning disable 414
        [SerializeField] [HideInInspector] 
        private bool m_IsExpanded = false;
        #pragma warning restore 414
        
        [SerializeField] private EnablerDouble m_ChangeBase = new EnablerDouble(false, 1f);
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public bool ChangeBase => this.m_ChangeBase.IsEnabled;
        public double Base => this.m_ChangeBase.Value;
    }
}