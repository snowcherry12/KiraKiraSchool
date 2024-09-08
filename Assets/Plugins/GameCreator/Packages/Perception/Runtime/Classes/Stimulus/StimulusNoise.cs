using System;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    public struct StimulusNoise : IStimulus
    {
        // PROPERTIES: ----------------------------------------------------------------------------

        [field: NonSerialized] public string Tag { get; }
        
        [field: NonSerialized] public Vector3 Position { get; }
        [field: NonSerialized] public float Radius { get; }
        
        [field: NonSerialized] public float Intensity { get; private set; }

        // CONSTRUCTORS: --------------------------------------------------------------------------
        
        public StimulusNoise(string tag, Vector3 position, float radius, float intensity)
        {
            this.Tag = tag;
            this.Position = position;
            this.Radius = radius;
            this.Intensity = intensity;
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void DecreaseIntensity(float amount)
        {
            this.Intensity -= amount;
        }
    }
}