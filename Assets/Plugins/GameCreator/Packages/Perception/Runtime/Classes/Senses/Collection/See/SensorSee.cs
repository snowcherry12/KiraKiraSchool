using System;
using System.Collections.Generic;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Title("See")]
    [Category("See")]
    
    [Image(typeof(IconEye), ColorTheme.Type.TextLight)]
    [Description("Allows characters to use their vision sight to detect changes in the scene")]
    
    [Serializable]
    public class SensorSee : TSensor
    {
        private struct Optometry
        {
            public Vector3 opticsPosition;
            public Quaternion opticsRotation;
            
            public float dimThreshold;
            public float litThreshold;
            
            public float primaryRadius;
            public float peripheralRadius;
            
            public float primaryAngleHalf;
            public float peripheralAngleHalf;
            
            public float primaryHeightHalf;
            public float peripheralHeightHalf;
        }
        
        // CONSTANTS: -----------------------------------------------------------------------------
        
        private static readonly Color GIZMOS_HIT_COLOR = new Color(1, 0, 0, 0.5f);
        private const float GIZMOS_HIT_RADIUS = 0.05f;
        
        private static readonly Color GIZMOS_PRIMARY_COLOR = new Color(0, 1, 0, 0.5f);
        private static readonly Color GIZMOS_PERIPHERAL_COLOR = new Color(0, 1, 0, 0.25f);
        
        private const float RAY_OFFSET = 0.005f;
        private static readonly RaycastHit[] HITS = new RaycastHit[64];
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private UpdateMode m_Update = UpdateMode.EveryFrame;
        [SerializeField] private PropertyGetDecimal m_Interval = GetDecimalRandomRange.Create();
        [SerializeField] private PropertyGetDecimal m_DetectionSpeed = GetDecimalConstantOne.Create;
        [SerializeField] private LayerMask m_LayerMask = Physics.DefaultRaycastLayers;

        [SerializeField] private PropertyGetGameObject m_Optics = GetGameObjectSelf.Create();
        [SerializeField] private Vector3 m_OffsetPosition = Vector3.zero;
        [SerializeField] private Vector3 m_OffsetRotation = Vector3.zero;
        
        [SerializeField] private bool m_UseLuminance = true;
        [SerializeField] private PropertyGetDecimal m_DimThreshold = GetDecimalDecimal.Create(0.25f);
        [SerializeField] private PropertyGetDecimal m_LitThreshold = GetDecimalDecimal.Create(0.75f);
        
        [SerializeField] private PropertyGetDecimal m_PrimaryAngle = GetDecimalDecimal.Create(120f);
        [SerializeField] private PropertyGetDecimal m_PrimaryRadius = GetDecimalDecimal.Create(3f);
        [SerializeField] private PropertyGetDecimal m_PrimaryHeight = GetDecimalDecimal.Create(2.5);
        
        [SerializeField] private PropertyGetDecimal m_PeripheralRadius = GetDecimalDecimal.Create(2f);
        [SerializeField] private PropertyGetDecimal m_PeripheralAngle = GetDecimalDecimal.Create(120f);
        [SerializeField] private PropertyGetDecimal m_PeripheralHeight = GetDecimalDecimal.Create(0f);
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private float m_LastUpdateTime = -999f;
        [NonSerialized] private List<Vector3> m_DebugHits = new List<Vector3>();
        [NonSerialized] private List<ISpatialHash> m_EvidenceNearby = new List<ISpatialHash>();
        [NonSerialized] private Dictionary<int, Vector3> m_PositionsLastSeen = new Dictionary<int, Vector3>();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => "See";

        private Vector3 OpticsPosition
        {
            get
            {
                Transform transform = this.m_Optics.Get<Transform>(this.Perception.Args);
                Vector3 offset = this.Perception.transform.TransformDirection(this.m_OffsetPosition);
                
                return transform != null
                    ? transform.position + offset
                    : this.Perception.transform.position + offset;
            }
        }

        private Quaternion OpticsRotation
        {
            get
            {
                Transform transform = this.m_Optics.Get<Transform>(this.Perception.Args);
                return transform != null
                    ? transform.rotation * Quaternion.Euler(this.m_OffsetRotation)
                    : this.Perception.transform.rotation * Quaternion.Euler(this.m_OffsetRotation);
            }
        }

        // EVENTS: --------------------------------------------------------------------------------

        public event Action<GameObject> EventSee; 

        // PROTECTED METHODS: ---------------------------------------------------------------------
        
        protected override void OnUpdate()
        { }

        protected override void OnFixedUpdate()
        {
            switch (this.m_Update)
            {
                case UpdateMode.EveryFrame:
                    this.RunUpdate(this.Perception.DeltaTime);
                    break;
                
                case UpdateMode.Interval:
                {
                    float interval = (float) this.m_Interval.Get(this.Perception.Args);
                    if (this.Perception.Time >= this.m_LastUpdateTime + interval)
                    {
                        this.RunUpdate(interval);
                    }
                    break;
                }
                case UpdateMode.Manual: break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Run(float deltaTime)
        {
            if (this.m_Update != UpdateMode.Manual) return; 
            this.RunUpdate(deltaTime);
        }

        public Vector3 GetPositionLastSeen(GameObject target)
        {
            if (target == null) return default;
            int instanceId = target.GetInstanceID();
            
            return this.m_PositionsLastSeen.GetValueOrDefault(instanceId);
        }

        public bool CanSee(GameObject target)
        {
            if (target == null) return false;
            
            Optometry optometry = new Optometry
            {
                opticsPosition = this.OpticsPosition,
                opticsRotation = this.OpticsRotation,
                dimThreshold = (float) this.m_DimThreshold.Get(this.Perception.Args),
                litThreshold = (float) this.m_LitThreshold.Get(this.Perception.Args),
                primaryRadius = (float) this.m_PrimaryRadius.Get(this.Perception.Args),
                peripheralRadius = (float) this.m_PeripheralRadius.Get(this.Perception.Args),
                primaryAngleHalf = (float) this.m_PrimaryAngle.Get(this.Perception.Args) * 0.5f,
                peripheralAngleHalf = (float) this.m_PeripheralAngle.Get(this.Perception.Args) * 0.5f,
                primaryHeightHalf = (float) this.m_PrimaryHeight.Get(this.Perception.Args) * 0.5f,
                peripheralHeightHalf = (float) this.m_PeripheralHeight.Get(this.Perception.Args) * 0.5f,
            };
            
            float score = this.CalculateScore(optometry, target.transform);
            return score > float.Epsilon;
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private void RunUpdate(float deltaTime)
        {
            float detectionSpeed = (float) this.m_DetectionSpeed.Get(this.Perception.Args);
            Optometry optometry = new Optometry
            {
                opticsPosition = this.OpticsPosition,
                opticsRotation = this.OpticsRotation,
                dimThreshold = (float) this.m_DimThreshold.Get(this.Perception.Args),
                litThreshold = (float) this.m_LitThreshold.Get(this.Perception.Args),
                primaryRadius = (float) this.m_PrimaryRadius.Get(this.Perception.Args),
                peripheralRadius = (float) this.m_PeripheralRadius.Get(this.Perception.Args),
                primaryAngleHalf = (float) this.m_PrimaryAngle.Get(this.Perception.Args) * 0.5f,
                peripheralAngleHalf = (float) this.m_PeripheralAngle.Get(this.Perception.Args) * 0.5f,
                primaryHeightHalf = (float) this.m_PrimaryHeight.Get(this.Perception.Args) * 0.5f,
                peripheralHeightHalf = (float) this.m_PeripheralHeight.Get(this.Perception.Args) * 0.5f,
            };
            
            #if UNITY_EDITOR
            this.m_DebugHits.Clear();
            #endif

            float maxRadius = optometry.primaryRadius + optometry.peripheralRadius; 
            SpatialHashEvidence.Find(
                optometry.opticsPosition, 
                maxRadius,
                this.m_EvidenceNearby
            );

            foreach (ISpatialHash evidenceNearby in this.m_EvidenceNearby)
            {
                Evidence evidence = evidenceNearby as Evidence;
                if (evidence == null) continue;
                
                if (!evidence.SensorSee) continue;
                
                string tag = evidence.GetTag(this.Perception.gameObject);
                if (evidence.IsTampered == this.Perception.GetEvidence(tag)) continue;
                
                float score = this.CalculateScore(optometry, evidence.transform);
                if (score <= float.Epsilon) continue;
                
                this.Perception.InvestigateEvidence(evidence);
            }
            
            foreach (Tracker tracker in this.Perception.TrackerList)
            {
                if (tracker.Target == null) continue;
                Transform target = tracker.Target.transform;
                
                float score = this.CalculateScore(optometry, target);
                if (score <= float.Epsilon) continue;
                
                float increment = score * detectionSpeed * deltaTime;
                this.Perception.AddAwareness(tracker.Target, increment);

                int instanceId = tracker.Target.GetInstanceID();
                this.m_PositionsLastSeen[instanceId] = target.position;

                this.EventSee?.Invoke(tracker.Target);
            }
            
            this.m_LastUpdateTime = this.Perception.Time;
        }

        private float CalculateScore(Optometry optometry, Transform target)
        {
            Vector3 localDirection = target.position - optometry.opticsPosition;
            Vector3 localPoint = Quaternion.Inverse(optometry.opticsRotation) * localDirection;
            
            if (localPoint == optometry.opticsPosition) return 0f;

            float scoreDistance = CalculateScore(
                localPoint.XZ().magnitude, 
                optometry.primaryRadius, 
                optometry.peripheralRadius
            );
            
            float scoreAngle = CalculateScore(
                Vector3.Angle(Vector3.forward, localPoint),
                optometry.primaryAngleHalf,
                optometry.peripheralAngleHalf
            );
            
            float scoreHeight = CalculateScore(
                Math.Abs(localPoint.y),
                optometry.primaryHeightHalf,
                optometry.peripheralHeightHalf
            );
            
            float score = MinScore(scoreDistance, scoreAngle, scoreHeight);
            
            Camouflage camouflage = target.Get<Camouflage>();
            if (camouflage != null)
            {
                score -= camouflage.GetSightDamp(this.Perception.gameObject);
            }
            
            if (score <= 0) return 0f;
            
            bool hasOcclusion = this.CalculateOcclusion(
                optometry.opticsPosition,
                target.position - optometry.opticsPosition,
                target,
                out float occlusion
            );

            if (hasOcclusion) score = Math.Max(score - occlusion, 0f);
            if (score <= 0f) return 0f;

            if (this.m_UseLuminance)
            {
                float luminance = LuminanceManager.Instance.LuminanceAt(target);
                if (luminance < optometry.dimThreshold) return 0f;
                if (luminance < optometry.litThreshold)
                {
                    score *= Mathf.InverseLerp(
                        optometry.dimThreshold,
                        optometry.litThreshold,
                        luminance
                    );
                }
            }

            return score;
        }
        
        private bool CalculateOcclusion(
            Vector3 position, 
            Vector3 direction,
            Transform target, 
            out float occlusion)
        {
            int numHits = Physics.RaycastNonAlloc(
                position,
                direction.normalized,
                HITS,
                Math.Max(direction.magnitude - RAY_OFFSET, 0f),
                this.m_LayerMask,
                QueryTriggerInteraction.Collide
            );

            occlusion = 0f;
            if (numHits == 0) return false;
            
            for (int i = 0; i < numHits; ++i)
            {
                Collider hit = HITS[i].collider;
                Obstruction hitObstruction = hit.Get<Obstruction>();
                
                if (hit.isTrigger && hitObstruction == null) continue;
                
                if (hit.transform.IsChildOf(target)) continue;
                if (hit.transform.IsChildOf(this.Perception.transform)) continue;

                #if UNITY_EDITOR
                this.m_DebugHits.Add(HITS[i].point);
                #endif
                
                if (hitObstruction == null)
                {
                    occlusion = 1f;
                    return true;
                }

                occlusion += hitObstruction.GetSightDamp(this.Perception.gameObject);
                if (occlusion >= 1f) break;
            }

            occlusion = Math.Min(occlusion, 1f);
            return true;
        }
        
        // PRIVATE STATIC METHODS: ----------------------------------------------------------------

        private static float CalculateScore(float value, float limit, float peripheral)
        {
            if (value > limit + peripheral) return -1f;
            if (value > limit) return peripheral > float.Epsilon 
                ? 1f - (value - limit) / peripheral
                : 0f;
            
            return 1f;
        }

        private static float MinScore(float a, float b, float c)
        {
            float min = Math.Min(a, b);
            return Math.Min(min, c);
        }

        // GIZMOS: --------------------------------------------------------------------------------

        protected override void OnDrawGizmos(Perception perception)
        {
            Animator animator = perception.GetComponentInChildren<Animator>();
            if (animator == null) return;

            GameObject optics = this.m_Optics.EditorValue;
            if (optics == null) optics = perception.gameObject;
            
            Vector3 opticsPosition = optics.transform.position + perception.transform.TransformDirection(this.m_OffsetPosition);
            Quaternion opticsRotation = optics.transform.rotation * Quaternion.Euler(this.m_OffsetRotation);
            
            // GameObject optics = this.m_Optics.EditorValue;
            // if (optics == null) return;
            
            // private Vector3 OpticsPosition
            // {
            //     get
            //     {
            //         Transform transform = this.m_Optics.Get<Transform>(this.Perception.Args);
            //         Vector3 offset = this.Perception.transform.TransformDirection(this.m_OffsetPosition);
            //     
            //         return transform != null
            //             ? transform.position + offset
            //             : this.Perception.transform.position;
            //     }
            // }
            //
            // private Quaternion OpticsRotation
            // {
            //     get
            //     {
            //         Transform transform = this.m_Optics.Get<Transform>(this.Perception.Args);
            //         return transform != null
            //             ? transform.rotation * Quaternion.Euler(this.m_OffsetRotation)
            //             : this.Perception.transform.rotation;
            //     }
            // }

            float primaryAngle = (float) this.m_PrimaryAngle.EditorValue;
            float primaryRadius = (float) this.m_PrimaryRadius.EditorValue;
            float primaryHeight = (float) this.m_PrimaryHeight.EditorValue;

            if (primaryAngle <= float.Epsilon) return;
            if (primaryRadius <= float.Epsilon) return;
            if (primaryHeight <= float.Epsilon) return;

            primaryAngle = Math.Min(primaryAngle, 360f);
            Quaternion primaryRotation = opticsRotation * Quaternion.Euler(0f, primaryAngle * -0.5f, 0f);

            Gizmos.color = GIZMOS_PRIMARY_COLOR;
            GizmosExtension.Vision(
                opticsPosition,
                primaryRotation, 
                primaryAngle,
                primaryRadius,
                primaryHeight
            );
            
            float peripheralAngle = primaryAngle + (float) this.m_PeripheralAngle.EditorValue;
            float peripheralRadius = primaryRadius + (float) this.m_PeripheralRadius.EditorValue;
            float peripheralHeight = primaryHeight + (float) this.m_PeripheralHeight.EditorValue;

            if (peripheralAngle <= float.Epsilon) return;
            if (peripheralRadius <= float.Epsilon) return;
            if (peripheralHeight <= float.Epsilon) return;
            
            peripheralAngle = Math.Min(peripheralAngle, 360f);
            Quaternion peripheralRotation = opticsRotation * Quaternion.Euler(0f, peripheralAngle * -0.5f, 0f);
            
            Gizmos.color = GIZMOS_PERIPHERAL_COLOR;
            GizmosExtension.Vision(
                opticsPosition,
                peripheralRotation, 
                peripheralAngle,
                peripheralRadius,
                peripheralHeight
            );

            if (Application.isPlaying && this.m_DebugHits != null)
            {
                Gizmos.color = GIZMOS_HIT_COLOR;
                foreach (Vector3 debugHit in this.m_DebugHits)
                {
                    Gizmos.DrawSphere(debugHit, GIZMOS_HIT_RADIUS);
                }
            }
        }
    }
}