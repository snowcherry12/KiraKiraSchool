using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Title("Is Sight ID")]
    [Description("Checks if the Character is currently in a specific Sight on a Weapon")]

    [Category("Shooter/Sights/Is Sight ID")]
    
    [Parameter("Character", "The Character reference with a Weapon equipped")]
    [Parameter("Weapon", "The weapon to check")]

    [Keywords("Shooter", "Pose", "Aiming")]
    [Image(typeof(IconSight), ColorTheme.Type.Red)]
    
    [Serializable]
    public class ConditionShooterIsSightID : Condition
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField]
        private PropertyGetGameObject m_Character = GetGameObjectSelf.Create();
        
        [SerializeField]
        private PropertyGetWeapon m_Weapon = GetWeaponNone.Create();

        [SerializeField]
        private PropertyGetString m_SightID = GetStringId.Create("sight-id");
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"{this.m_Character} {this.m_Weapon} is {this.m_SightID}";

        // RUN METHOD: ----------------------------------------------------------------------------
        
        protected override bool Run(Args args)
        {
            ShooterWeapon weapon = this.m_Weapon.Get(args) as ShooterWeapon;
            if (weapon == null) return false;
            
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return false;

            WeaponData data = character.Combat.RequestStance<ShooterStance>().Get(weapon);
            return data?.SightId.String == this.m_SightID.Get(args);
        }
    }
}
