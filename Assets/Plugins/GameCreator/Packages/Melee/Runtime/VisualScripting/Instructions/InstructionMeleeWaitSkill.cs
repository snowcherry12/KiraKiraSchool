using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Version(0, 0, 1)]
    
    [Title("Wait until Phase")]
    [Description("Waits until the current Skill's phase reaches the chosen one")]

    [Category("Melee/Skills/Wait until Phase")]
    
    [Parameter("Character", "The Character reference")]
    [Parameter("Phase", "The Phase which waits to")]

    [Keywords("Melee", "Combat", "Anticipation", "Strike", "Recovery", "Finish", "Combo", "Skill")]
    [Image(typeof(IconMeleeSkill), ColorTheme.Type.Yellow, typeof(OverlayHourglass))]
    
    [Serializable]
    public class InstructionMeleeWaitSkill : Instruction
    {
        private enum UntilPhase
        {
            FinishStrike,
            FinishRecovery
        }
        
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();
        [SerializeField] private UntilPhase m_Until = UntilPhase.FinishStrike;

        [NonSerialized] private MeleeStance m_MeleeStance;

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Wait {this.m_Character} until {TextUtils.Humanize(this.m_Until)}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override async Task Run(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return;

            this.m_MeleeStance = character.Combat.RequestStance<MeleeStance>();
            if (this.m_MeleeStance == null) return;

            switch (this.m_Until)
            {
                case UntilPhase.FinishStrike: await this.Until(this.UntilFinishStrike); break;
                case UntilPhase.FinishRecovery: await this.Until(this.UntilFinishRecovery); break;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        private bool UntilFinishStrike()
        {
            if (this.m_MeleeStance == null) return true;

            return this.m_MeleeStance.CurrentPhase switch
            {
                MeleePhase.None => true,
                MeleePhase.Reaction => true,
                MeleePhase.Charge => true,
                MeleePhase.Anticipation => false,
                MeleePhase.Strike => false,
                MeleePhase.Recovery => true,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        private bool UntilFinishRecovery()
        {
            if (this.m_MeleeStance == null) return true;

            return this.m_MeleeStance.CurrentPhase switch
            {
                MeleePhase.None => true,
                MeleePhase.Reaction => true,
                MeleePhase.Charge => true,
                MeleePhase.Anticipation => false,
                MeleePhase.Strike => false,
                MeleePhase.Recovery => false,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}