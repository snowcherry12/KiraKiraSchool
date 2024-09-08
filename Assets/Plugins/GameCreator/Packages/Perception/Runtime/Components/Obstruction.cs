using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Icon(RuntimePaths.PACKAGES + "Perception/Editor/Gizmos/GizmoObstruction.png")]
    [AddComponentMenu("Game Creator/Perception/Obstruction")]
    
    [Serializable]
    public class Obstruction : MonoBehaviour
    {
        private static readonly RaycastHit[] HITS = new RaycastHit[32];
        
        private const float RAY_OFFSET = 0.005f;
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetDecimal m_SightDamping = GetDecimalConstantOne.Create;
        [SerializeField] private PropertyGetDecimal m_NoiseDamping = GetDecimalConstantOne.Create;
        [SerializeField] private PropertyGetDecimal m_ScentDamping = GetDecimalConstantOne.Create;
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private Args m_Args;
        
        // INITIALIZERS: --------------------------------------------------------------------------
        
        private void OnEnable()
        {
            this.m_Args = new Args(this.gameObject);
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public float GetSightDamp(GameObject target)
        {
            if (!this.isActiveAndEnabled) return 0f;
            
            this.m_Args.ChangeTarget(target);
            return (float) this.m_SightDamping.Get(this.m_Args);
        }
        
        public float GetNoiseDamp(GameObject target)
        {
            if (!this.isActiveAndEnabled) return 0f;
            
            this.m_Args.ChangeTarget(target);
            return (float) this.m_NoiseDamping.Get(this.m_Args);
        }
        
        public float GetScentDamp(GameObject target)
        {
            if (!this.isActiveAndEnabled) return 0f;
            
            this.m_Args.ChangeTarget(target);
            return (float) this.m_ScentDamping.Get(this.m_Args);
        }
        
        // PUBLIC STATIC METHODS: -----------------------------------------------------------------
        
        public static float GetNoiseDamp(Vector3 position, GameObject receiver, LayerMask layerMask)
        {
            float level = 0f;
            Vector3 direction = receiver.transform.position - position;
            
            int numHits = Physics.RaycastNonAlloc(
                position, 
                direction.normalized,
                HITS,
                Math.Max(direction.magnitude - RAY_OFFSET, 0f),
                layerMask,
                QueryTriggerInteraction.Collide
            );
            
            for (int i = 0; i < numHits; ++i)
            {
                Obstruction obstruction = HITS[i].collider.Get<Obstruction>();
                
                if (obstruction == null) continue;
                level += obstruction.GetNoiseDamp(receiver);
            }
            
            return level;
        }
        
        public static float GetScentDamp(Vector3 position, GameObject receiver, LayerMask layerMask)
        {
            float level = 0f;
            Vector3 direction = receiver.transform.position - position;
            
            int numHits = Physics.RaycastNonAlloc(
                position, 
                direction.normalized,
                HITS,
                Math.Max(direction.magnitude - RAY_OFFSET, 0f),
                layerMask,
                QueryTriggerInteraction.Collide
            );
            
            for (int i = 0; i < numHits; ++i)
            {
                Obstruction obstruction = HITS[i].collider.Get<Obstruction>();
                
                if (obstruction == null) continue;
                level += obstruction.GetScentDamp(receiver);
            }
            
            return level;
        }
    }
}