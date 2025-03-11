using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Version(0, 0, 1)]
    
    [Title("Eject Shell")]
    [Description("Ejects a Shell from the Weapon at the specified Weapon")]

    [Category("Shooter/Reload/Eject Shell")]
    
    [Parameter("Character", "The Character reference with a Weapon equipped")]
    [Parameter("Weapon", "Optional field. The weapon if there is more than one")]

    [Keywords("Shooter", "Combat", "Reload", "Shell", "Cartridge", "Empty", "Casket")]
    [Image(typeof(IconShell), ColorTheme.Type.Green)]
    
    [Serializable]
    public class InstructionShooterEjectShell : Instruction
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField]
        private PropertyGetGameObject m_Character = GetGameObjectSelf.Create();

        [SerializeField]
        private PropertyGetWeapon m_Weapon = GetWeaponNone.Create();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Eject {this.m_Weapon} Shell from {this.m_Character}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return DefaultResult;

            ShooterWeapon weapon = this.m_Weapon.Get(args) as ShooterWeapon;
            ShooterStance stance = character.Combat.RequestStance<ShooterStance>();

            stance.EjectShell(weapon);
            return DefaultResult;
        }
    }
}