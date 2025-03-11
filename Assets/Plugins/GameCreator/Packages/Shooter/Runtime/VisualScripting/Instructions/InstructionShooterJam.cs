using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Version(0, 0, 1)]
    
    [Title("Jam")]
    [Description("Jams a Weapon on a Character")]

    [Category("Shooter/Jam/Jam")]
    
    [Parameter("Character", "The Character reference with a Weapon equipped")]
    [Parameter("Weapon", "Optional field. The weapon to jam if there are more than one")]

    [Keywords("Shooter", "Combat", "Fix", "Jammed", "Jamming", "Malfunction", "Feed")]
    [Image(typeof(IconJam), ColorTheme.Type.Red)]
    
    [Serializable]
    public class InstructionShooterJam : Instruction
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField]
        private PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();

        [SerializeField]
        private PropertyGetWeapon m_Weapon = GetWeaponNone.Create();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Jam {this.m_Weapon}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return DefaultResult;

            ShooterWeapon weapon = this.m_Weapon.Get(args) as ShooterWeapon;
            ShooterStance stance = character.Combat.RequestStance<ShooterStance>();

            stance.Jam(weapon);
            return DefaultResult;
        }
    }
}