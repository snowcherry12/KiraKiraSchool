using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Version(0, 0, 1)]
    
    [Title("Unequip Shooter Weapon")]
    [Description("Unequip a Shooter Weapon from the targeted Character if possible")]

    [Category("Shooter/Equip/Unequip Shooter Weapon")]
    
    [Parameter("Character", "The Character reference unequipping the weapon")]
    [Parameter("Weapon", "The weapon reference to unequip")]

    [Keywords("Shooter", "Combat")]
    [Image(typeof(IconPistol), ColorTheme.Type.Red, typeof(OverlayCross))]
    
    [Serializable]
    public class InstructionShooterUnequipWeapon : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField]
        private PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();

        [SerializeField]
        private PropertyGetWeapon m_Weapon = GetWeaponShooterInstance.Create();

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Unequip {this.m_Weapon} from {this.m_Character}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override async Task Run(Args args)
        {
            ShooterWeapon weapon = this.m_Weapon.Get(args) as ShooterWeapon;
            if (weapon == null) return;
            
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return;

            await character.Combat.Unequip(weapon, args);
        }
    }
}