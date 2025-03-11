using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Title("Current Equipped")]
    [Category("Shooter/Current Equipped")]
    
    [Image(typeof(IconPistol), ColorTheme.Type.Red, typeof(OverlayDot))]
    [Description("A reference to a Shooter Weapon asset equipped by the specified Character")]

    [Serializable]
    public class GetWeaponShooterCharacter : PropertyTypeGetWeapon
    {
        [SerializeField] protected PropertyGetGameObject m_Character = GetGameObjectSelf.Create();

        public override IWeapon Get(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return null;

            foreach (Weapon weapon in character.Combat.Weapons)
            {
                if (weapon.Asset is not ShooterWeapon) continue;
                return weapon.Asset;
            }

            return null;
        }

        public static PropertyGetWeapon Create()
        {
            GetWeaponShooterCharacter instance = new GetWeaponShooterCharacter();
            return new PropertyGetWeapon(instance);
        }

        public override string String => $"{this.m_Character} Shooter Weapon";
    }
}