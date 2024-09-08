using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameCreator.Runtime.Perception
{
    [Title("Smell")]
    [Category("Smell")]
    
    [Image(typeof(IconNose), ColorTheme.Type.TextLight)]
    [Description("Allows characters to use their olfactory sense to detect changes in the scene")]
    
    [Serializable]
    public class SensorSmell : TSensor
    {
        public struct Trace
        {
            public int ScentId { get; }
            public float Score { get; }
            public string Tag { get; }

            public Trace(int scentId, float score, string tag)
            {
                this.ScentId = scentId;
                this.Score = score;
                this.Tag = tag;
            }
        }
        
        // CONSTANTS: -----------------------------------------------------------------------------

        private const float GIZMOS_MAX_ALPHA_NORMAL = 0.25f;
        private const float GIZMOS_MAX_ALPHA_ACTIVE = 0.5f;
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private UpdateMode m_Update = UpdateMode.EveryFrame;
        [SerializeField] private PropertyGetDecimal m_Interval = GetDecimalRandomRange.Create();

        [SerializeField] private PropertyGetDecimal m_Radius = GetDecimalDecimal.Create(5f);
        
        [FormerlySerializedAs("m_UseOccluders")] [SerializeField] private EnablerLayerMask m_UseObstruction = new EnablerLayerMask();
        [SerializeField] private PropertyGetDecimal m_MinIntensity = GetDecimalConstantZero.Create;
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        [NonSerialized] private float m_LastUpdateTime = -999f;
        
        [NonSerialized] private List<ISpatialHash> m_ScentsNearby = new List<ISpatialHash>();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => "Smell";

        [field: NonSerialized]
        private Dictionary<string, Trace> Scents { get; } = new Dictionary<string, Trace>();

        public float MinIntensity => (float) this.m_MinIntensity.Get(this.Perception.Args);
        
        public IEnumerable<Trace> Traces => this.Scents.Values;

        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected override void OnUpdate()
        {
            switch (this.m_Update)
            {
                case UpdateMode.EveryFrame:
                    this.RunUpdate();
                    break;
                
                case UpdateMode.Interval:
                {
                    float interval = (float) this.m_Interval.Get(this.Perception.Args);
                    if (this.Perception.Time >= this.m_LastUpdateTime + interval)
                    {
                        this.RunUpdate();
                    }
                    break;
                }
                case UpdateMode.Manual: break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
        
        protected override void OnFixedUpdate()
        { }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public bool CanSmell(StimulusScent stimulus)
        {
            float distance = Vector3.Distance(
                this.Perception.transform.position,
                stimulus.Position
            );

            if (distance > stimulus.Radius) return false;
            float perceivedIntensity = (1f - distance / stimulus.Radius) * stimulus.Intensity;
            
            if (this.m_UseObstruction.IsEnabled)
            {
                float value = Obstruction.GetScentDamp(
                    stimulus.Position,
                    this.Perception.gameObject,
                    this.m_UseObstruction.Value
                );
                
                perceivedIntensity -= value;
            }
            
            if (SmellManager.Instance.Dissipation > perceivedIntensity) return false;
            return this.MinIntensity <= perceivedIntensity;
        }
        
        public void Run()
        {
            if (this.m_Update != UpdateMode.Manual) return; 
            this.RunUpdate();
        }

        public float GetIntensity()
        {
            float maxScent = 0f;
            foreach (KeyValuePair<string, Trace> entry in this.Scents)
            {
                maxScent = Math.Max(maxScent, entry.Value.Score);
            }

            return maxScent;
        }
        
        public float GetIntensity(string scentTag)
        {
            return this.Scents.TryGetValue(scentTag, out Trace trace) ? trace.Score : 0f;
        }

        public int GetScentId(string scentTag)
        {
            return this.Scents.TryGetValue(scentTag, out Trace trace) ? trace.ScentId : 0;
        }

        public string GetMostIntenseScent()
        {
            string maxScentTag = string.Empty;
            float maxScent = 0f;
            
            foreach (KeyValuePair<string, Trace> entry in this.Scents)
            {
                if (maxScent >= entry.Value.Score) continue;
                
                maxScent = entry.Value.Score;
                maxScentTag = entry.Key;
            }

            return maxScentTag;
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private void RunUpdate()
        {
            float radius = (float) this.m_Radius.Get(this.Perception.Args);
            float minIntensity = this.MinIntensity;
            
            this.Scents.Clear();
            
            SpatialHashScent.Find(
                this.Perception.transform.position, 
                radius,
                this.m_ScentsNearby
            );
            
            foreach (ISpatialHash scentsNearby in this.m_ScentsNearby)
            {
                Scent scent = scentsNearby as Scent;
                if (scent == null) continue;
                
                float score = this.CalculateScore(radius, scent);
                
                if (score <= float.Epsilon) continue;
                if (score < minIntensity) continue;
                
                if (this.Scents.ContainsKey(scent.ScentTag))
                {
                    if (this.Scents[scent.ScentTag].Score < score)
                    {
                        this.Scents[scent.ScentTag] = new Trace(
                            scent.GetInstanceID(),
                            score,
                            scent.ScentTag
                        );
                    }
                }
                else
                {
                    this.Scents[scent.ScentTag] = new Trace(
                        scent.GetInstanceID(),
                        score,
                        scent.ScentTag
                    );
                }
            }
            
            this.m_LastUpdateTime = this.Perception.Time;
        }
        
        private float CalculateScore(float radius, Scent scent)
        {
            float distance = Vector3.Distance(
                this.Perception.transform.position, 
                scent.transform.position
            );
            
            float scentRadius = radius * scent.Ratio;
            if (distance > scentRadius) return 0f;
            
            float score = (1f - distance / scentRadius) * (1f - scent.Ratio);
            
            if (this.m_UseObstruction.IsEnabled)
            {
                score -= Obstruction.GetScentDamp(
                    scent.transform.position,
                    this.Perception.gameObject,
                    this.m_UseObstruction.Value
                );
            }
            
            return Math.Max(score, 0f);
        }
        
        // GIZMOS: --------------------------------------------------------------------------------

        protected override void OnDrawGizmos(Perception perception)
        {
            if (!Application.isPlaying) return;
            float radius = (float) this.m_Radius.Get(this.Perception.Args);

            Scent[] scents = UnityEngine.Object.FindObjectsByType<Scent>(FindObjectsSortMode.None);
            foreach (Scent scent in scents)
            {
                if (!scent.isActiveAndEnabled) continue;
                
                string scentTag = scent.ScentTag;
                float scentRadius = radius * scent.Ratio;
                
                float normalAlpha = (1f - scent.Ratio) * GIZMOS_MAX_ALPHA_NORMAL;
                Gizmos.color = new Color(1f, 0f, 0, normalAlpha);
                if (this.Scents.TryGetValue(scentTag, out Trace trace))
                {
                    if (trace.ScentId == scent.GetInstanceID())
                    {
                        float activeAlpha = (1f - scent.Ratio) * GIZMOS_MAX_ALPHA_ACTIVE;
                        Gizmos.color = new Color(0f, 1f, 0, activeAlpha);
                    }
                }
                
                Gizmos.DrawSphere(scent.transform.position, scentRadius);
            }
        }
    }
}