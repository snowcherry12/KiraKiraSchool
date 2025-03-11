using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Title("Shooter Weapon")]
    [Category("Shooter/Shooter Weapon")]
    
    [Image(typeof(IconPistol), ColorTheme.Type.Yellow)]
    [Description("A reference to a Shooter Weapon asset")]

    [Serializable] [HideLabelsInEditor]
    public class GetWeaponShooterInstance : PropertyTypeGetWeapon
    {
        [SerializeField] protected ShooterWeapon m_Weapon;

        public override IWeapon Get(Args args) => this.m_Weapon;
        public override IWeapon Get(GameObject gameObject) => this.m_Weapon;

        public static PropertyGetWeapon Create(ShooterWeapon weapon = null)
        {
            GetWeaponShooterInstance instance = new GetWeaponShooterInstance
            {
                m_Weapon = weapon
            };
            
            return new PropertyGetWeapon(instance);
        }

        public override string String => this.m_Weapon != null
            ? this.m_Weapon.name
            : "(none)";
    }
}