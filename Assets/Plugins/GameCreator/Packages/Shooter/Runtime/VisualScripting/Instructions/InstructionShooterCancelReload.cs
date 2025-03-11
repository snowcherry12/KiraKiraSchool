using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Version(0, 0, 1)]
    
    [Title("Try Quick Reload")]
    [Description("Attempts to Cancel the Reload during a Quick Reload phase")]

    [Category("Shooter/Reload/Try Quick Reload")]

    [Parameter("Character", "The Character reference with a Weapon equipped")]
    [Parameter("Weapon", "Optional field. The weapon to Quick Reload if there is more than one")]
    
    [Keywords("Shooter", "Combat", "Ammo", "Load", "Quick", "Fast", "Skip")]
    [Image(typeof(IconReload), ColorTheme.Type.Green, typeof(OverlayHourglass))]
    
    [Serializable]
    public class InstructionShooterCancelReload : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField]
        private PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();

        [SerializeField]
        private PropertyGetWeapon m_Weapon = GetWeaponNone.Create();

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Try Quick Reload {this.m_Weapon} on {this.m_Character}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            ShooterWeapon weapon = this.m_Weapon.Get(args) as ShooterWeapon;
            Character character = this.m_Character.Get<Character>(args);
            
            if (character == null) return DefaultResult;

            character.Combat
                .RequestStance<ShooterStance>()
                .TryQuickReload(weapon);
            
            return DefaultResult;
        }
    }
}