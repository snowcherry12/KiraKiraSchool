using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class Cooldown : ISerializationCallbackReceiver
    {
        // EXPOSED MEMBERS: --------------------------------------------------------------------------------------------

        [SerializeField] private float m_ElapsedTime;
        [SerializeField] private float m_Duration;

        // PROPERTIES: -------------------------------------------------------------------------------------------------

        [field: NonSerialized] public float StartTime { get; private set; }
        [field: NonSerialized] public float Duration { get; private set; }

        public bool IsReady => Time.time >= this.StartTime + this.Duration;

        public float Completion
        {
            get
            {
                if (this.Duration <= 0f) return 0f;

                float ratio = (Time.time - this.StartTime) / this.Duration;
                return Mathf.Clamp01(ratio);
            }
        }
        
        // CONSTRUCTORS: -----------------------------------------------------------------------------------------------

        public Cooldown()
        { }

        public Cooldown(Item item, Args args)
        {
            this.Set(item, args);
        }

        public Cooldown(float startTime, float duration)
        {
            this.Set(startTime, duration);
        }

        public Cooldown(Cooldown cooldown)
        {
            this.Set(cooldown.StartTime, cooldown.Duration);
        }

        // PUBLIC METHODS: ---------------------------------------------------------------------------------------------

        public void Set(float startTime, float duration)
        {
            this.StartTime = startTime;
            this.Duration = duration;
        }

        public void Set(Item item, Args args)
        {
            float startTime = Time.time;
            float duration = item.Usage.GetCooldownDuration(args);
            
            this.Set(startTime, duration);
        }

        public void Reset()
        {
            this.StartTime = 0f;
            this.Duration = 0f;
        }
        
        // SERIALIZATION CALLBACKS: ------------------------------------------------------------------------------------

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            this.m_ElapsedTime = Time.time - this.StartTime;
            this.m_Duration = this.Duration;
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            this.StartTime = Time.time - this.m_ElapsedTime;
            this.Duration = this.m_Duration;
        }
    }
}