using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Version(0, 0, 1)]
    
    [Title("Fix Jam")]
    [Description("Attempts to fix a jammed Weapon")]

    [Category("Shooter/Jam/Fix Jam")]
    
    [Parameter("Character", "The Character reference with a Weapon equipped")]
    [Parameter("Weapon", "Optional field. The weapon to fix if there are more than one")]

    [Keywords("Shooter", "Combat", "Fix", "Jammed", "Jamming", "Malfunction", "Feed")]
    [Image(typeof(IconJam), ColorTheme.Type.Green)]
    
    [Serializable]
    public class InstructionShooterFixJam : Instruction
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField]
        private PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();

        [SerializeField]
        private PropertyGetWeapon m_Weapon = GetWeaponNone.Create();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Fix {this.m_Weapon}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override async Task Run(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return;

            ShooterWeapon weapon = this.m_Weapon.Get(args) as ShooterWeapon;
            ShooterStance stance = character.Combat.RequestStance<ShooterStance>();

            await stance.FixJam(weapon);
        }
    }
}