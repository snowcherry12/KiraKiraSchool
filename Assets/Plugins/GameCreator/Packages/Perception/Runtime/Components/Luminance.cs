using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Icon(RuntimePaths.PACKAGES + "Perception/Editor/Gizmos/GizmoLuminance.png")]
    [AddComponentMenu("Game Creator/Perception/Luminance")]

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Light))]
    
    [Serializable]
    public class Luminance : MonoBehaviour
    {
        private static readonly RaycastHit[] HITS = new RaycastHit[32];
        
        private const float RAY_OFFSET = 0.005f;
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetDecimal m_Multiplier = GetDecimalConstantOne.Create;
        [SerializeField] private LayerMask m_LayerMask = Physics.DefaultRaycastLayers; 
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        private Light Light { get; set; }
        private Args Args { get; set; }
        
        // INITIALIZE METHODS: --------------------------------------------------------------------

        private void Awake()
        {
            this.Light = this.Get<Light>();
            this.Args = new Args(this.gameObject);
        }

        private void OnEnable()
        {
            LuminanceManager.Instance.Insert(this);
        }

        private void OnDisable()
        {
            if (ApplicationManager.IsExiting) return;
            LuminanceManager.Instance.Remove(this);
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public float IntensityTo(Transform target)
        {
            if (target == null) return 0f;
            if (this.Light == null || !this.Light.isActiveAndEnabled) return 0f;

            if (this.Args.Target != target.gameObject)
            {
                this.Args.ChangeTarget(target.gameObject);
            }

            float intensity = this.Light.type switch
            {
                LightType.Spot => this.GetSpotIntensityTo(target),
                LightType.Directional => this.GetDirectionalIntensityTo(target),
                LightType.Point => this.GetPointIntensityTo(target),
                LightType.Area => this.GetAreaIntensityTo(target),
                LightType.Disc => this.GetDiscIntensityTo(target),
                _ => throw new ArgumentOutOfRangeException()
            };

            return intensity * (float) this.m_Multiplier.Get(this.Args);
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private float GetSpotIntensityTo(Transform target)
        {
            Vector3 direction = target.position - this.transform.position;
            float angle = Vector3.Angle(direction, this.transform.forward);

            float spotOuterAngle = this.Light.spotAngle * 0.5f;
            float spotInnerAngle = this.Light.innerSpotAngle * 0.5f;
            
            if (angle > spotOuterAngle || spotOuterAngle <= float.Epsilon) return 0f;
            float angleRatio = 1f - (angle > spotInnerAngle
                ? (angle - spotInnerAngle) / spotOuterAngle
                : 0f);
            
            Vector3 projection = TransformUtils
                .InverseTransformPoint(
                    target.position,
                    this.transform.position,
                    this.transform.rotation,
                    Vector3.one
                )
                .PointOnVector(
                    Vector3.forward
                );
            
            float projectionMagnitude = projection.magnitude;
            if (projectionMagnitude > this.Light.range) return 0f;
            
            float distanceRatio = 1f - projectionMagnitude / this.Light.range;
            float intensity = Mathf.Sqrt(this.Light.intensity * distanceRatio * angleRatio);
            
            int numHits = Physics.RaycastNonAlloc(
                this.transform.position,
                direction, 
                HITS,
                Math.Max(direction.magnitude - RAY_OFFSET, 0f),
                this.m_LayerMask,
                QueryTriggerInteraction.Ignore
            );
            
            for (int i = 0; i < numHits; ++i)
            {
                Collider hit = HITS[i].collider;
                if (hit.transform.IsChildOf(target)) continue;
                
                Obstruction obstruction = hit.Get<Obstruction>();
                if (obstruction == null) return 0f;
                
                intensity -= obstruction.GetSightDamp(target.gameObject);
            }
            
            return intensity;
        }
        
        private float GetDirectionalIntensityTo(Transform target)
        {
            float intensity = this.Light.intensity;
            Vector3 inverseDirection = -this.transform.forward;
            
            int numHits = Physics.RaycastNonAlloc(
                target.position,
                inverseDirection, 
                HITS,
                Mathf.Infinity, 
                this.m_LayerMask,
                QueryTriggerInteraction.Ignore
            );

            for (int i = 0; i < numHits; ++i)
            {
                Obstruction obstruction = HITS[i].collider.Get<Obstruction>();
                if (obstruction == null) return 0f;

                intensity -= obstruction.GetSightDamp(target.gameObject);
            }

            return intensity;
        }
        
        private float GetPointIntensityTo(Transform target)
        {
            Vector3 direction = target.position - this.transform.position;
            float distance = direction.magnitude;
            
            if (distance >= this.Light.range) return 0f;
            
            float distanceRatio = 1f - distance / this.Light.range;
            float intensity = Mathf.Sqrt(this.Light.intensity * distanceRatio);
            
            int numHits = Physics.RaycastNonAlloc(
                this.transform.position,
                direction, 
                HITS,
                Math.Max(distance - RAY_OFFSET, 0f),
                this.m_LayerMask,
                QueryTriggerInteraction.Ignore
            );
            
            for (int i = 0; i < numHits; ++i)
            {
                Collider hit = HITS[i].collider;
                if (hit.transform.IsChildOf(target)) continue;
                
                Obstruction obstruction = hit.Get<Obstruction>();
                if (obstruction == null) return 0f;
                
                intensity -= obstruction.GetSightDamp(target.gameObject);
            }
            
            return intensity;
        }
        
        private float GetAreaIntensityTo(Transform target)
        {
            throw new Exception($"Area lights are not {nameof(Luminance)} types supported");
        }
        
        private float GetDiscIntensityTo(Transform target)
        {
            throw new Exception($"Disc lights are not {nameof(Luminance)} types supported");
        }
    }
}