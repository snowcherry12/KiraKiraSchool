using System;
using GameCreator.Runtime.Common;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GameCreator.Runtime.Dialogue
{
    [Serializable]
    public class DialogueRepository : TRepository<DialogueRepository>
    {
        internal const string REPOSITORY_ID = "dialogue.general";
        
        // REPOSITORY PROPERTIES: -----------------------------------------------------------------
        
        public override string RepositoryID => REPOSITORY_ID;

        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private Values m_Values = new Values();

        // PROPERTIES: ----------------------------------------------------------------------------

        public Values Values => this.m_Values;
        
        // EDITOR ENTER PLAYMODE: -----------------------------------------------------------------

        #if UNITY_EDITOR
        
        [InitializeOnEnterPlayMode]
        public static void InitializeOnEnterPlayMode() => Instance = null;
        
        #endif
    }
}