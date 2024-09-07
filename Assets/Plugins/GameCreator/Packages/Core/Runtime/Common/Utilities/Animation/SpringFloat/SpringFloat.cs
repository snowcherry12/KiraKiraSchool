using System;

namespace GameCreator.Runtime.Common
{
    public class SpringFloat
    {
        private const float EPSILON = 1e-5f;
        private const float LOG_N_2 = 0.69314718056f;
        
        public const float DEFAULT_DECAY = 0.25f;
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        [field: NonSerialized] public float Current { get; set; }
        [field: NonSerialized] public float Target { get; set; }
        
        [field: NonSerialized] public float Decay { get; set; }
        
        [field: NonSerialized] private float Velocity { get; set; }
        
        // CONSTRUCTORS: --------------------------------------------------------------------------

        public SpringFloat(float value, float decay = DEFAULT_DECAY)
        {
            this.Current = value;
            this.Target = value;
            
            this.Decay = decay;
            this.Velocity = 0f;
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public float Update(float deltaTime)
        {
            float damping = DecayToDamping(this.Decay) / 2.0f;
            float remaining = this.Current - this.Target;
            float delta = this.Velocity + remaining * damping;
            float exponentDelta = NegativeExponent(damping * deltaTime);

            this.Current = exponentDelta * (remaining + delta * deltaTime) + this.Target;
            this.Velocity = exponentDelta * (this.Velocity - delta * damping * deltaTime);

            return this.Current;
        }
        
        public float Update(float decay, float deltaTime)
        {
            this.Decay = decay;
            return this.Update(deltaTime);
        }
        
        public float Update(float target, float decay, float deltaTime)
        {
            this.Target = target;
            return this.Update(decay, deltaTime);
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private static float DecayToDamping(float decay)
        {
            return 4.0f * LOG_N_2 / (decay + EPSILON);
        }
        
        private static float NegativeExponent(float x)
        {
            return 1.0f / (1.0f + x + 0.48f * x * x + 0.235f * x * x * x);
        }
    }
}