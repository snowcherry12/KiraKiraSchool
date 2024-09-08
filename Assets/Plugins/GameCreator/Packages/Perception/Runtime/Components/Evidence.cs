using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Icon(RuntimePaths.PACKAGES + "Perception/Editor/Gizmos/GizmoEvidence.png")]
    [AddComponentMenu("Game Creator/Perception/Evidence")]
    [DisallowMultipleComponent]
    
    [Serializable]
    public class Evidence : MonoBehaviour, ISpatialHash
    {
        private static readonly Color GIZMO_TAMPER_COLOR_ON = new Color(1f, 0f, 0f, 0.5f);
        private static readonly Color GIZMO_TAMPER_COLOR_OFF = new Color(0f, 1f, 0f, 0.5f);
        private static readonly Vector3 GIZMO_TAMPER_SIZE = Vector3.one * 0.25f;
        
        private static readonly Color GIZMO_HEAR_COLOR = new Color(1f, 1f, 0f, 0.25f);
        private static readonly Color GIZMO_SCENT_COLOR = new Color(1f, 0f, 1f, 0.25f);

        private const int GIZMO_OCTAHEDRON_NUM = 3;
        
        private static readonly List<ISpatialHash> ListPerceptions = new List<ISpatialHash>();
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private PropertyGetString m_Tag = GetStringId.Create("my-evidence-tag");
        
        [SerializeField] private bool m_OnSight = true;
        
        [SerializeField] private bool m_EmitNoise;
        [SerializeField] private PropertyGetDecimal m_NoiseRadius = GetDecimalDecimal.Create(5f);
        [SerializeField] private PropertyGetDecimal m_NoiseLevel = GetDecimalConstantOne.Create;
        
        [SerializeField] private bool m_EmitScent;
        [SerializeField] private PropertyGetDecimal m_ScentInterval = GetDecimalConstantOne.Create;
        [SerializeField] private PropertyGetDecimal m_ScentRadius = GetDecimalDecimal.Create(3f);
        [SerializeField] private PropertyGetDecimal m_ScentLevel = GetDecimalConstantOne.Create;
        
        [SerializeField] private bool m_StartTampered;

        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private Args m_Args;
        [NonSerialized] private bool m_IsTampered;

        [NonSerialized] private float m_LastTimeEmitScent;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public bool SensorSee => this.m_OnSight;
        public bool SensorHear => this.m_EmitNoise;
        public bool SensorSmell => this.m_EmitScent;
        
        public bool IsTampered
        {
            get => this.isActiveAndEnabled && this.m_IsTampered;
            set
            {
                if (this.m_IsTampered == value) return;
                this.m_IsTampered = value;
                
                if (this.m_IsTampered) this.EventTamper?.Invoke();
                this.EventChange?.Invoke();
            }
        }

        // EVENTS: --------------------------------------------------------------------------------

        public event Action EventChange;
        public event Action EventTamper;

        // INITIALIZE METHODS: --------------------------------------------------------------------

        private void Awake()
        {
            this.m_Args = new Args(this.gameObject);
        }

        private void Start()
        {
            if (this.m_StartTampered)
            {
                this.IsTampered = true;
            }
        }

        private void OnEnable()
        {
            SpatialHashEvidence.Insert(this);
        }

        private void OnDisable()
        {
            SpatialHashEvidence.Remove(this);
        }
        
        // UPDATE METHODS: ------------------------------------------------------------------------

        private void Update()
        {
            if (!this.m_IsTampered) return;
            
            if (this.m_EmitNoise) this.UpdateNoise();
            if (this.m_EmitScent) this.UpdateScent();
        }

        private void UpdateNoise()
        {
            float radius = (float) this.m_NoiseRadius.Get(this.gameObject);
            float level = (float) this.m_NoiseLevel.Get(this.gameObject);
            
            HearManager.Instance.AddGizmoNoise(this.transform.position, radius, 0f);
            SpatialHashPerception.Find(this.transform.position, radius, ListPerceptions);
            
            foreach (ISpatialHash spatialHash in ListPerceptions)
            {
                Perception perception = spatialHash as Perception;
                if (perception == null) continue;
                
                string evidenceTag = this.GetTag(perception.gameObject);
                if (perception.GetEvidence(evidenceTag)) continue;

                SensorHear sensorHear = perception.GetSensor<SensorHear>();
                if (sensorHear == null) continue;
                
                StimulusNoise noise = new StimulusNoise(
                    evidenceTag,
                    this.transform.position,
                    radius,
                    level
                );
                
                if (sensorHear.CanHear(noise))
                {
                    perception.InvestigateEvidence(this);
                }
            }
        }
        
        private void UpdateScent()
        {
            float interval = (float) this.m_ScentInterval.Get(this.m_Args);
            if (Time.time < this.m_LastTimeEmitScent + interval) return;
            
            float radius = (float) this.m_ScentRadius.Get(this.gameObject);
            float level = (float) this.m_ScentLevel.Get(this.gameObject);
            
            SpatialHashPerception.Find(this.transform.position, radius, ListPerceptions);

            foreach (ISpatialHash spatialHash in ListPerceptions)
            {
                Perception perception = spatialHash as Perception;
                if (perception == null) continue;
                
                string evidenceTag = this.GetTag(perception.gameObject);
                if (perception.GetEvidence(evidenceTag)) continue;

                SensorSmell sensorSmell = perception.GetSensor<SensorSmell>();
                if (sensorSmell == null) continue;
                
                StimulusScent scent = new StimulusScent(
                    evidenceTag,
                    this.transform.position,
                    radius,
                    level
                );
                
                if (sensorSmell.CanSmell(scent))
                {
                    perception.InvestigateEvidence(this);
                }
            }
            
            this.m_LastTimeEmitScent = Time.time;
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public string GetTag(GameObject target)
        {
            this.m_Args.ChangeTarget(target);
            return this.m_Tag.Get(this.m_Args);
        }
        
        // GIZMOS: --------------------------------------------------------------------------------
        
        private void OnDrawGizmosSelected()
        {
            if (Application.isPlaying)
            {
                Gizmos.color = this.IsTampered ? GIZMO_TAMPER_COLOR_ON : GIZMO_TAMPER_COLOR_OFF;
                Gizmos.DrawCube(this.transform.position, GIZMO_TAMPER_SIZE);

                if (this.SensorHear)
                {
                    Gizmos.color = GIZMO_HEAR_COLOR;
                    GizmosExtension.Octahedron(
                        this.transform.position,
                        Quaternion.identity,
                        (float) this.m_NoiseRadius.Get(this.gameObject),
                        GIZMO_OCTAHEDRON_NUM
                    );
                }
                
                if (this.SensorSmell)
                {
                    Gizmos.color = GIZMO_SCENT_COLOR;
                    GizmosExtension.Octahedron(
                        this.transform.position,
                        Quaternion.identity,
                        (float) this.m_ScentRadius.Get(this.gameObject),
                        GIZMO_OCTAHEDRON_NUM
                    );
                }
            }
            else
            {
                if (this.SensorHear)
                {
                    Gizmos.color = GIZMO_HEAR_COLOR;
                    GizmosExtension.Octahedron(
                        this.transform.position,
                        Quaternion.identity,
                        (float) this.m_NoiseRadius.EditorValue,
                        GIZMO_OCTAHEDRON_NUM
                    );
                }
                
                if (this.SensorSmell)
                {
                    Gizmos.color = GIZMO_SCENT_COLOR;
                    GizmosExtension.Octahedron(
                        this.transform.position,
                        Quaternion.identity,
                        (float) this.m_ScentRadius.EditorValue,
                        GIZMO_OCTAHEDRON_NUM
                    );
                }
            }
        }
    }
}