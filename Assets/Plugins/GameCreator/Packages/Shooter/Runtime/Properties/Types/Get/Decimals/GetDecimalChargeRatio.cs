using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Title("Charge Ratio")]
    [Category("Shooter/Shooting/Charge Ratio")]

    [Image(typeof(IconCharge), ColorTheme.Type.Green)]
    [Description("A value between 0 and 1 that indicates the current charge")]
    
    [Serializable]
    public class GetDecimalChargeRatio : PropertyTypeGetDecimal
    {
        [SerializeField] private PropertyGetGameObject m_Character = GetGameObjectSelf.Create();
        [SerializeField] private PropertyGetWeapon m_Weapon = GetWeaponShooterCharacter.Create();
        
        public override double Get(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            ShooterWeapon weapon = this.m_Weapon.Get(args) as ShooterWeapon;

            if (character == null) return 0;
            if (weapon == null) return 0;

            WeaponData stance = character.Combat.RequestStance<ShooterStance>().Get(weapon);
            return stance.ChargeRatio;
        }

        public static PropertyGetDecimal Create => new PropertyGetDecimal(
            new GetDecimalChargeRatio()
        );

        public override string String => $"{this.m_Character}[{m_Weapon}] Charge";
    }
}