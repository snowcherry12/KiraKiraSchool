using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [AddComponentMenu("")]
    [DisallowMultipleComponent]
    [Icon(RuntimePaths.PACKAGES + "Perception/Editor/Gizmos/GizmoScentUI.png")]
    
    [Serializable]
    public class Scent : MonoBehaviour, ISpatialHash
    {
        private const float DISPEL_MIN_DURATION = 0.01f;
        public const int SCENT_ID_NONE = 0;
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private string m_ScentTag;
        [SerializeField] private GameObject m_Source;
        
        [SerializeField] private float m_Duration;
        [SerializeField] private float m_Level;
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        [NonSerialized] private Action<Scent> m_OnCompleteCallback;
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        [field: NonSerialized] public float StartTime { get; private set; }
        [field: NonSerialized] public int NextScentId { get; internal set; }

        public string ScentTag => this.m_ScentTag;
        
        public GameObject Source => this.m_Source;
        
        public float Ratio
        {
            get
            {
                float dispel = this.m_Duration / (1f + SmellManager.Instance.Dissipation);
                return (Time.time - this.StartTime) / dispel;
            }
        }
        
        // INITIALIZERS: --------------------------------------------------------------------------

        private void OnEnable()
        {
            SpatialHashScent.Insert(this);
        }
        
        private void OnDisable()
        {
            SpatialHashScent.Remove(this);
            this.m_OnCompleteCallback?.Invoke(this);
        }
        
        public static Scent Create(string scentTag, Transform parent, GameObject source)
        {
            GameObject instance = new GameObject("Scent");
            instance.transform.SetParent(parent);
            
            Scent scent = instance.AddComponent<Scent>();

            scent.m_ScentTag = scentTag;
            scent.m_Source = source;
            
            return scent;
        }
        
        // UPDATE METHODS: ------------------------------------------------------------------------

        private void Update()
        {
            if (this.Ratio > 1f) this.gameObject.SetActive(false);
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Run(float duration, float level, Action<Scent> onComplete)
        {
            if (this.m_Source == null)
            {
                onComplete?.Invoke(this);
                return;
            }
            
            this.m_Duration = Math.Max(duration, DISPEL_MIN_DURATION);
            this.m_Level = level;
            
            this.m_OnCompleteCallback = onComplete;
            this.StartTime = Time.time;
            
            this.transform.SetPositionAndRotation(
                this.m_Source.transform.position,
                this.m_Source.transform.rotation
            );

            this.NextScentId = SCENT_ID_NONE;
            this.gameObject.SetActive(true);
        }
    }
}