using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Version(0, 0, 1)]
    
    [Title("Reload Weapon")]
    [Description("Attempts to Reload a Shooter Weapon")]

    [Category("Shooter/Reload/Reload Weapon")]
    
    [Parameter("Character", "The Character reference with a Weapon equipped")]
    [Parameter("Weapon", "Optional field. The weapon to Reload if there is more than one")]
        
    [Keywords("Shooter", "Combat", "Ammo", "Load")]
    [Image(typeof(IconReload), ColorTheme.Type.Green)]
    
    [Serializable]
    public class InstructionShooterReload : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField]
        private PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();

        [SerializeField]
        private PropertyGetWeapon m_Weapon = GetWeaponNone.Create();

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Reload {this.m_Weapon} on {this.m_Character}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override async Task Run(Args args)
        {
            ShooterWeapon weapon = this.m_Weapon.Get(args) as ShooterWeapon;
            Character character = this.m_Character.Get<Character>(args);
            
            if (character == null) return;
            
            await character.Combat
                .RequestStance<ShooterStance>()
                .Reload(weapon);
        }
    }
}