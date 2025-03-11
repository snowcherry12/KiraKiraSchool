using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [AddComponentMenu("")]
    public class WindManager : Singleton<WindManager>
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [NonSerialized] private Vector3 m_Wind = Vector3.zero;
        [NonSerialized] private float m_Force;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        /// <summary>
        /// Returns the wind direction with its magnitude
        /// </summary>
        public Vector3 Wind
        {
            get => this.m_Wind * this.m_Force;
            set
            {
                this.m_Wind = value.normalized;
                this.m_Force = value.magnitude;
                this.EventChange?.Invoke();
            }
        }

        /// <summary>
        /// Returns the wind magnitude
        /// </summary>
        public float Magnitude
        {
            get => this.m_Force;
            set => this.m_Force = value;
        }

        /// <summary>
        /// Returns the wind direction normalized
        /// </summary>
        public Vector3 Direction
        {
            get => this.m_Wind;
            set => this.Wind = value.normalized;
        }
        
        // EVENTS: --------------------------------------------------------------------------------

        public event Action EventChange;
    }
}