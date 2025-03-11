using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Version(0, 0, 1)]
    
    [Title("Release Fire Trigger")]
    [Description("Releases the fire trigger on a shooter weapon")]

    [Category("Shooter/Shooting/Release Fire Trigger")]
    
    [Parameter("Character", "The Character reference with a Weapon equipped")]
    [Parameter("Weapon", "Optional field. The weapon to shoot if there are more than one")]
    [Parameter("Shot ID", "Optional Shot type to fire. Uses the default Shot if empty")]

    [Keywords("Shooter", "Combat", "Shoot", "Execute", "Trigger", "Press", "Blast")]
    [Image(typeof(IconTriggerRelease), ColorTheme.Type.Red, typeof(OverlayArrowRight))]
    
    [Serializable]
    public class InstructionShooterFireRelease : Instruction
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField]
        private PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();

        [SerializeField]
        private PropertyGetWeapon m_Weapon = GetWeaponNone.Create();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Release Fire {this.m_Weapon}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return DefaultResult;

            ShooterWeapon weapon = this.m_Weapon.Get(args) as ShooterWeapon;
            ShooterStance stance = character.Combat.RequestStance<ShooterStance>();
            
            stance.ReleaseTrigger(weapon);
            return DefaultResult;
        }
    }
}