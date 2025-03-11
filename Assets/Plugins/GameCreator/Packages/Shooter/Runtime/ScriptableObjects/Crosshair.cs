using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Icon(EditorPaths.PACKAGES + "Shooter/Editor/Gizmos/GizmoCrosshair.png")]
    
    [CreateAssetMenu(
        fileName = "Crosshair", 
        menuName = "Game Creator/Shooter/Crosshair",
        order    = 50
    )]
    
    [Serializable]
    public class Crosshair : TSkin<GameObject>
    {
        private const HideFlags HIDE_FLAGS = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
        
        private const string ERR_NO_VALUE = "Prefab value cannot be empty";
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override string Description => string.Empty;

        public override string HasError => this.Value == null 
            ? ERR_NO_VALUE
            : string.Empty;

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public GameObject Create()
        {
            if (this.m_Value == null) return null;
            GameObject instance = Instantiate(this.m_Value);
            
            instance.hideFlags = HIDE_FLAGS;
            return instance;
        }
    }
}