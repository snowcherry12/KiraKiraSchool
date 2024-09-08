using System;
using UnityEngine;

namespace GameCreator.Runtime.Dialogue
{
    [Serializable]
    public class Acting
    {
        [SerializeField] private PortraitMode m_Portrait = PortraitMode.ActorDefault;
        
        [SerializeField] private Actor m_Actor;
        [SerializeField] private int m_Expression;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        public PortraitMode Portrait => this.m_Portrait;
        
        public Actor Actor => this.m_Actor;
        
        public int Expression => this.m_Expression;
    }
}