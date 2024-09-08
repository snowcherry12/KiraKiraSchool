using System;
using GameCreator.Runtime.Common;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GameCreator.Runtime.Stats
{
    [Serializable]
    public class StatsRepository : TRepository<StatsRepository>
    {
        // REPOSITORY PROPERTIES: -----------------------------------------------------------------
        
        public override string RepositoryID => "stats.general";

        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private StatusEffects m_StatusEffects = new StatusEffects();

        // PROPERTIES: ----------------------------------------------------------------------------

        public StatusEffects StatusEffects => this.m_StatusEffects;
        
        // EDITOR ENTER PLAYMODE: -----------------------------------------------------------------

        #if UNITY_EDITOR
        
        [InitializeOnEnterPlayMode]
        public static void InitializeOnEnterPlayMode() => Instance = null;
        
        #endif
    }
}