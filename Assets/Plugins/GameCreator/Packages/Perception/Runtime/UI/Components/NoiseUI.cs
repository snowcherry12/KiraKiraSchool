using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Perception.UnityUI
{
    [AddComponentMenu("Game Creator/UI/Perception/Noise UI")]
    [Icon(RuntimePaths.PACKAGES + "Perception/Editor/Gizmos/GizmoNoiseUI.png")]
    
    [DefaultExecutionOrder(ApplicationManager.EXECUTION_ORDER_LAST_LATER)]
    
    [Serializable]
    public class NoiseUI : MonoBehaviour
    {
        private const float DIN_SMOOTH_TIME_DECAY = 0.25f;
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Perception = GetGameObjectPlayer.Create();
        
        [SerializeField] private float m_MinIntensity = 0f;
        [SerializeField] private float m_MaxIntensity = 1f;
        
        [SerializeField] private TimeMode m_Time = new TimeMode(TimeMode.UpdateMode.GameTime);
        [SerializeField] private UpdateInterval m_Update = UpdateInterval.EveryFrame;
        [SerializeField] private PropertyGetDecimal m_Interval = GetDecimalConstantOne.Create;

        [SerializeField] private ProgressAmbient m_Din = new ProgressAmbient();
        [SerializeField] private ProgressNoise m_Noise = new ProgressNoise();
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private Args m_Args;
        [NonSerialized] private Perception m_PerceptionCache;
        
        [NonSerialized] private float m_LastTimeUpdate = -999f;
        
        [NonSerialized] private AnimFloat m_DinValue = new AnimFloat(0f);
        
        // INITIALIZERS: --------------------------------------------------------------------------

        private void Awake()
        {
            this.m_Args = new Args(this.gameObject);
        }

        private void OnEnable()
        {
            this.m_PerceptionCache = this.m_Perception.Get<Perception>(this.m_Args);
            
            this.m_DinValue.Target = 0f;
            this.m_DinValue.Current = 0f;
        }

        // UPDATE METHODS: ------------------------------------------------------------------------

        private void LateUpdate()
        {
            bool updateData = false;
            
            switch (this.m_Update)
            {
                case UpdateInterval.EveryFrame:
                    updateData = true;
                    break;
                
                case UpdateInterval.Interval:
                    float interval = (float) this.m_Interval.Get(this.m_Args);
                    if (this.m_Time.Time >= this.m_LastTimeUpdate + interval)
                    {
                        updateData = true;
                        this.m_LastTimeUpdate = this.m_Time.Time;
                    }
                    break;
                
                default: throw new ArgumentOutOfRangeException();
            }

            if (updateData)
            {
                this.m_DinValue.Target = this.m_PerceptionCache != null
                    ? HearManager.Instance.DinFor(this.m_PerceptionCache)
                    : 0f;
            }

            if (this.m_PerceptionCache == null) return;
            
            SensorHear sensorHear = this.m_PerceptionCache.GetSensor<SensorHear>();
            if (sensorHear == null) return;
            
            this.m_DinValue.UpdateWithDelta(
                this.m_DinValue.Target,
                DIN_SMOOTH_TIME_DECAY,
                this.m_Time.DeltaTime
            );

            float ratioDin = Mathf.InverseLerp(
                this.m_MinIntensity,
                this.m_MaxIntensity,
                this.m_DinValue.Current
            );
            
            float ratioNoise = Mathf.InverseLerp(
                this.m_MinIntensity,
                this.m_MaxIntensity,
                sensorHear.HighestIntensity
            );

            this.m_Din.Refresh(ratioDin, false);
            this.m_Noise.Refresh(ratioNoise, ratioNoise >= ratioDin);
        }
    }
}