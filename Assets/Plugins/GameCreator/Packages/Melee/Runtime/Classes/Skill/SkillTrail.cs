using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Serializable]
    public class SkillTrail
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private bool m_IsActive = true;
        
        [SerializeField] private EnablerFloat m_Length = new EnablerFloat(false, Trail.DEFAULT_LENGTH);
        [SerializeField] private EnablerInt m_Quads = new EnablerInt(false, Trail.DEFAULT_QUADS);
        [SerializeField] private EnablerMaterial m_Material = new EnablerMaterial(false, null);

        // PROPERTIES: ----------------------------------------------------------------------------
        
        public bool IsActive => this.m_IsActive;
        
        public bool HasQuads => this.m_Quads.IsEnabled;
        public int Quads => this.m_Quads.Value;
        
        public bool HasLength => this.m_Length.IsEnabled;
        public float Length => this.m_Length.Value;
        
        public bool HasMaterial => this.m_Material.IsEnabled;
        public Material Material => this.m_Material.Value;
    }
}