using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Title("Has Equipped Shooter")]
    [Description("Returns true if the Character has a specific Shooter Weapon equipped")]

    [Category("Shooter/Has Equipped Shooter")]
    
    [Parameter("Character", "The targeted Character")]
    [Parameter("Weapon", "The Shooter Weapon to check if it is equipped")]

    [Keywords("Combat", "Shooter")]
    
    [Image(typeof(IconPistol), ColorTheme.Type.Blue)]
    
    [Serializable]
    public class ConditionShooterHasEquipped : Condition
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();
        [SerializeField] private PropertyGetWeapon m_Weapon = new PropertyGetWeapon();

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"has {this.m_Character} Equipped {this.m_Weapon}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            ShooterWeapon shooterWeapon = this.m_Weapon.Get(args) as ShooterWeapon;
            
            return character != null &&
                   shooterWeapon != null &&
                   character.Combat.IsEquipped(shooterWeapon);
        }
    }
}
