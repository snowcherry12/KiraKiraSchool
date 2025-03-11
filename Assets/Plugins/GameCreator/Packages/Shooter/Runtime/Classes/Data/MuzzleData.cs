using System;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    public readonly struct MuzzleData
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        [field: NonSerialized] public Vector3 Position { get; }
        [field: NonSerialized] public Vector3 Direction { get; }
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public MuzzleData(Vector3 position, Vector3 direction)
        {
            this.Position = position;
            this.Direction = direction;
        }
    }
}