using System;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    public readonly struct StrikeOutput : IEquatable<StrikeOutput>
    {
        public static readonly StrikeOutput NONE = new StrikeOutput(null, Vector3.zero);
        
        // PROPERTIES: ----------------------------------------------------------------------------

        [field: NonSerialized] public GameObject GameObject { get; }
        [field: NonSerialized] public Vector3 Point { get; }

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public StrikeOutput(GameObject gameObject, Vector3 point)
        {
            this.GameObject = gameObject;
            this.Point = point;
        }

        // EQUATABLE: -----------------------------------------------------------------------------
        
        public bool Equals(StrikeOutput other)
        {
            return this.GameObject == other.GameObject;
        }
    }
}