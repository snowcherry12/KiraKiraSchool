using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Perception.UnityUI
{
    [AddComponentMenu("Game Creator/UI/Perception/Smell UI")]
    [Icon(RuntimePaths.PACKAGES + "Perception/Editor/Gizmos/GizmoSmellUI.png")]
    
    [DefaultExecutionOrder(ApplicationManager.EXECUTION_ORDER_LAST_LATER)]
    
    [Serializable]
    public class SmellUI : MonoBehaviour
    {
        private const float DEFAULT_SMOOTH_TIME = 1f;
        
        public enum ScentType
        {
            All,
            Specific
        }
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Perception = GetGameObjectPlayer.Create();
        
        [SerializeField] private float m_MinIntensity = 0f;
        [SerializeField] private float m_MaxIntensity = 1f;
        [SerializeField] private float m_SmoothTime = DEFAULT_SMOOTH_TIME;

        [SerializeField] private UpdateInterval m_Update = UpdateInterval.EveryFrame;
        [SerializeField] private PropertyGetDecimal m_Interval = GetDecimalConstantOne.Create;

        [SerializeField] private ScentType m_Scents = ScentType.All;
        [SerializeField] private PropertyGetString m_Scent = GetStringId.Create("my-scent-tag");
        [SerializeField] private ProgressSmell m_Smell = new ProgressSmell();
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private Args m_Args;
        [NonSerialized] private Perception m_PerceptionCache;
        [NonSerialized] private float m_LastTimeUpdate = -999f;
        
        [NonSerialized] private AnimFloat m_ScentValue = new AnimFloat(0f);
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        private float Intensity
        {
            get
            {
                if (this.m_PerceptionCache == null) return 0f;
            
                SensorSmell smellSensor = this.m_PerceptionCache.GetSensor<SensorSmell>();
                if (smellSensor == null) return 0;
            
                return this.m_Scents switch
                {
                    ScentType.All => smellSensor.GetIntensity(),
                    ScentType.Specific => smellSensor.GetIntensity(this.m_Scent.Get(this.m_Args)),
                    _ => throw new ArgumentOutOfRangeException()
                };   
            }
        }
        
        // INITIALIZERS: --------------------------------------------------------------------------

        private void Awake()
        {
            this.m_Args = new Args(this.gameObject);
        }

        private void OnEnable()
        {
            this.m_PerceptionCache = this.m_Perception.Get<Perception>(this.m_Args);
            if (this.m_PerceptionCache == null) return;

            SensorSmell sensorSmell = this.m_PerceptionCache.GetSensor<SensorSmell>();
            if (sensorSmell == null) return;
            
            this.m_ScentValue.Target = 0f;
            this.m_ScentValue.Current = 0f;
        }

        // UPDATE METHODS: ------------------------------------------------------------------------

        private void LateUpdate()
        {
            switch (this.m_Update)
            {
                case UpdateInterval.EveryFrame:
                    this.RunUpdate();
                    break;
                
                case UpdateInterval.Interval:
                    float interval = (float) this.m_Interval.Get(this.m_Args);
                    if (Time.unscaledTime >= this.m_LastTimeUpdate + interval)
                    {
                        this.RunUpdate();
                        this.m_LastTimeUpdate = Time.unscaledTime;
                    }
                    break;
                
                default: throw new ArgumentOutOfRangeException();
            }
            
            this.m_ScentValue.UpdateWithDelta(
                this.m_ScentValue.Target,
                this.m_SmoothTime,
                Time.unscaledDeltaTime
            );
            
            float ratioSmell = Mathf.InverseLerp(
                this.m_MinIntensity,
                this.m_MaxIntensity,
                this.m_ScentValue.Current
            );
            
            this.m_Smell.Refresh(ratioSmell, false);
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void RunUpdate()
        {
            this.m_ScentValue.Target = this.Intensity;
        }
    }
}