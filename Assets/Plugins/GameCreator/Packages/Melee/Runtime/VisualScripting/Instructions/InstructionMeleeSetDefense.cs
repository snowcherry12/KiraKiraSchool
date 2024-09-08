using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Version(0, 0, 1)]
    
    [Title("Set Defense")]
    [Description("Sets the current defensive value of a Shield on a Character")]

    [Category("Melee/Defense/Set Defense")]
    
    [Parameter("Character", "The Character that has a defensive combat value")]
    [Parameter("Value", "The new defense value, clamped between 0 and the maximum defense value")]

    [Keywords("Melee", "Combat", "Shield", "Defense", "Block")]
    [Image(typeof(IconShieldSolid), ColorTheme.Type.TextLight, typeof(OverlayDot))]
    
    [Serializable]
    public class InstructionMeleeSetDefense : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();
        [SerializeField] private PropertyGetDecimal m_Value = GetDecimalDecimal.Create(99f);

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Defense of {this.m_Character} = {this.m_Value}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return DefaultResult;

            character.Combat.CurrentDefense = (float) this.m_Value.Get(args);
            return DefaultResult;
        }
    }
}