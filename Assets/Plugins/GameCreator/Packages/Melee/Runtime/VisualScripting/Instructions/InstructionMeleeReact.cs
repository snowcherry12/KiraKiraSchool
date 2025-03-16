using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Version(0, 0, 1)]
    
    [Title("Play Melee Reaction")]
    [Description("Plays a Melee Reaction on a Character")]

    [Category("Melee/Skills/Play Melee Reaction")]
    
    [Parameter("Character", "The Character that plays the Melee Reaction")]
    [Parameter("Attacker", "The Character set as the attacker")]
    [Parameter("Reaction", "The Melee Reaction asset played")]

    [Keywords("Melee", "Combat")]
    [Image(typeof(IconReaction), ColorTheme.Type.Yellow)]
    
    [Serializable]
    public class InstructionMeleeReact : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();

        [SerializeField] private PropertyGetGameObject m_Attacker = GetGameObjectNone.Create();

        [SerializeField] private MeleeReaction m_Reaction;

        [SerializeField] private PropertyGetDirection m_Direction = GetDirectionConstantBackward.Create;
        [SerializeField] private PropertyGetDecimal m_Force = GetDecimalConstantZero.Create;

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => string.Format(
            "React {0} with {1}", 
            this.m_Character, 
            this.m_Reaction != null ? TextUtils.Humanize(this.m_Reaction.name) : "(none)"
        );

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            if (this.m_Reaction == null) return DefaultResult;
            
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return DefaultResult;

            GameObject attacker = this.m_Attacker.Get(args);
            ReactionInput input = new ReactionInput(
                this.m_Direction.Get(args),
                (float) this.m_Force.Get(args)
            );

            character.Combat
                .RequestStance<MeleeStance>()
                .PlayReaction(attacker, input, this.m_Reaction, true);
            
            return DefaultResult;
        }
    }
}