using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Title("Has Sight ID")]
    [Description("Checks if the Character has a specific Sight on a Weapon")]

    [Category("Shooter/Sights/Has Sight ID")]
    
    [Parameter("Character", "The Character reference with a Weapon equipped")]
    [Parameter("Weapon", "The weapon to check")]

    [Keywords("Shooter", "Pose", "Aiming")]
    [Image(typeof(IconSight), ColorTheme.Type.Red)]
    
    [Serializable]
    public class ConditionShooterHasSightID : Condition
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField]
        private PropertyGetGameObject m_Character = GetGameObjectSelf.Create();
        
        [SerializeField]
        private PropertyGetWeapon m_Weapon = GetWeaponNone.Create();

        [SerializeField]
        private PropertyGetString m_SightID = GetStringId.Create("sight-id");
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"{this.m_Character} {this.m_Weapon} has {this.m_SightID}";

        // RUN METHOD: ----------------------------------------------------------------------------
        
        protected override bool Run(Args args)
        {
            ShooterWeapon weapon = this.m_Weapon.Get(args) as ShooterWeapon;
            if (weapon == null) return false;
            
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return false;

            IdString sightId = new IdString(this.m_SightID.Get(args));
            return weapon.Sights.Get(sightId) != null;
        }
    }
}
