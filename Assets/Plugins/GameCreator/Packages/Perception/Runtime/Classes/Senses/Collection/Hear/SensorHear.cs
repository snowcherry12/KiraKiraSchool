using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameCreator.Runtime.Perception
{
    [Title("Hear")]
    [Category("Hear")]
    
    [Image(typeof(IconEar), ColorTheme.Type.TextLight)]
    [Description("Allows characters to use their hearing sense to detect changes in the scene")]
    
    [Serializable]
    public class SensorHear : TSensor
    {
        internal class Noise
        {
            public float Intensity { get; set; }
            public Vector3 Position { get; set; }

            public void Update(float decay)
            {
                this.Intensity = Math.Max(this.Intensity - decay, 0f);
            }
        }
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [FormerlySerializedAs("m_UseOccluders")] [SerializeField] private EnablerLayerMask m_UseObstruction = new EnablerLayerMask();
        [SerializeField] private PropertyGetDecimal m_MinIntensity = GetDecimalConstantZero.Create;
        [SerializeField] private PropertyGetDecimal m_MaxIntensity = GetDecimalConstantOne.Create;
        [SerializeField] private PropertyGetDecimal m_DecayTime = GetDecimalConstantTwo.Create;
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] 
        private Dictionary<PropertyName, Noise> m_Noises = new Dictionary<PropertyName, Noise>();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => "Hear";

        public float DecayTime => (float) this.m_DecayTime.Get(this.Perception.Args);
        
        public float HighestIntensity
        {
            get
            {
                float maxIntensity = 0f;
                foreach (KeyValuePair<PropertyName, Noise> entry in this.m_Noises)
                {
                    maxIntensity = Math.Max(
                        entry.Value?.Intensity ?? 0f,
                        maxIntensity
                    );
                }

                return maxIntensity;
            }
        }
        
        [field: NonSerialized] public StimulusNoise LastNoiseReceived { get; private set; }
        
        // EVENTS: --------------------------------------------------------------------------------

        public event Action<string, float> EventReceiveNoise;
        public event Action<string> EventHearNoise;
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public float GetIntensity(string noiseTag)
        {
            return this.m_Noises.TryGetValue(noiseTag, out Noise noise) && noise != null
                ? noise.Intensity
                : 0f;
        }
        
        public Vector3 GetPositionLastHeard(string noiseTag)
        {
            return this.m_Noises.TryGetValue(noiseTag, out Noise noise) && noise != null
                ? noise.Position
                : default;
        }
        
        public bool CanHear(StimulusNoise stimulus)
        {
            float stimulusIntensity = this.GetStimulusIntensity(stimulus);
            return this.CanHearStimulusIntensity(stimulusIntensity);
        }
        
        public void OnReceiveNoise(StimulusNoise stimulus)
        {
            float stimulusIntensity = this.GetStimulusIntensity(stimulus);

            Noise noise = this.m_Noises.TryGetValue(stimulus.Tag, out noise)
                ? noise
                : null;

            if (noise == null)
            {
                noise = new Noise();
                this.m_Noises[stimulus.Tag] = noise;
            }

            noise.Position = stimulus.Position;
            noise.Intensity = Math.Max(
                noise.Intensity,
                stimulusIntensity
            );
            
            this.EventReceiveNoise?.Invoke(stimulus.Tag, stimulusIntensity);
            
            if (this.CanHearStimulusIntensity(stimulusIntensity))
            {
                this.LastNoiseReceived = stimulus;
                this.EventHearNoise?.Invoke(stimulus.Tag);
            }
        }

        // PROTECTED METHODS: ---------------------------------------------------------------------
        
        protected override void OnUpdate()
        {
            float decayTime = this.DecayTime;
            foreach (KeyValuePair<PropertyName, Noise> entry in this.m_Noises)
            {
                float decay = this.Perception.DeltaTime / decayTime;
                this.m_Noises[entry.Key].Update(decay);
            }
        }
        
        protected override void OnFixedUpdate()
        { }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private float GetStimulusIntensity(StimulusNoise stimulus)
        {
            if (this.m_UseObstruction.IsEnabled)
            {
                float value = Obstruction.GetNoiseDamp(
                    stimulus.Position,
                    this.Perception.gameObject,
                    this.m_UseObstruction.Value
                );
                
                stimulus.DecreaseIntensity(value);
            }

            return stimulus.Intensity;
        }

        private bool CanHearStimulusIntensity(float stimulusIntensity)
        {
            float threshold = HearManager.Instance.DinFor(this.Perception);
            
            if (threshold > stimulusIntensity) return false;

            float minIntensity = (float) this.m_MinIntensity.Get(this.Perception.Args);
            float maxIntensity = (float) this.m_MaxIntensity.Get(this.Perception.Args);

            return minIntensity <= stimulusIntensity && maxIntensity >= stimulusIntensity;
        }
        
        // GIZMOS: --------------------------------------------------------------------------------

        protected override void OnDrawGizmos(Perception perception)
        { }
    }
}