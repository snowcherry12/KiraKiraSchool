using System;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    [Serializable]
    public abstract class TOutputPort : TPort
    {
        [SerializeField] private Connection[] m_Connections = Array.Empty<Connection>();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public sealed override PortMode Mode => PortMode.Output;

        public Connection[] Connections => this.m_Connections;
    }
}