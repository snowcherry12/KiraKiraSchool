using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Title("Kinematic Projectile")]
    [Image(typeof(IconCubeSolid), ColorTheme.Type.Blue)]
    
    [Category("Kinematic Projectile")]
    [Description("Instantiates a game object and manually moves it with a kinematic translation")]
    
    [Serializable]
    public class ShotKinematic : TShotProjectile
    {
        private static readonly List<Vector3> TRAJECTORY_POINTS = new List<Vector3>(128);
        
        private enum HitMode
        {
            /// <summary>
            /// Hits the target(s) upon impacting
            /// </summary>
            OnImpact,
            
            /// <summary>
            /// Hits the target(s) upon the timeout ending
            /// </summary>
            OnTimeout
        }
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private PropertyGetDecimal m_Force = GetDecimalDecimal.Create(50f);
        [SerializeField] private PropertyGetDecimal m_Gravity = GetDecimalPhysicsGravityEarth.Create;
        [SerializeField] private PropertyGetDecimal m_AirResistance = GetDecimalConstantZero.Create;
        [SerializeField] private PropertyGetDecimal m_WindInfluence = GetDecimalConstantOne.Create;

        [SerializeField] private PropertyGetDecimal m_AttractionForce = GetDecimalConstantZero.Create;
        [SerializeField] private PropertyGetGameObject m_AttractionTarget = GetGameObjectNone.Create();
        
        [SerializeField] private LayerMask m_LayerMask = Physics.DefaultRaycastLayers;
        [SerializeField] private PropertyGetDecimal m_MaxDistance = GetDecimalDecimal.Create(100f);
        
        [SerializeField] private HitMode m_Hit = HitMode.OnImpact;
        
        [SerializeField]
        private PropertyGetInteger m_Pierces = new PropertyGetInteger(
            new GetDecimalConstantZero()
        );
        
        [SerializeField] private PropertyGetDecimal m_Timeout = new PropertyGetDecimal(5f);
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override void OnRun(WeaponData weaponData, ShotData shotData)
        {
            Bullet bullet = shotData.Projectile.Require<Bullet>();
            
            bullet.SetFromKinematic(
                shotData,
                (float) this.m_Force.Get(weaponData.WeaponArgs),
                (float) this.m_Gravity.Get(weaponData.WeaponArgs),
                (float) this.m_AirResistance.Get(weaponData.WeaponArgs),
                (float) this.m_WindInfluence.Get(weaponData.WeaponArgs),
                (float) this.m_AttractionForce.Get(weaponData.WeaponArgs),
                this.m_AttractionTarget.Get(weaponData.WeaponArgs),
                this.m_LayerMask,
                (float) this.m_MaxDistance.Get(weaponData.WeaponArgs),
                this.m_Hit == HitMode.OnTimeout,
                (int) this.m_Pierces.Get(weaponData.WeaponArgs),
                (float) this.m_Timeout.Get(weaponData.WeaponArgs)
            );
        }
        
        // TRAJECTORY METHOD: ---------------------------------------------------------------------

        public override Vector3 GetTrajectory(
            List<Vector3> points,
            int maxResolution,
            float maxDistance,
            EnablerLayerMask useRaycast,
            WeaponData weaponData)
        {
            SightItem sight = weaponData.Weapon.Sights.Get(weaponData.SightId); 
            MuzzleData muzzle = sight.Sight.GetMuzzle(weaponData.WeaponArgs, weaponData.Weapon);
            
            maxDistance = Mathf.Min(maxDistance, (float) this.m_MaxDistance.Get(weaponData.WeaponArgs));
            
            float force = (float) this.m_Force.Get(weaponData.WeaponArgs);
            Vector3 velocity = muzzle.Direction.normalized * force;
            
            points.Add(muzzle.Position);
            
            Bullet.TracePositions(
                TRAJECTORY_POINTS,
                muzzle.Position,
                velocity,
                maxDistance,
                maxDistance / maxResolution,
                (float) this.m_AirResistance.Get(weaponData.WeaponArgs),
                (float) this.m_WindInfluence.Get(weaponData.WeaponArgs),
                (float) this.m_Gravity.Get(weaponData.WeaponArgs),
                Bullet.KINEMATIC_BULLET_MASS
            );

            if (useRaycast.IsEnabled)
            {
                for (int i = 1; i < TRAJECTORY_POINTS.Count; ++i)
                {
                    Vector3 prevPoint = TRAJECTORY_POINTS[i - 1];
                    Vector3 nextPoint = TRAJECTORY_POINTS[i - 0];
                    
                    Vector3 direction = nextPoint - prevPoint;
                    
                    bool isHit = Physics.Raycast(
                        prevPoint,
                        direction.normalized,
                        out RaycastHit hit,
                        direction.magnitude,
                        useRaycast.Value,
                        QueryTriggerInteraction.Ignore
                    );
                    
                    if (isHit)
                    {
                        points.Add(hit.point);
                        return hit.normal;
                    }
                    
                    points.Add(nextPoint);
                }

                return Vector3.zero;
            }
            
            points.AddRange(TRAJECTORY_POINTS);
            return Vector3.zero;
        }
    }
}