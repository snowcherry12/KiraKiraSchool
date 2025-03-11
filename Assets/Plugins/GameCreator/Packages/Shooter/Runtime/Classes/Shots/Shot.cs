using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Serializable]
    public class Shot
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeReference] private TShot m_Type = new ShotRaycast();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public TShot Value => this.m_Type;

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public Vector3 GetTrajectory(
            List<Vector3> points,
            int maxResolution,
            float maxDistance,
            EnablerLayerMask useRaycast,
            WeaponData weaponData)
        {
            return this.m_Type.GetTrajectory(
                points,
                maxResolution,
                maxDistance,
                useRaycast,
                weaponData
            );
        }
        
        public Vector3 GetLaser(
            out Vector3 pointSource,
            out Vector3 pointTarget,
            float maxDistance,
            EnablerLayerMask useRaycast,
            WeaponData weaponData)
        {
            return this.m_Type.GetLaser(
                out pointSource,
                out pointTarget,
                maxDistance,
                useRaycast,
                weaponData
            );
        }
    }
}