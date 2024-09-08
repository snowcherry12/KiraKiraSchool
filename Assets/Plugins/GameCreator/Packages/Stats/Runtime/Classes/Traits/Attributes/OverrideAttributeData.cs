using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Serializable]
    public class OverrideAttributeData
    {
        #pragma warning disable 414
        [SerializeField] [HideInInspector] 
        private bool m_IsExpanded = false;
        #pragma warning restore 414
        
        [SerializeField] private EnablerRatio m_ChangeStartPercent = new EnablerRatio(false, 1f);

        // PROPERTIES: ----------------------------------------------------------------------------

        public bool ChangeStartPercent => this.m_ChangeStartPercent.IsEnabled;
        public double StartPercent => this.m_ChangeStartPercent.Value;
    }
}