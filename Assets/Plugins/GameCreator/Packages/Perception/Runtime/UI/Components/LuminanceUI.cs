using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Perception.UnityUI
{
    [AddComponentMenu("Game Creator/UI/Perception/Luminance UI")]
    [Icon(RuntimePaths.PACKAGES + "Perception/Editor/Gizmos/GizmoLuminanceUI.png")]
    
    [DefaultExecutionOrder(ApplicationManager.EXECUTION_ORDER_LAST_LATER)]
    
    [Serializable]
    public class LuminanceUI : MonoBehaviour
    {
        private const float DEFAULT_SMOOTH_TIME = 1f;
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Target = GetGameObjectPlayer.Create();
        
        [SerializeField] private float m_MinIntensity = 0f;
        [SerializeField] private float m_MaxIntensity = 1f;
        [SerializeField] private float m_SmoothTime = DEFAULT_SMOOTH_TIME;

        [SerializeField] private UpdateInterval m_Update = UpdateInterval.EveryFrame;
        [SerializeField] private PropertyGetDecimal m_Interval = GetDecimalConstantOne.Create;

        [SerializeField] private ProgressLuminance m_Lit = new ProgressLuminance();
        [SerializeField] private ProgressLuminance m_Dim = new ProgressLuminance();
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private Args m_Args;
        [NonSerialized] private float m_LastTimeUpdate = -999f;
        
        [NonSerialized] private AnimFloat m_LuminanceValue = new AnimFloat(0f);
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public float Luminance
        {
            get
            {
                Transform target = this.m_Target.Get<Transform>(this.m_Args);
                return target != null
                    ? LuminanceManager.Instance.LuminanceAt(target)
                    : 0f;
            }
        }
        
        // INITIALIZERS: --------------------------------------------------------------------------

        private void Awake()
        {
            this.m_Args = new Args(this.gameObject);
        }

        private void OnEnable()
        {
            float luminanceTarget = this.Luminance;
            
            this.m_LuminanceValue.Target = luminanceTarget;
            this.m_LuminanceValue.Current = luminanceTarget;
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
            
            this.m_LuminanceValue.UpdateWithDelta(
                this.m_LuminanceValue.Target,
                this.m_SmoothTime,
                Time.unscaledDeltaTime
            );

            float ratioLit = Mathf.InverseLerp(
                this.m_MinIntensity,
                this.m_MaxIntensity,
                this.m_LuminanceValue.Current
            );
            
            this.m_Lit.Refresh(ratioLit, false);
            this.m_Dim.Refresh(1f - ratioLit, false);
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void RunUpdate()
        {
            this.m_LuminanceValue.Target = this.Luminance;
        }
    }
}