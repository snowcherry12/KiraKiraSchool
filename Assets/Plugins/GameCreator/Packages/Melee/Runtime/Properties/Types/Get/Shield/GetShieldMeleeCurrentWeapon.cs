using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Title("Current Melee Weapon")]
    [Category("Melee/Current Melee Weapon")]
    
    [Image(typeof(IconShieldSolid), ColorTheme.Type.Red)]
    [Description("A reference to the Shield from the current Melee Weapon asset")]

    [Serializable] [HideLabelsInEditor]
    public class GetShieldMeleeCurrentWeapon : PropertyTypeGetShield
    {
        [SerializeField] private PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();

        public override IShield Get(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return null;

            Weapon[] weapons = character.Combat.Weapons;
            foreach (Weapon weapon in weapons)
            {
                if (weapon.Asset.Shield == null) continue;
                return weapon.Asset.Shield;
            }

            return null;
        }

        public static PropertyGetShield Create()
        {
            GetShieldMeleeCurrentWeapon instance = new GetShieldMeleeCurrentWeapon();
            return new PropertyGetShield(instance);
        }

        public override string String => "Current Shield";
    }
}