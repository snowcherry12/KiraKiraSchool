using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Title("Melee Weapon")]
    [Category("Melee/Melee Weapon")]
    
    [Image(typeof(IconMeleeSword), ColorTheme.Type.Yellow)]
    [Description("A reference to a Melee Weapon asset")]

    [Serializable] [HideLabelsInEditor]
    public class GetWeaponMeleeInstance : PropertyTypeGetWeapon
    {
        [SerializeField] protected MeleeWeapon m_Weapon;

        public override IWeapon Get(Args args) => this.m_Weapon;
        public override IWeapon Get(GameObject gameObject) => this.m_Weapon;

        public static PropertyGetWeapon Create(MeleeWeapon weapon = null)
        {
            GetWeaponMeleeInstance instance = new GetWeaponMeleeInstance
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