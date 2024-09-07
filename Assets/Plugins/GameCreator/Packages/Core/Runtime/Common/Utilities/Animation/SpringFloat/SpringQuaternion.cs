using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    public class SpringQuaternion
    {
        private const float EPSILON = 1e-5f;
        private const float LOG_N_2 = 0.69314718056f;

        public const float DEFAULT_DECAY = 0.25f;

        // PROPERTIES: ----------------------------------------------------------------------------

        [field: NonSerialized] public Quaternion Current { get; set; }
        [field: NonSerialized] public Quaternion Target { get; set; }

        [field: NonSerialized] public float Decay { get; set; }

        [field: NonSerialized] private Vector3 Velocity { get; set; }

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public SpringQuaternion(float decay = DEFAULT_DECAY) : this(Quaternion.identity, decay)
        { }
        
        public SpringQuaternion(Quaternion value, float decay = DEFAULT_DECAY)
        {
            this.Current = value;
            this.Target = value;

            this.Decay = decay;
            this.Velocity = Vector3.zero;
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Update(float deltaTime)
        {
            float damping = DecayToDamping(this.Decay) / 2.0f;
            Vector3 remaining = QuaternionToAngleAxis(this.Current * Quaternion.Inverse(this.Target));
            Vector3 delta = this.Velocity + remaining * damping;
            float exponentDelta = NegativeExponent(damping * deltaTime);
            
            this.Current = QuaternionFromAngleAxis(exponentDelta * (remaining + delta * deltaTime)) * this.Target;
            this.Velocity = exponentDelta * (this.Velocity - delta * damping * deltaTime);
        }

        public void Update(float decay, float deltaTime)
        {
            this.Decay = decay;
            this.Update(deltaTime);
        }

        public void Update(Quaternion target, float decay, float deltaTime)
        {
            this.Target = target;
            this.Update(decay, deltaTime);
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

        private static Vector3 QuaternionToAngleAxis(Quaternion q)
        {
            if (q == Quaternion.identity) 
            {
                return Vector3.zero; 
            }

            float angle = 2.0f * Mathf.Acos(q.w); 
            float scale = Mathf.Sqrt(1.0f - Mathf.Clamp(q.w * q.w, -1.0f, 1.0f));

            Vector3 axis = scale < EPSILON
                ? new Vector3(q.x, q.y, q.z)
                : new Vector3(q.x / scale, q.y / scale, q.z / scale);
            
            return axis * angle * Mathf.Deg2Rad; 
        }
        
        private static Quaternion QuaternionFromAngleAxis(Vector3 scaledAxis) 
        {
            float angle = scaledAxis.magnitude;
            Vector3 axis = scaledAxis.normalized; 

            return Quaternion.AngleAxis(angle * Mathf.Rad2Deg, axis); 
        }
    }
}