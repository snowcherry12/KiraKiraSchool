using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    public struct Goal
    {
        [field: NonSerialized] public IdString Name { get; }

        [field: NonSerialized] public float Weight { get; }

        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public Goal(IdString name, float weight)
        {
            this.Name = name;
            this.Weight = Mathf.Max(1f, weight);
        }
    }
}