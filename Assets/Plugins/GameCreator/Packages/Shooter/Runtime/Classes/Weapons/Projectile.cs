using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameCreator.Runtime.Shooter
{
    [Serializable]
    public class Projectile
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private Shot m_Shot = new Shot();
        
        [FormerlySerializedAs("m_Impact")]
        [SerializeField] private MaterialSoundsAsset m_ImpactSound;

        [SerializeField] private PropertyGetInstantiate m_ImpactEffect = new PropertyGetInstantiate(
            new GetGameObjectNone()
        );
        
        // INTERNAL METHODS: ----------------------------------------------------------------------

        internal bool Run(
            Args args,
            ShooterWeapon weapon,
            float chargeRatio,
            float pullTime
        )
        {
            return this.m_Shot.Value.Run(
                args,
                weapon,
                this.m_ImpactSound,
                this.m_ImpactEffect,
                chargeRatio,
                pullTime
            );
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public Vector3 GetTrajectory(
            List<Vector3> points,
            int maxResolution,
            float maxDistance,
            EnablerLayerMask useRaycast,
            WeaponData weaponData)
        {
            points.Clear();
            return this.m_Shot.GetTrajectory(
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
            return this.m_Shot.GetLaser(
                out pointSource,
                out pointTarget,
                maxDistance,
                useRaycast,
                weaponData
            );
        }
        
        // GIZMOS: --------------------------------------------------------------------------------
        
        public void StageGizmos(StagingGizmos stagingGizmos)
        { }
    }
}