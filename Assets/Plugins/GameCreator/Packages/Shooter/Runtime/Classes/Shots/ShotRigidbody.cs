using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Title("Rigidbody Projectile")]
    [Image(typeof(IconPhysics), ColorTheme.Type.Green)]
    
    [Category("Rigidbody Projectile")]
    [Description("Instantiates a game object with a Rigidbody component and applies force to it")]
    
    [Serializable]
    public class ShotRigidbody : TShotProjectile
    {
        private enum ForceModeStart
        {
            /// <summary>
            /// Add an instant force impulse to the rigidbody, using its mass.
            /// </summary>
            UseMass = ForceMode.Impulse,
            
            /// <summary>
            /// Add an instant velocity change to the rigidbody, ignoring its mass.
            /// </summary>
            IgnoreMass = ForceMode.VelocityChange
        }
        
        private enum ForceModeAttraction
        {
            /// <summary>
            /// Add a continuous force to the rigidbody, using its mass.
            /// </summary>
            UseMass = ForceMode.Force,
            
            /// <summary>
            /// Add a continuous acceleration to the rigidbody, ignoring its mass.
            /// </summary>
            IgnoreMass = ForceMode.Acceleration
        }
        
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
        
        private static readonly List<Vector3> TRAJECTORY_POINTS = new List<Vector3>(128);
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private ForceModeStart m_Impulse = ForceModeStart.IgnoreMass;
        [SerializeField] private PropertyGetDecimal m_ImpulseForce = GetDecimalDecimal.Create(50f);

        [SerializeField] private PropertyGetDecimal m_Mass = GetDecimalConstantOne.Create;
        [SerializeField] private PropertyGetDecimal m_AirResistance = GetDecimalConstantZero.Create;
        [SerializeField] private PropertyGetDecimal m_WindInfluence = GetDecimalConstantOne.Create;
        
        [SerializeField] private ForceModeAttraction m_Attraction = ForceModeAttraction.IgnoreMass;
        [SerializeField] private PropertyGetDecimal m_AttractionForce = GetDecimalConstantZero.Create;
        [SerializeField] private PropertyGetGameObject m_AttractionTarget = GetGameObjectNone.Create();
        [SerializeField] private PropertyGetDecimal m_MaxDistance = GetDecimalDecimal.Create(100f);

        [SerializeField] private HitMode m_Hit = HitMode.OnImpact;
        [SerializeField] private PropertyGetDecimal m_Timeout = new PropertyGetDecimal(5f);
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override void OnRun(WeaponData weaponData, ShotData shotData)
        {
            Bullet bullet = shotData.Projectile.Require<Bullet>();
            
            bullet.SetFromRigidbody(
                shotData,
                (ForceMode) this.m_Impulse,
                (float) this.m_ImpulseForce.Get(weaponData.WeaponArgs),
                (float) this.m_Mass.Get(weaponData.WeaponArgs),
                (float) this.m_AirResistance.Get(weaponData.WeaponArgs),
                (float) this.m_WindInfluence.Get(weaponData.WeaponArgs),
                (ForceMode) this.m_Attraction,
                (float) this.m_AttractionForce.Get(weaponData.WeaponArgs),
                this.m_AttractionTarget.Get(weaponData.WeaponArgs),
                (float) this.m_MaxDistance.Get(weaponData.WeaponArgs),
                this.m_Hit == HitMode.OnTimeout,
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
            
            float force = (float) this.m_ImpulseForce.Get(weaponData.WeaponArgs);
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
                Physics.gravity.magnitude,
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