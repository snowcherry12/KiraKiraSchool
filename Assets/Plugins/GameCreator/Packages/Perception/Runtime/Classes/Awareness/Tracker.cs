using System;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    public class Tracker
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private float m_Awareness;

        [NonSerialized] private readonly Perception m_Perception;
        [NonSerialized] private float m_LastIncreaseTime;

        // PROPERTIES: ----------------------------------------------------------------------------

        public float Awareness
        {
            get => this.m_Awareness;
            set
            {
                float newAwareness = Math.Clamp(value, 0f, 1f);
                if (Math.Abs(this.m_Awareness - newAwareness) < float.Epsilon) return;

                if (this.m_Awareness <= newAwareness) this.m_LastIncreaseTime = this.m_Perception.Time;

                AwareStage prevStage = GetStage(this.m_Awareness);
                AwareStage nextStage = GetStage(newAwareness);
                
                this.m_Awareness = newAwareness;
                
                this.EventChangeAwareness?.Invoke(this.Target, this.m_Awareness);
                if (prevStage == nextStage) return;
                
                this.EventChangeStage?.Invoke(this.Target, nextStage);
            }
        }

        [field: NonSerialized] public GameObject Target { get; }

        public AwareStage Stage => GetStage(this.m_Awareness);

        // EVENTS: --------------------------------------------------------------------------------

        public event Action<GameObject, float> EventChangeAwareness;
        public event Action<GameObject, AwareStage> EventChangeStage; 
        
        // INITIALIZERS: --------------------------------------------------------------------------

        public Tracker(Perception perception, GameObject target)
        {
            this.m_Awareness = 0f;

            this.m_Perception = perception;
            this.m_LastIncreaseTime = this.m_Perception.Time;
            
            this.Target = target;
        }
        
        public void Enable()
        {
            this.Awareness = 0f;
        }
        
        public void Disable()
        {
            this.Awareness = 0f;
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public void Update()
        {
            AwareStage currentStage = GetStage(this.Awareness);
            float forgetTime = this.m_LastIncreaseTime;

            forgetTime += currentStage == AwareStage.Aware
                ? Math.Max(this.m_Perception.ForgetDelay, this.m_Perception.AwareDuration)
                : this.m_Perception.ForgetDelay;
            
            if (this.m_Perception.Time <= forgetTime) return;
            this.Awareness -= this.m_Perception.DeltaTime * this.m_Perception.ForgetSpeed;
        }
        
        // PUBLIC STATIC METHODS: -----------------------------------------------------------------

        public static AwareStage GetStage(float awareness)
        {
            return awareness switch
            {
                <= 0.05f => AwareStage.None,
                <= 0.50f => AwareStage.Suspicious,
                <= 0.95f => AwareStage.Alert,
                _ => AwareStage.Aware
            };
        }
    }
}