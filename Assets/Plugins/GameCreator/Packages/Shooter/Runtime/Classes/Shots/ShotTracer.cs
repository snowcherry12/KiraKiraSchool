using System;
using System.Collections.Generic;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Title("Tracer Projectile")]
    [Image(typeof(IconAimTarget), ColorTheme.Type.Blue)]
    
    [Category("Tracer Projectile")]
    [Description("Instantiates a game object that moves along a path between the muzzle and the target")]
    
    [Serializable]
    public class ShotTracer : TShotProjectile
    {
        private static readonly List<Vector3> TRAJECTORY_POINTS = new List<Vector3>(128);
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField]
        private PropertyGetDecimal m_Speed = GetDecimalDecimal.Create(10f);
        
        [SerializeField]
        private PropertyGetGameObject m_Target = GetGameObjectCharacterTarget.Create();

        [SerializeField]
        private PropertyGetDecimal m_DeviationX = GetDecimalConstantOne.Create;
        
        [SerializeField]
        private PropertyGetDecimal m_DeviationY = GetDecimalConstantOne.Create;
        
        [SerializeField] private LayerMask m_LayerMask = Physics.DefaultRaycastLayers;
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override void OnRun(WeaponData weaponData, ShotData shotData)
        {
            SightItem sight = weaponData.Weapon.Sights.Get(weaponData.SightId); 
            MuzzleData muzzle = sight.Sight.GetMuzzle(weaponData.WeaponArgs, weaponData.Weapon);

            GameObject target = this.m_Target.Get(weaponData.WeaponArgs);
            Vector3 deviation = weaponData.Character.transform.TransformDirection(new Vector3(
                (float) this.m_DeviationX.Get(weaponData.WeaponArgs),
                (float) this.m_DeviationY.Get(weaponData.WeaponArgs),
                0f
            ));
            
            Bullet bullet = shotData.Projectile.Require<Bullet>();
            bullet.SetFromTracer(
                shotData,
                (float) this.m_Speed.Get(weaponData.WeaponArgs),
                target,
                deviation,
                this.m_LayerMask
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

            GameObject target = this.m_Target.Get(weaponData.WeaponArgs);
            if (target == null) return Vector3.zero;

            Vector3 deviation = weaponData.Character.transform.TransformDirection(new Vector3(
                (float) this.m_DeviationX.Get(weaponData.WeaponArgs),
                (float) this.m_DeviationY.Get(weaponData.WeaponArgs),
                0f
            ));
            
            TRAJECTORY_POINTS.Clear();
            
            for (int i = 0; i < maxResolution; ++i)
            {
                Vector3 point = Bezier.Get(
                    muzzle.Position,
                    target.transform.position,
                    deviation,
                    Vector3.zero, 
                    i / (float) maxResolution
                );
                
                TRAJECTORY_POINTS.Add(point);
            }
            
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