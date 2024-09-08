using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Title("Feel")]
    [Category("Feel")]
    
    [Image(typeof(IconFeel), ColorTheme.Type.TextLight)]
    [Description("Allows characters to use a sixth-sense and detect entities close")]
    
    [Serializable]
    public class SensorFeel : TSensor
    {
        // CONSTANTS: -----------------------------------------------------------------------------
        
        private static readonly Color GIZMOS_COLOR = new Color(0, 0, 1, 0.25f);
        
        private const float RAY_OFFSET = 0.005f;
        private static readonly RaycastHit[] HITS = new RaycastHit[1];
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private UpdateMode m_Update = UpdateMode.EveryFrame;
        [SerializeField] private PropertyGetDecimal m_Interval = GetDecimalRandomRange.Create();
        [SerializeField] private PropertyGetDecimal m_DetectionSpeed = GetDecimalConstantOne.Create;

        [SerializeField] private PropertyGetDecimal m_Radius = GetDecimalConstantOne.Create;
        [SerializeField] private LayerMask m_LayerMask = Physics.DefaultRaycastLayers;
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private float m_LastUpdateTime = -999f;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => "Feel";
        
        // EVENTS: --------------------------------------------------------------------------------

        public event Action<GameObject> EventFeel; 

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

        public bool CanFeel(GameObject target)
        {
            if (target == null) return false;

            Vector3 direction = target.transform.position - this.Perception.transform.position;
            float distance = direction.magnitude;

            double radius = this.m_Radius.Get(this.Perception.Args);
            if (distance > radius) return false;
            
            int numHits = Physics.RaycastNonAlloc(
                this.Perception.transform.position,
                direction.normalized,
                HITS,
                Math.Max(distance - RAY_OFFSET, 0f),
                this.m_LayerMask,
                QueryTriggerInteraction.Ignore
            );
            
            return numHits == 0 ||
                   HITS[0].collider.transform.IsChildOf(target.transform) ||
                   HITS[0].collider.transform.IsChildOf(this.Perception.transform);
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private void RunUpdate(float deltaTime)
        {
            float detectionSpeed = (float) this.m_DetectionSpeed.Get(this.Perception.Args);
            
            foreach (Tracker tracker in this.Perception.TrackerList)
            {
                if (tracker.Target == null) continue;
                if (this.CanFeel(tracker.Target))
                {
                    float increment = detectionSpeed * deltaTime;
                    this.Perception.AddAwareness(tracker.Target, increment);
                    
                    this.EventFeel?.Invoke(tracker.Target);
                }
            }
            
            this.m_LastUpdateTime = this.Perception.Time;
        }

        // GIZMOS: --------------------------------------------------------------------------------

        protected override void OnDrawGizmos(Perception perception)
        {
            Gizmos.color = GIZMOS_COLOR;
            GizmosExtension.Octahedron(
                perception.transform.position,
                perception.transform.rotation,
                (float) this.m_Radius.EditorValue
            );
        }
    }
}