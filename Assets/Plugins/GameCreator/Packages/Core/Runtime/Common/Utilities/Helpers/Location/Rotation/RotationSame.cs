using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    public readonly struct RotationSame : IRotation
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [NonSerialized] private readonly Transform m_Transform;
        
        // CONSTRUCTORS: --------------------------------------------------------------------------

        public RotationSame(Transform transform)
        {
            this.m_Transform = transform;
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public bool HasRotation(GameObject source) => source != null && this.m_Transform != null;
        
        public Quaternion GetRotation(GameObject source)
        {
            return this.m_Transform.rotation;
        }
    }
}