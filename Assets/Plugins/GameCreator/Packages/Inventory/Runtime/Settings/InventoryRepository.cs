using System;
using GameCreator.Runtime.Common;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class InventoryRepository : TRepository<InventoryRepository>
    {
        internal const string REPOSITORY_ID = "inventory.general";
        
        // REPOSITORY PROPERTIES: -----------------------------------------------------------------
        
        public override string RepositoryID => REPOSITORY_ID;

        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private Catalogue m_Items = new Catalogue();

        // PROPERTIES: ----------------------------------------------------------------------------

        public Catalogue Items => this.m_Items;
        
        // EDITOR ENTER PLAYMODE: -----------------------------------------------------------------

        #if UNITY_EDITOR
        
        [InitializeOnEnterPlayMode]
        public static void InitializeOnEnterPlayMode() => Instance = null;
        
        #endif
    }
}