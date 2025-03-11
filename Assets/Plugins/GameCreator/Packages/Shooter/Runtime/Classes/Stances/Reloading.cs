using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    public class Reloading
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [NonSerialized] private CancelReloadSequence m_CancelReload;

        [NonSerialized] private float m_StartTime;
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        [field: NonSerialized] private Character Character { get; set; }

        [field: NonSerialized] public ShooterWeapon WeaponReloading { get; private set; }
        [field: NonSerialized] public Reload ReloadReloading { get; private set; }
        [field: NonSerialized] public float ReloadSpeed { get; private set; }
        
        public bool IsReloading => this.WeaponReloading != null;
        [field: NonSerialized] public bool TriedQuickReload { get; private set; }
        [field: NonSerialized] public bool CanPartialReload { get; private set; }
        
        public bool CanQuickReload
        {
            get
            {
                if (this.ReloadReloading == null) return false;

                float t = this.Ratio;
                return this.ReloadReloading.CanQuickReload(t);
            }
        }

        public float Ratio
        {
            get
            {
                if (this.IsReloading == false) return -1f;
                
                float time = this.Character.Time.Time - this.m_StartTime;
                float duration = this.ReloadReloading.Duration / this.ReloadSpeed;
                
                return duration > 0f ? time / duration : 1f;
            }
        }
        
        public GameObject PreviousMagazine { get; internal set; }
        public GameObject CurrentMagazine { get; internal set; }

        public Vector2 QuickReloadRange => this.ReloadReloading != null 
            ? this.ReloadReloading.GetQuickReload()
            : Vector2.zero;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public Reloading(Character character)
        {
            this.Character = character;
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public async Task Reload(ShooterWeapon weapon)
        {
            if (weapon == null) return;
            if (this.IsReloading) return;
            
            ShooterStance stance = this.Character.Combat.RequestStance<ShooterStance>();
            WeaponData weaponData = stance.Get(weapon);
            
            if (weaponData.IsJammed) return;
            
            Args args = stance.Get(weapon).WeaponArgs;
            ShooterMunition munition = (ShooterMunition) this.Character.Combat.RequestMunition(weapon);
            bool isFull = munition.InMagazine >= weapon.Magazine.GetMagazineSize(args);
            
            Reload reload = weapon.GetReload(this.Character, isFull);
            if (reload == null) return;

            GameObject prop = this.Character.Combat.GetProp(weapon);
            if (prop == null) return;
            
            this.m_CancelReload = new CancelReloadSequence();

            this.WeaponReloading = weapon;
            this.ReloadReloading = reload;
            
            this.CanPartialReload = false;
            this.ReloadSpeed = reload.GetSpeed(args);

            await weapon.RunOnEnterReload(this.Character, this.ReloadSpeed, args);
            
            if (reload.GetDiscardMagazineAmmo(args))
            {
                int remaining = munition.InMagazine;
                if (remaining != 0) weapon.Magazine.DiscardMagazine(remaining, args);
            }

            bool isComplete = false;
            
            while (isComplete == false)
            {
                this.TriedQuickReload = false;
                this.CanPartialReload = true;
                
                this.m_StartTime = this.Character.Time.Time;
                
                bool correct = await reload.Play(
                    this.Character,
                    this.ReloadSpeed,
                    this.m_CancelReload,
                    args
                );
                
                if (correct == false || this.m_CancelReload.CancelReason == CancelReason.ForceStop)
                {
                    isComplete = true;
                    continue;
                }

                if (!weapon.Magazine.GetHasMagazine(args))
                {
                    isComplete = true;
                    continue;
                }
                
                int reloadAmount = reload.GetReloadAmount(args);
                int magazineSize = weapon.Magazine.GetMagazineSize(args);
                
                munition.InMagazine = Math.Min(munition.InMagazine + reloadAmount, magazineSize);
                bool partialReload = this.m_CancelReload.CancelReason == CancelReason.PartialReload;
                
                isComplete = partialReload || munition.InMagazine >= magazineSize;
            }
            
            this.CanPartialReload = false;
            
            await weapon.RunOnExitReload(
                this.Character,
                this.ReloadSpeed,
                this.m_CancelReload,
                args
            );
            
            this.WeaponReloading = null;
            this.ReloadReloading = null;
        }
        
        public void Stop(ShooterWeapon weapon, CancelReason cancelReason)
        {
            if (weapon == null) return;
            
            if (this.IsReloading == false) return;
            if (this.ReloadReloading == null) return;
            
            Args args = this.Character.Combat.RequestStance<ShooterStance>().Get(weapon).WeaponArgs;
            this.ReloadReloading.Stop(this.Character, this.m_CancelReload, args, cancelReason);
        }

        public bool TryQuickReload(ShooterWeapon weapon)
        {
            if (weapon == null) return false;
            if (this.IsReloading == false) return false;
            if (this.TriedQuickReload) return false;
            
            if (this.ReloadReloading == null) return false;
            
            if (this.CanQuickReload)
            {
                Args args = this.Character.Combat.RequestStance<ShooterStance>().Get(weapon).WeaponArgs;
                this.ReloadReloading.Stop(
                    this.Character,
                    this.m_CancelReload,
                    args,
                    CancelReason.QuickReload
                );
                
                return true;
            }

            this.TriedQuickReload = true;
            return false;
        }
    }
}