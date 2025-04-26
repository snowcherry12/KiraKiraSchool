using System;
using UnityEngine;

namespace GameCreator.Runtime.Characters
{
    public struct MotionFollowData
    {
        public static MotionFollowData None => new MotionFollowData();
        
        [field: NonSerialized] public Transform Transform { get; }
        [field: NonSerialized] public float MinRadius { get; }
        [field: NonSerialized] public float MaxRadius { get; }

        public MotionFollowData(Transform transform, float minRadius, float maxRadius)
        {
            this.Transform = transform;
            this.MinRadius = minRadius;
            this.MaxRadius = maxRadius;
        }
    }
}