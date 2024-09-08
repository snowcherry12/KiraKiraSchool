using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Version(0, 0, 1)]
    
    [Title("Equip Melee Weapon")]
    [Description("Equips a Melee Weapon on the targeted Character if possible")]

    [Category("Melee/Equip/Equip Melee Weapon")]
    
    [Parameter("Character", "The Character reference equipping the weapon")]
    [Parameter("Weapon", "The weapon reference to equip")]
    [Parameter("Model", "The optional 3D model instance")]

    [Keywords("Melee", "Combat")]
    [Image(typeof(IconMeleeSword), ColorTheme.Type.Blue, typeof(OverlayPlus))]
    
    [Serializable]
    public class InstructionMeleeEquipWeapon : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField]
        private PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();

        [SerializeField]
        private PropertyGetWeapon m_Weapon = GetWeaponMeleeInstance.Create();
        
        [SerializeField]
        private PropertyGetGameObject m_Model = GetGameObjectNone.Create();

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Equip {this.m_Weapon} on {this.m_Character}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override async Task Run(Args args)
        {
            MeleeWeapon weapon = this.m_Weapon.Get(args) as MeleeWeapon;
            if (weapon == null) return;
            
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return;

            GameObject model = this.m_Model.Get(args);
            await character.Combat.Equip(weapon, model, args);
        }
    }
}