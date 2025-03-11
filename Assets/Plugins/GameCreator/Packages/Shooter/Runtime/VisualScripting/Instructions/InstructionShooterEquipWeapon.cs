using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Version(0, 0, 1)]
    
    [Title("Equip Shooter Weapon")]
    [Description("Equips a Shooter Weapon on the targeted Character if possible")]

    [Category("Shooter/Equip/Equip Shooter Weapon")]
    
    [Parameter("Character", "The Character reference equipping the weapon")]
    [Parameter("Weapon", "The weapon reference to equip")]

    [Keywords("Shooter", "Combat")]
    [Image(typeof(IconPistol), ColorTheme.Type.Blue, typeof(OverlayPlus))]
    
    [Serializable]
    public class InstructionShooterEquipWeapon : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField]
        private PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();

        [SerializeField]
        private PropertyGetWeapon m_Weapon = GetWeaponShooterInstance.Create();
        
        [SerializeField]
        private PropertyGetGameObject m_Model = GetGameObjectCharactersLastPropAttached.Create;

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Equip {this.m_Weapon} on {this.m_Character}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override async Task Run(Args args)
        {
            ShooterWeapon weapon = this.m_Weapon.Get(args) as ShooterWeapon;
            if (weapon == null) return;
            
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return;

            if (character.Combat.IsEquipped(weapon)) return;

            GameObject model = this.m_Model.Get(args);
            if (model == null) return;
            
            await character.Combat.Equip(weapon, model, args);
        }
    }
}