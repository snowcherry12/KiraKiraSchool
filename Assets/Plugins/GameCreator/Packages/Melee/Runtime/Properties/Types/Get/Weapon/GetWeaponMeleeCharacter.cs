using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Title("Current Equipped")]
    [Category("Melee/Current Equipped")]

    [Image(typeof(IconMeleeSword), ColorTheme.Type.Yellow, typeof(OverlayDot))]
    [Description("A reference to a Melee Weapon asset equipped by the specified Character")]

    [Serializable]
    public class GetWeaponMeleeCharacter : PropertyTypeGetWeapon
    {
        [SerializeField] protected PropertyGetGameObject m_Character = GetGameObjectSelf.Create();

        public override IWeapon Get(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return null;

            foreach (Weapon weapon in character.Combat.Weapons)
            {
                if (weapon.Asset is not MeleeWeapon) continue;
                return weapon.Asset;
            }

            return null;
        }

        public static PropertyGetWeapon Create()
        {
            GetWeaponMeleeCharacter instance = new GetWeaponMeleeCharacter();
            return new PropertyGetWeapon(instance);
        }

        public override string String => $"{this.m_Character} Melee Weapon";
    }
}
