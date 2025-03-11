using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Serializable]
    public abstract class TShotProjectile : TShot
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] protected PropertyGetGameObject m_Prefab = GetGameObjectInstance.Create();
        [SerializeField] private PropertyGetDecimal m_Delay = GetDecimalConstantZero.Create; 
        
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
            GameObject prop = character.Combat.GetProp(weapon);

            for (int i = 0; i < projectilesUsed; ++i)
            {
                Vector3 spreadDirection = sight.Sight.GetSpreadDirection(
                    weaponData.WeaponArgs, 
                    weapon
                );
                
                ShotData shotData = new ShotData(
                    character,
                    weapon, weaponData.SightId,
                    prop,
                    muzzle.Position,
                    spreadDirection,
                    impactSound,
                    impactEffect,
                    i == 0 ? cartridgesUsed : 0,
                    chargeRatio,
                    (float) this.m_Delay.Get(weaponData.WeaponArgs),
                    pullTime
                );

                if (weapon.CanShoot(shotData, weaponData.WeaponArgs) == false) return false;
            
                GameObject prefab = this.m_Prefab.Get(weaponData.WeaponArgs);
                GameObject projectile = PoolManager.Instance.Pick(
                    prefab,
                    muzzle.Position,
                    Quaternion.LookRotation(spreadDirection),
                    1
                );
            
                shotData.UpdateProjectile(projectile);
                weapon.OnShoot(shotData, weaponData.WeaponArgs);
            
                this.OnRun(weaponData, shotData);
            }
            
            return true;
        }
        
        // ABSTRACT METHODS: ----------------------------------------------------------------------

        protected abstract void OnRun(WeaponData weaponData, ShotData shotData);
    }
}