using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Version(0, 0, 1)]
    
    [Title("Set Buffer Window")]
    [Description("Sets the maximum time for an input to register before it can be executed")]

    [Category("Melee/Input/Set Buffer Window")]
    
    [Parameter("Character", "The Character reference")]
    [Parameter("Buffer Window", "The time of the Buffer Window, in seconds")]

    [Keywords("Melee", "Combat", "Buffer", "Window")]
    [Image(typeof(IconTimer), ColorTheme.Type.Yellow)]
    
    [Serializable]
    public class InstructionMeleeInputSetBuffer : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();
        [SerializeField] private PropertyGetDecimal m_BufferWindow = GetDecimalDecimal.Create(0.5f);

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Input {this.m_Character} Buffer = {this.m_BufferWindow}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return DefaultResult;

            float bufferWindow = (float) this.m_BufferWindow.Get(args);
            character.Combat.RequestStance<MeleeStance>().BufferWindow = bufferWindow;

            return DefaultResult;
        }
    }
}