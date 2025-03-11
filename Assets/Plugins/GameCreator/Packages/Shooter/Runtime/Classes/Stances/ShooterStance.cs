using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    public class ShooterStance : TStance
    {
        public static readonly int ID = "Shooter".GetHashCode();
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized]
        private readonly Dictionary<int, WeaponData> m_Equipment = new Dictionary<int, WeaponData>();

        [NonSerialized]
        private readonly AnimFloat m_Accuracy = new AnimFloat(0f, 0f);
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override int Id => ID;
        
        [field: NonSerialized] public override Character Character { get; set; }
        
        [field: NonSerialized] public Args Args { get; private set; }
        
        public float CurrentAccuracy => this.m_Accuracy.Current;

        [field: NonSerialized] public Reloading Reloading { get; private set; }
        [field: NonSerialized] public Shooting Shooting { get; private set; } 
        [field: NonSerialized] public Jamming Jamming { get; private set; }
        
        // EVENTS: --------------------------------------------------------------------------------

        

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public ShooterStance()
        { }

        // STANCE METHODS: ------------------------------------------------------------------------
        
        public override void OnEnable(Character character)
        {
            if (this.IsEnabled) return;
            base.OnEnable(character);
            
            this.Character = character;
            this.Args ??= new Args(
                this.Character.gameObject,
                this.Character.Combat.Targets.Primary
            );

            this.m_Accuracy.Target = 1f;
            
            this.Reloading = new Reloading(character);
            this.Shooting = new Shooting(character);
            this.Jamming = new Jamming(character);
            
            this.Character.Combat.Targets.EventChangeTarget -= this.OnChangeTarget;
            this.Character.Combat.Targets.EventChangeTarget += this.OnChangeTarget;
            
            this.Character.Combat.EventEquip -= this.OnEquipWeapon;
            this.Character.Combat.EventUnequip -= this.OnUnequipWeapon;
            
            this.Character.Combat.EventEquip += this.OnEquipWeapon;
            this.Character.Combat.EventUnequip += this.OnUnequipWeapon;
            
            UpdateManager.SubscribeLateUpdate(
                this.OnLateUpdate,
                ApplicationManager.EXECUTION_ORDER_LAST
            );
        }

        public override void OnDisable(Character character)
        {
            if (ApplicationManager.IsExiting) return;
            base.OnDisable(character);
            
            this.Character.Combat.Targets.EventChangeTarget -= this.OnChangeTarget;
            this.Character.Combat.EventEquip -= this.OnEquipWeapon;
            this.Character.Combat.EventUnequip -= this.OnUnequipWeapon;
            
            UpdateManager.UnsubscribeLateUpdate(
                this.OnLateUpdate,
                ApplicationManager.EXECUTION_ORDER_LAST
            );
        }

        public override void OnUpdate()
        { }
        
        private void OnLateUpdate()
        {
            this.m_Accuracy.Target = 0f;
            float accuracyRecovery = 0f;
            
            foreach (KeyValuePair<int, WeaponData> entry in this.m_Equipment)
            {
                entry.Value.OnUpdate();
                Accuracy accuracy = entry.Value.Weapon.Accuracy;
                
                this.m_Accuracy.Target = Mathf.Max(
                    accuracy.CalculateTargetAccuracy(this, entry.Value),
                    this.m_Accuracy.Target
                );
                
                accuracyRecovery = Mathf.Max(
                    accuracy.GetAccuracyRecover(entry.Value.WeaponArgs),
                    accuracyRecovery
                );
            }
            
            if (this.m_Accuracy.Current < this.m_Accuracy.Target)
            {
                this.m_Accuracy.Current = this.m_Accuracy.Target;
            }
            
            this.m_Accuracy.UpdateWithDelta(
                this.m_Accuracy.Target,
                accuracyRecovery,
                this.Character.Time.DeltaTime
            );
        }
        
        // PUBLIC GETTERS: ------------------------------------------------------------------------

        public WeaponData Get(ShooterWeapon weapon)
        {
            int weaponInstanceId = weapon != null ? weapon.GetInstanceID() : default;
            return this.m_Equipment.GetValueOrDefault(weaponInstanceId);
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void EnterSight(ShooterWeapon optionalWeapon, IdString sightId)
        {
            ShooterWeapon shooterWeapon = this.GetWeapon(optionalWeapon);
            if (shooterWeapon == null) return;

            if (shooterWeapon.Sights.Contains(sightId))
            {
                int weaponInstanceId = shooterWeapon.GetInstanceID();
                WeaponData weaponData = this.m_Equipment[weaponInstanceId];
                
                if (weaponData.SightId == sightId) return;
                shooterWeapon.Sights.Get(weaponData.SightId)?.Exit(this.Character, shooterWeapon);
                
                weaponData.OnChangeSight(sightId);
                shooterWeapon.Sights.Get(weaponData.SightId)?.Enter(this.Character, shooterWeapon);
            }
        }
        
        public void ExitSight(ShooterWeapon optionalWeapon)
        {
            ShooterWeapon shooterWeapon = this.GetWeapon(optionalWeapon);
            if (shooterWeapon == null) return;

            IdString sightId = shooterWeapon.Sights.DefaultId;
            this.EnterSight(shooterWeapon, sightId);
        }
        
        /// <summary>
        /// Pulls the fire trigger of the specified weapon (or default one) on a specific
        /// shot angle
        /// </summary>
        /// <param name="optionalWeapon"></param>
        public void PullTrigger(ShooterWeapon optionalWeapon)
        {
            ShooterWeapon shooterWeapon = this.GetWeapon(optionalWeapon);
            if (shooterWeapon == null) return;
            
            int shooterWeaponId = shooterWeapon.GetInstanceID();
            if (!this.m_Equipment.TryGetValue(shooterWeaponId, out WeaponData weaponData)) return;

            if (this.Reloading.WeaponReloading == shooterWeapon)
            {
                if (this.Reloading.CanPartialReload == false) return;
                
                this.StopReload(shooterWeapon, CancelReason.PartialReload);
                return;
            }
            
            if (!this.Reloading.IsReloading)
            {
                bool autoReload = shooterWeapon.Magazine.AutoReload(weaponData.WeaponArgs);
                int ammo = shooterWeapon.Magazine.GetAmmo(weaponData.WeaponArgs);
                
                ShooterMunition munition = (ShooterMunition) this.Character
                    .Combat
                    .RequestMunition(shooterWeapon);

                int inMagazine = shooterWeapon.Magazine.GetHasMagazine(weaponData.WeaponArgs)
                    ? munition.InMagazine
                    : int.MaxValue;
                
                if (autoReload && ammo != 0 && inMagazine == 0)
                {
                    _ = this.Reload(shooterWeapon);
                    return;
                }
            }
            
            weaponData.OnPullTrigger();
        }
        
        /// <summary>
        /// Releases the fire trigger of the specified weapon (or default one) on a specific
        /// shot angle
        /// </summary>
        /// <param name="optionalWeapon"></param>
        public void ReleaseTrigger(ShooterWeapon optionalWeapon)
        {
            ShooterWeapon shooterWeapon = this.GetWeapon(optionalWeapon);
            if (shooterWeapon == null) return;
            
            int shooterWeaponId = shooterWeapon.GetInstanceID();
            if (!this.m_Equipment.TryGetValue(shooterWeaponId, out WeaponData weaponData)) return;
            
            weaponData.OnReleaseTrigger();
        }

        /// <summary>
        /// Starts reloading the specified weapon or the default one
        /// </summary>
        /// <param name="optionalWeapon"></param>
        public async Task Reload(ShooterWeapon optionalWeapon)
        {
            ShooterWeapon shooterWeapon = this.GetWeapon(optionalWeapon);
            if (shooterWeapon == null) return;
            
            await this.Reloading.Reload(shooterWeapon);
        }

        /// <summary>
        /// Forces the reloading status at any point
        /// </summary>
        /// <param name="optionalWeapon"></param>
        /// <param name="reason"></param>
        public void StopReload(ShooterWeapon optionalWeapon, CancelReason reason)
        {
            ShooterWeapon shooterWeapon = this.GetWeapon(optionalWeapon);
            if (shooterWeapon == null) return;
            
            this.Reloading.Stop(shooterWeapon, reason);
        }

        /// <summary>
        /// Attempts to perform a quick reload. Returns false if it fails. True otherwise and
        /// stops the reloading
        /// </summary>
        /// <param name="optionalWeapon"></param>
        /// <returns></returns>
        public bool TryQuickReload(ShooterWeapon optionalWeapon)
        {
            ShooterWeapon shooterWeapon = this.GetWeapon(optionalWeapon);
            return shooterWeapon != null && this.Reloading.TryQuickReload(shooterWeapon);
        }

        /// <summary>
        /// Forces to jam a Weapon on a Character
        /// </summary>
        /// <param name="optionalWeapon"></param>
        public void Jam(ShooterWeapon optionalWeapon)
        {
            ShooterWeapon shooterWeapon = this.GetWeapon(optionalWeapon);
            if (shooterWeapon == null) return;
            
            this.Jamming.Jam(shooterWeapon);
        }

        /// <summary>
        /// Attempts to fix a jammed Weapon
        /// </summary>
        /// <param name="optionalWeapon"></param>
        public async Task FixJam(ShooterWeapon optionalWeapon)
        {
            ShooterWeapon shooterWeapon = this.GetWeapon(optionalWeapon);
            if (shooterWeapon == null) return;
            
            await this.Jamming.Fix(shooterWeapon);
        }
        
        /// <summary>
        /// Plays the ejection particle of an empty Shell leaving the weapon
        /// </summary>
        /// <param name="optionalWeapon"></param>
        public void EjectShell(ShooterWeapon optionalWeapon)
        {
            ShooterWeapon weapon = this.GetWeapon(optionalWeapon);
            if (weapon == null) return;

            int shooterWeaponId = weapon.GetInstanceID();
            if (!this.m_Equipment.TryGetValue(shooterWeaponId, out WeaponData weaponData)) return;
            
            weapon.Shell.Eject(weaponData.WeaponArgs);
        }
        
        /// <summary>
        /// Kicks the accuracy with a value so it's accuracy instantly decreases but recovers
        /// back to the targeted accuracy
        /// </summary>
        /// <param name="kick"></param>
        public void AccuracyKick(float kick)
        {
            float currentAccuracy = Mathf.Clamp01(this.m_Accuracy.Current + kick);
            this.m_Accuracy.Current = currentAccuracy;
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private ShooterWeapon GetWeapon(ShooterWeapon weapon)
        {
            if (weapon != null && this.m_Equipment.ContainsKey(weapon.GetInstanceID()))
            {
                return weapon;
            }

            foreach (Weapon currentWeapon in this.Character.Combat.Weapons)
            {
                if (currentWeapon?.Asset is not ShooterWeapon shooterWeapon) continue;
                if (this.m_Equipment.ContainsKey(shooterWeapon.GetInstanceID()))
                {
                    return shooterWeapon;
                }
            }

            return null;
        }
        
        private void OnChangeTarget(GameObject newTarget)
        {
            this.Args.ChangeTarget(newTarget);
        }
        
        private void OnEquipWeapon(IWeapon weapon, GameObject prop)
        {
            if (weapon is not ShooterWeapon shooterWeapon) return;

            WeaponData weaponData = new WeaponData(this.Character, shooterWeapon, prop);
            this.m_Equipment[shooterWeapon.GetInstanceID()] = weaponData;
            
            this.EnterSight(shooterWeapon, shooterWeapon.Sights.DefaultId);
            weaponData.OnEquip();
            
            this.m_Accuracy.Target = 1f;
        }
        
        private void OnUnequipWeapon(IWeapon weapon, GameObject prop)
        {
            if (weapon is not ShooterWeapon shooterWeapon) return;

            int weaponId = shooterWeapon.GetInstanceID();
            if (this.m_Equipment.TryGetValue(weaponId, out WeaponData weaponData))
            {
                weaponData.OnUnequip();
                
                SightItem currentSight = shooterWeapon.Sights.Get(weaponData.SightId);
                currentSight?.Exit(this.Character, shooterWeapon);
            }
            
            this.m_Equipment.Remove(shooterWeapon.GetInstanceID());
            this.m_Accuracy.Target = 1f;
        }
    }
}