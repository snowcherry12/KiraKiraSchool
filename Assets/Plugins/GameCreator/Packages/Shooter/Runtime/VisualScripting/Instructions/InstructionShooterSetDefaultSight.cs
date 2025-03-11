using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Version(0, 0, 1)]
    
    [Title("Set Default Sight")]
    [Description("Changes the to a new Sight of the specified Shooter Weapon")]

    [Category("Shooter/Sights/Set Default Sight")]
    
    [Parameter("Character", "The Character reference with a Weapon equipped")]
    [Parameter("Weapon", "Optional field. The weapon to shoot if there are more than one")]

    [Keywords("Shooter", "Combat", "Aim", "Scope", "Ease", "Draw", "Holster")]
    [Image(typeof(IconSight), ColorTheme.Type.Green, typeof(OverlayDot))]
    
    [Serializable]
    public class InstructionShooterSetDefaultSight : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField]
        private PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();

        [SerializeField]
        private PropertyGetWeapon m_Weapon = GetWeaponNone.Create();

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Sight {this.m_Character} = Default";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return DefaultResult;

            ShooterWeapon weapon = this.m_Weapon.Get(args) as ShooterWeapon;
            ShooterStance stance = character.Combat.RequestStance<ShooterStance>();
            
            stance.ExitSight(weapon);
            return DefaultResult;
        }
    }
}