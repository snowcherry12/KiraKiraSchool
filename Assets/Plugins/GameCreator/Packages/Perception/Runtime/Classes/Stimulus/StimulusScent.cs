using System;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    public struct StimulusScent : IStimulus
    {
        // PROPERTIES: ----------------------------------------------------------------------------

        [field: NonSerialized] public string Tag { get; }
        
        [field: NonSerialized] public Vector3 Position { get; }
        [field: NonSerialized] public float Radius { get; }
        
        [field: NonSerialized] public float Intensity { get; }

        // CONSTRUCTORS: --------------------------------------------------------------------------
        
        public StimulusScent(string tag, Vector3 position, float radius, float intensity)
        {
            this.Tag = tag;
            this.Position = position;
            this.Radius = radius;
            this.Intensity = intensity;
        }
    }
}