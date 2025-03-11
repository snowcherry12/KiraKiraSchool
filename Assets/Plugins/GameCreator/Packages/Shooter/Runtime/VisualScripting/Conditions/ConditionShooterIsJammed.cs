using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Title("Is Jammed")]
    [Description("Checks if the Shooter Weapon on a Character is jammed")]

    [Category("Shooter/Jam/Is Jammed")]
    
    [Parameter("Character", "The Character reference with a Weapon equipped")]
    [Parameter("Weapon", "The weapon to check")]

    [Keywords("Shooter", "Combat", "Jam", "Malfunction")]
    [Image(typeof(IconJam), ColorTheme.Type.Green)]
    
    [Serializable]
    public class ConditionShooterIsJammed : Condition
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField]
        private PropertyGetGameObject m_Character = GetGameObjectSelf.Create();

        [SerializeField]
        private PropertyGetWeapon m_Weapon = GetWeaponNone.Create();
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"{this.m_Character} is Jammed {this.m_Weapon}";

        // RUN METHOD: ----------------------------------------------------------------------------
        
        protected override bool Run(Args args)
        {
            ShooterWeapon weapon = this.m_Weapon.Get(args) as ShooterWeapon;
            if (weapon == null) return false;
            
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return false;
            
            WeaponData data = character.Combat.RequestStance<ShooterStance>().Get(weapon);
            return data is { IsJammed: true };
        }
    }
}
