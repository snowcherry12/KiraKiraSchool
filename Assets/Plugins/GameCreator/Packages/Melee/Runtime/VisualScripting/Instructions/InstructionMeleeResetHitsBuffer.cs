using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Version(0, 0, 1)]
    
    [Title("Reset Skill Hits")]
    [Description("Resets the hit performed by the ongoing Skill")]

    [Category("Melee/Skills/Reset Skill Hits")]
    
    [Parameter("Character", "The Character reference resetting the hit buffer")]

    [Keywords("Melee", "Combat")]
    [Image(typeof(IconMeleeSkill), ColorTheme.Type.TextLight, typeof(OverlayCross))]
    
    [Serializable]
    public class InstructionMeleeResetHitsBuffer : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField]
        private PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Reset {this.m_Character} Skill Hits";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return DefaultResult;
            
            character.Combat.RequestStance<MeleeStance>().ResetHitsBuffer();
            return DefaultResult;
        }
    }
}