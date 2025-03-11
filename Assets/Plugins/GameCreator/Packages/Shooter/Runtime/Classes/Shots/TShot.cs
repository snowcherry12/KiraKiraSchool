using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Title("Shot")]
    
    [Serializable]
    public abstract class TShot : IShot, IComparer<RaycastHit>
    {
        public abstract bool Run(Args args,
            ShooterWeapon weapon,
            MaterialSoundsAsset impactSound,
            PropertyGetInstantiate impactEffect,
            float chargeRatio,
            float pullTime);
        
        public abstract Vector3 GetTrajectory(
            List<Vector3> points,
            int maxResolution,
            float maxDistance,
            EnablerLayerMask useRaycast,
            WeaponData weaponData
        );
        
        public Vector3 GetLaser(
            out Vector3 pointSource,
            out Vector3 pointTarget,
            float maxDistance,
            EnablerLayerMask useRaycast,
            WeaponData weaponData)
        {
            SightItem sight = weaponData.Weapon.Sights.Get(weaponData.SightId); 
            MuzzleData muzzle = sight.Sight.GetMuzzle(weaponData.WeaponArgs, weaponData.Weapon);

            Vector3 offset = weaponData.Prop != null
                ? weaponData.Prop.transform.TransformDirection(sight.Sight.Laser.Offset)
                : Vector3.zero;
            
            pointSource = muzzle.Position + offset;
            
            if (useRaycast.IsEnabled)
            {
                bool isHit = Physics.Raycast(
                    muzzle.Position,
                    muzzle.Direction,
                    out RaycastHit hit,
                    maxDistance,
                    useRaycast.Value,
                    QueryTriggerInteraction.Ignore
                );
                    
                if (isHit)
                {
                    pointTarget = hit.point;
                    return hit.normal;
                }
            }
            
            pointTarget = muzzle.Position + muzzle.Direction.normalized * maxDistance;
            return Vector3.zero;
        }
        
        // COMPARER INTERFACE: --------------------------------------------------------------------
        
        int IComparer<RaycastHit>.Compare(RaycastHit x, RaycastHit y)
        {
            return x.distance.CompareTo(y.distance);
        }
    }
}