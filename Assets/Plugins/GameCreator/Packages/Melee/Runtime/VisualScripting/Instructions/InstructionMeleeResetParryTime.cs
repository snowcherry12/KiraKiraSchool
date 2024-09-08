using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Version(0, 0, 1)]
    
    [Title("Reset Parry Time")]
    [Description("Resets the registered time of the last parried attack")]

    [Category("Melee/Skills/Reset Parry Time")]
    
    [Parameter("Character", "The Character reference resetting the value")]

    [Keywords("Melee", "Combat")]
    [Image(typeof(IconShieldSolid), ColorTheme.Type.Green, typeof(OverlayHourglass))]
    
    [Serializable]
    public class InstructionMeleeResetParryTime : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField]
        private PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Reset {this.m_Character} Parry Time";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            if (character != null) character.Combat.ResetParryTime();
            
            return DefaultResult;
        }
    }
}