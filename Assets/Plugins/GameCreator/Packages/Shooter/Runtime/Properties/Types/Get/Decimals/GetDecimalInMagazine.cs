using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Title("Magazine")]
    [Category("Shooter/Ammo/Magazine")]

    [Image(typeof(IconMagazine), ColorTheme.Type.Green)]
    [Description("The amount of Magazine ammo in the Character weapon")]
    
    [Serializable]
    public class GetDecimalInMagazine : PropertyTypeGetDecimal
    {
        [SerializeField] private PropertyGetGameObject m_Character = GetGameObjectSelf.Create();
        [SerializeField] private PropertyGetWeapon m_Weapon = GetWeaponShooterCharacter.Create();
        
        public override double Get(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            IWeapon weapon = this.m_Weapon.Get(args);

            if (character == null) return 0;
            if (weapon == null) return 0;

            return character.Combat.RequestMunition(weapon) is ShooterMunition munition
                ? munition.InMagazine
                : 0;
        }

        public static PropertyGetDecimal Create => new PropertyGetDecimal(
            new GetDecimalInMagazine()
        );

        public override string String => $"{this.m_Character}[{m_Weapon}] Magazine";
    }
}