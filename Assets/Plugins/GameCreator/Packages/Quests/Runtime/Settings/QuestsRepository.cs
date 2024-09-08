using System;
using GameCreator.Runtime.Common;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GameCreator.Runtime.Quests
{
    [Serializable]
    public class QuestsRepository : TRepository<QuestsRepository>
    {
        internal const string REPOSITORY_ID = "quests.general";
        
        // REPOSITORY PROPERTIES: -----------------------------------------------------------------
        
        public override string RepositoryID => REPOSITORY_ID;

        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private QuestsList m_Quests = new QuestsList();

        // PROPERTIES: ----------------------------------------------------------------------------

        public QuestsList Quests => this.m_Quests;

        // EDITOR ENTER PLAYMODE: -----------------------------------------------------------------

        #if UNITY_EDITOR
        
        [InitializeOnEnterPlayMode]
        public static void InitializeOnEnterPlayMode() => Instance = null;
        
        #endif
    }
}