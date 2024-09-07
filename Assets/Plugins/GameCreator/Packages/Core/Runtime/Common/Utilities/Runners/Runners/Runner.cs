using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Serializable]
    public abstract class Runner : MonoBehaviour
    {
        #if UNITY_EDITOR
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void OnEnterPlayMode()
        {
            Pool.Clear();
        }
        
        #endif

        public const string KEY_RUNNER_SHOW_HIERARCHY = "gc:runner-show-hierarchy";
        
        protected static Dictionary<int, RunnerPool> Pool = new Dictionary<int, RunnerPool>();

        protected const HideFlags TEMPLATE_FLAGS = HideFlags.None;
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        [field: NonSerialized] public GameObject Template { get; set; }
        
        /* Implement exposed member: m_Value */
    }
}