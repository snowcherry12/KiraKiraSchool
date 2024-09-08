using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Icon(RuntimePaths.PACKAGES + "Perception/Editor/Gizmos/GizmoCamouflage.png")]
    [AddComponentMenu("Game Creator/Perception/Camouflage")]
    
    [Serializable]
    public class Camouflage : MonoBehaviour
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetDecimal m_SightDamping = GetDecimalConstantPointFive.Create;
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private Args m_Args;
        
        // INITIALIZERS: --------------------------------------------------------------------------

        private void Awake()
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
    }
}