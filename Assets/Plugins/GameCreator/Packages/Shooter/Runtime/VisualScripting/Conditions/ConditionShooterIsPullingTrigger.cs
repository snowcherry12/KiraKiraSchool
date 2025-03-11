using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Title("Is Pulling Trigger")]
    [Description("Checks if the Character is pulling the trigger of a weapon and it's valid")]

    [Category("Shooter/Shooting/Is Pulling Trigger")]
    
    [Parameter("Character", "The Character reference with a Weapon equipped")]
    [Parameter("Weapon", "The weapon to check")]

    [Keywords("Shooter", "Combat", "Shoot", "Execute", "Trigger", "Press", "Blast")]
    [Image(typeof(IconTriggerPull), ColorTheme.Type.Blue, typeof(OverlayArrowLeft))]
    
    [Serializable]
    public class ConditionShooterIsPullingTrigger : Condition
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField]
        private PropertyGetGameObject m_Character = GetGameObjectSelf.Create();

        [SerializeField]
        private PropertyGetWeapon m_Weapon = GetWeaponNone.Create();
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"{this.m_Character} Pulling Trigger of {this.m_Weapon}";

        // RUN METHOD: ----------------------------------------------------------------------------
        
        protected override bool Run(Args args)
        {
            ShooterWeapon weapon = this.m_Weapon.Get(args) as ShooterWeapon;
            if (weapon == null) return false;
            
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return false;

            WeaponData data = character.Combat.RequestStance<ShooterStance>().Get(weapon);
            return data is { IsPullingTrigger: true };
        }
    }
}
