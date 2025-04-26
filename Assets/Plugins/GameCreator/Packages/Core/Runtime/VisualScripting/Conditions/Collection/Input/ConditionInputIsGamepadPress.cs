using System;
using GameCreator.Runtime.Common;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace GameCreator.Runtime.VisualScripting
{
    [Title("Is Gamepad Button Pressed")]
    [Description("Returns true if the Gamepad button is pressed during this frame")]

    [Category("Input/Is Gamepad Button Pressed")]
    
    [Parameter("Button", "The Gamepad button that is checked")]

    [Keywords("Button", "Down", "Key")]
    
    [Image(typeof(IconGamepad), ColorTheme.Type.Yellow, typeof(OverlayArrowLeft))]
    [Serializable]
    public class ConditionInputIsGamepadPress : Condition
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] protected GamepadButton m_Button = GamepadButton.South;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"{this.m_Button} just pressed";
        
        public GamepadButton Button
        {
            get => this.m_Button;
            set => this.m_Button = value;
        }
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            return Gamepad.current[this.m_Button].wasPressedThisFrame;
        }
    }
}
