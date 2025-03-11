using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Version(0, 0, 1)]
    
    [Title("Set Munition")]
    [Description("Changes the Munition value of a particular Weapon on a Character")]

    [Category("Shooter/Ammo/Change Munition")]

    [Parameter("Character", "The Character reference with a Weapon equipped")]
    [Parameter("Weapon", "The weapon to reload the munition value")]
    [Parameter("Munition", "The new value for the Weapon's Munition")]
    
    [Keywords("Shooter", "Combat", "Ammo", "Load")]
    [Image(typeof(IconBullet), ColorTheme.Type.Red)]
    
    [Serializable]
    public class InstructionShooterSetMunition : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField]
        private PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();

        [SerializeField]
        private PropertyGetWeapon m_Weapon = GetWeaponNone.Create();

        [SerializeField]
        private PropertyGetInteger m_Munition = new PropertyGetInteger();

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Set {this.m_Character} Munition {this.m_Weapon} = {this.m_Munition}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            ShooterWeapon weapon = this.m_Weapon.Get(args) as ShooterWeapon;
            Character character = this.m_Character.Get<Character>(args);
            
            if (character == null) return DefaultResult;

            ShooterMunition munition = (ShooterMunition) character
                .Combat
                .RequestMunition(weapon);
            
            if (munition == null) return DefaultResult;
            munition.Total = (int) this.m_Munition.Get(args);
            
            return DefaultResult;
        }
    }
}