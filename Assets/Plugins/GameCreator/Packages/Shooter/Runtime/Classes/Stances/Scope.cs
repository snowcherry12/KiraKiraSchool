using System;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    public readonly struct Scope
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        [field: NonSerialized] public bool HasScope { get; }
        
        [field: NonSerialized] public Transform Transform { get; }
        
        [field: NonSerialized] public Vector3 LocalPosition { get; }
        [field: NonSerialized] public Quaternion LocalRotation { get; }

        public Vector3 WorldPosition => this.Transform.TransformPoint(this.LocalPosition);
        public Quaternion WorldRotation => this.Transform.rotation * this.LocalRotation;
        
        public Vector3 LocalDirection => this.LocalRotation * Vector3.forward;
        public Vector3 WorldDirection => this.WorldRotation * Vector3.forward;
        
        [field: NonSerialized] public float Distance { get; }
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public Scope(Transform transform, Vector3 position, Quaternion rotation, float distance)
        {
            this.HasScope = true;
            this.Transform = transform;
            this.LocalPosition = position;
            this.LocalRotation = rotation;
            this.Distance = distance;
        }
    }
}