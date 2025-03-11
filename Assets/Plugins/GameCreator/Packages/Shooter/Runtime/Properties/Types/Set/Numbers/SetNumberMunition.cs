using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Title("Munition")]
    [Category("Shooter/Munition")]

    [Image(typeof(IconBullet), ColorTheme.Type.Red)]
    [Description("The amount of Munition value of a Character with a Weapon")]

    [Serializable]
    public class SetNumberMunition : PropertyTypeSetNumber
    {
        [SerializeField] private PropertyGetGameObject m_Character = GetGameObjectSelf.Create();
        [SerializeField] private PropertyGetWeapon m_Weapon = GetWeaponShooterCharacter.Create();

        public override void Set(double value, Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            IWeapon weapon = this.m_Weapon.Get(args);
            
            if (character == null) return;
            if (weapon == null) return;

            if (character.Combat.RequestMunition(weapon) is ShooterMunition munition)
            {
                munition.Total = (int) value;
            }
        }

        public override double Get(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            IWeapon weapon = this.m_Weapon.Get(args);

            if (character == null) return 0;
            if (weapon == null) return 0;

            return character.Combat.RequestMunition(weapon) is ShooterMunition munition
                ? munition.Total
                : 0;
        }
        
        public static PropertySetNumber Create => new PropertySetNumber(
            new SetNumberMunition()
        );

        public override string String => $"{this.m_Character}[{m_Weapon}] Munition";
    }
}