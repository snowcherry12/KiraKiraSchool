using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Version(0, 0, 1)]
    
    [Title("Unequip Melee Weapon")]
    [Description("Unequip a Melee Weapon from the targeted Character if possible")]

    [Category("Melee/Equip/Unequip Melee Weapon")]
    
    [Parameter("Character", "The Character reference unequipping the weapon")]
    [Parameter("Weapon", "The weapon reference to unequip")]

    [Keywords("Melee", "Combat")]
    [Image(typeof(IconMeleeSword), ColorTheme.Type.Red, typeof(OverlayCross))]
    
    [Serializable]
    public class InstructionMeleeUnequipWeapon : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField]
        private PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();

        [SerializeField]
        private PropertyGetWeapon m_Weapon = GetWeaponMeleeInstance.Create();

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Unequip {this.m_Weapon} from {this.m_Character}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override async Task Run(Args args)
        {
            MeleeWeapon weapon = this.m_Weapon.Get(args) as MeleeWeapon;
            if (weapon == null) return;
            
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return;

            await character.Combat.Unequip(weapon, args);
        }
    }
}