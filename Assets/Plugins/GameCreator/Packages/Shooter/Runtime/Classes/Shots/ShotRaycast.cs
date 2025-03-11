using System;
using System.Collections.Generic;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Title("Raycast")]
    [Image(typeof(IconBullsEye), ColorTheme.Type.Yellow)]
    
    [Category("Raycast")]
    [Description("Casts a ray using the physics engine and reports the hit immediately")]
    
    [Serializable]
    public class ShotRaycast : TShot
    {
        private static readonly PropertyName COLLECTION_LINE_RENDERER = "ShotLineRenderer";
        
        private static readonly RaycastHit[] HITS = new RaycastHit[128];
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private LayerMask m_LayerMask = Physics.DefaultRaycastLayers;
        [SerializeField] private PropertyGetDecimal m_MaxDistance = GetDecimalDecimal.Create(20f);

        [SerializeField]
        private PropertyGetInteger m_Pierces = new PropertyGetInteger(
            new GetDecimalConstantZero()
        );

        [SerializeField] private bool m_UseLineRenderer = true;
        [SerializeField] private PropertyGetDecimal m_Duration = GetDecimalConstantPointFive.Create;
        [SerializeField] private PropertyGetMaterial m_LineMaterial = GetMaterialInstance.Create();
        [SerializeField] private PropertyGetColor m_Color = GetColorColorsWhite.Create;
        
        [SerializeField] private PropertyGetDecimal m_Width = GetDecimalConstantPointOne.Create;
        [SerializeField] private LineTextureMode m_TextureMode = LineTextureMode.Stretch;
        [SerializeField] private LineAlignment m_TextureAlign = LineAlignment.View;
        
        // RUN METHOD: ----------------------------------------------------------------------------
        
        public override bool Run(Args args,
            ShooterWeapon weapon,
            MaterialSoundsAsset impactSound,
            PropertyGetInstantiate impactEffect,
            float chargeRatio,
            float pullTime)
        {
            Character character = args.Self.Get<Character>();
            
            WeaponData weaponData = character.Combat.RequestStance<ShooterStance>().Get(weapon);
            weaponData.CombatArgs.ChangeTarget(null);
            
            SightItem sight = weapon.Sights.Get(weaponData.SightId);
            if (sight?.Sight == null) return false;
            
            MuzzleData muzzle = sight.Sight.GetMuzzle(
                weaponData.WeaponArgs,
                weapon
            );

            int projectilesUsed = weapon.Fire.ProjectilesPerShot(weaponData.WeaponArgs);
            int cartridgesUsed = weapon.Fire.CartridgesPerShot(weaponData.WeaponArgs);

            for (int i = 0; i < projectilesUsed; ++i)
            {
                Vector3 spreadDirection = sight.Sight.GetSpreadDirection(
                    weaponData.WeaponArgs, 
                    weapon
                );
                
                ShotData data = new ShotData(
                    character,
                    weapon,
                    weaponData.SightId,
                    character.Combat.GetProp(weapon),
                    muzzle.Position,
                    spreadDirection,
                    impactSound,
                    impactEffect,
                    i == 0 ? cartridgesUsed : 0,
                    chargeRatio,
                    0f,
                    pullTime
                );

                if (weapon.CanShoot(data, weaponData.WeaponArgs) == false) return false;
                weapon.OnShoot(data, weaponData.WeaponArgs);

                float maxDistance = (float) this.m_MaxDistance.Get(weaponData.WeaponArgs);
                int pierces = (int) this.m_Pierces.Get(weaponData.WeaponArgs);
                int numHits = Physics.RaycastNonAlloc(
                    muzzle.Position,
                    spreadDirection,
                    HITS,
                    maxDistance,
                    this.m_LayerMask,
                    QueryTriggerInteraction.Ignore
                );

                Array.Sort(HITS, 0, numHits, this);
                int numIterations = Math.Min(pierces + 1, numHits);

                for (int iteration = 0; iteration < numIterations; ++iteration)
                {
                    RaycastHit hit = HITS[iteration];
                    
                    data.UpdateHit(hit.collider.gameObject, hit.point, hit.distance, numIterations);
                    weaponData.CombatArgs.ChangeTarget(hit.collider.gameObject);
                    
                    if (weapon.CanHit(data, weaponData.CombatArgs))
                    {
                        weapon.OnHit(data, weaponData.CombatArgs);
                        MaterialSounds.Play(
                            weaponData.CombatArgs,
                            hit.point,
                            hit.normal,
                            hit.collider.gameObject,
                            impactSound,
                            UnityEngine.Random.Range(-180f, 180f)
                        );
                        
                        impactEffect?.Get(
                            weaponData.CombatArgs,
                            hit.point,
                            Quaternion.LookRotation(hit.normal)
                        );
                    }
                }

                if (this.m_UseLineRenderer)
                {
                    float duration = (float) this.m_Duration.Get(args);

                    GameObject instance = PoolManager.Instance.Pick(
                        COLLECTION_LINE_RENDERER.GetHashCode(),
                        muzzle.Position,
                        Quaternion.identity,
                        3,
                        duration
                    );

                    Tracer shotLineRenderer = instance.Require<Tracer>();
                    shotLineRenderer.OnShoot(
                        muzzle.Position,
                        numIterations != 0
                            ? HITS[numIterations - 1].point
                            : muzzle.Position + spreadDirection.normalized * maxDistance,
                        duration,
                        maxDistance,
                        this.m_LineMaterial.Get(args),
                        this.m_Color.Get(args),
                        (float) this.m_Width.Get(args),
                        this.m_TextureMode,
                        this.m_TextureAlign
                    );
                }
            }

            return true;
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

            points.Add(muzzle.Position);
            
            Vector3 edgePoint = muzzle.Position + muzzle.Direction.normalized * maxDistance;
            if (useRaycast.IsEnabled == false)
            {
                points.Add(edgePoint);
                return (muzzle.Position - edgePoint).normalized;
            }

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
                points.Add(hit.point);
                return hit.normal;
            }

            points.Add(edgePoint);
            return Vector3.zero;
        }
    }
}