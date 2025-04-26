using System;
using UnityEngine;

namespace GameCreator.Runtime.Characters
{
    public struct DriverAdditionalTranslation
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private Vector3 m_Value;
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        [field: NonSerialized] public bool HasValue { get; private set; }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Add(Vector3 amount)
        {
            this.HasValue = true;
            this.m_Value += amount;
        }

        public Vector3 Consume()
        {
            Vector3 value = this.m_Value;
            
            this.m_Value = Vector3.zero;
            this.HasValue = false;

            return value;
        }
    }
}