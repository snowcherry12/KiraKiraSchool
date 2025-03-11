using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Serializable]
    public class SightList : TPolymorphicList<SightItem>
    {
        private static readonly Color COLOR_GIZMOS = new Color(1f, 0f, 0f, 1f);
        private const float RADIUS_GIZMOS = 0.01f;
        private const float WIDTH_GIZMOS = 0.1f;
        private const float HEIGHT_GIZMOS = 0.1f;
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeReference] private SightItem[] m_Sights = 
        {
            new SightItem()
        };
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized]
        private Dictionary<IdString, SightItem> m_SightsMap;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override int Length => this.m_Sights.Length;

        public IdString DefaultId => this.m_Sights.Length != 0
            ? this.m_Sights[0].Id
            : IdString.EMPTY;

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public bool Contains(IdString sightId)
        {
            #if UNITY_EDITOR
            
            foreach (SightItem sight in this.m_Sights)
            {
                if (sight.Id != sightId) continue;
                return true;
            }
            
            return false;
            
            #else
            
            this.RequireSightMap();
            return this.m_SightsMap.ContainsKey(sightId);

            #endif
        }

        public SightItem Get(IdString sightId)
        {
            #if UNITY_EDITOR
            
            foreach (SightItem sight in this.m_Sights)
            {
                if (sight.Id != sightId) continue;
                return sight;
            }
            
            return null;
            
            #else

            this.RequireSightMap();
            return this.m_SightsMap.GetValueOrDefault(sightId);

            #endif
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void RequireSightMap()
        {
            if (this.m_SightsMap != null) return;
            
            this.m_SightsMap = new Dictionary<IdString, SightItem>(this.m_Sights.Length);
            foreach (SightItem Sight in this.m_Sights)
            {
                this.m_SightsMap[Sight.Id] = Sight;
            }
        }
        
        // GIZMOS: --------------------------------------------------------------------------------
        
        public void StageGizmos(StagingGizmos stagingGizmos)
        {
            Gizmos.color = COLOR_GIZMOS;
            Matrix4x4 restoreMatrix = Gizmos.matrix;

            foreach (SightItem sightItem in this.m_Sights)
            {
                if (sightItem.ScopeThrough == false) continue;
                
                Matrix4x4 rotationMatrix = Matrix4x4.TRS(
                    stagingGizmos.transform.TransformPoint(sightItem.ScopePosition),
                    stagingGizmos.transform.rotation * sightItem.ScopeRotation,
                    Vector3.one
                );
                
                Gizmos.matrix = rotationMatrix;
                GizmosExtension.Octahedron(Vector3.zero, Quaternion.identity, RADIUS_GIZMOS, 4);
                Gizmos.DrawLine(Vector3.zero, Vector3.back * sightItem.ScopeDistance);
                
                Gizmos.DrawWireCube(Vector3.back * sightItem.ScopeDistance, new Vector3(WIDTH_GIZMOS, HEIGHT_GIZMOS, 0f));
            }

            Gizmos.matrix = restoreMatrix;
        }
    }
}