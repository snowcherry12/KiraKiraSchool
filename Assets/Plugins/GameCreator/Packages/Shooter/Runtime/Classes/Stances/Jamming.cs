using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    public class Jamming
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        [field: NonSerialized] private Character Character { get; set; }
        
        [field: NonSerialized] public bool IsFixing { get; private set; }

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public Jamming(Character character)
        {
            this.Character = character;
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public async Task Fix(ShooterWeapon weapon)
        {
            if (weapon == null) return;

            ShooterStance stance = this.Character.Combat.RequestStance<ShooterStance>();
            WeaponData weaponData = stance.Get(weapon);
            
            if (weaponData == null) return;
            if (weaponData.IsJammed == false) return;
            
            this.IsFixing = true;

            AnimationClip animation = weapon.Jam.GetAnimation(weaponData.WeaponArgs);
            if (animation != null)
            {
                ConfigGesture gestureConfig = new ConfigGesture(
                    0f,
                    animation.length,
                    weapon.Jam.GetSpeed(weaponData.WeaponArgs),
                    false,
                    weapon.Jam.TransitionIn,
                    weapon.Jam.TransitionOut
                );

                await this.Character.Gestures.CrossFade(
                    animation, weapon.Jam.Mask, BlendMode.Blend, 
                    gestureConfig, true
                );
            }
            
            this.IsFixing = false;
            weaponData.IsJammed = false;
        }

        public void Jam(ShooterWeapon weapon)
        {
            if (weapon == null) return;

            ShooterStance stance = this.Character.Combat.RequestStance<ShooterStance>();
            WeaponData weaponData = stance.Get(weapon);
            
            if (weaponData == null) return;
            if (weaponData.IsJammed) return;
            
            weaponData.IsJammed = true;
        }
    }
}