using System;
using GameCreator.Runtime.Common;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace GameCreator.Runtime.VisualScripting
{
    [Title("Is Gamepad Button Held Down")]
    [Description("Returns true if the Gamepad button is being held down this frame")]

    [Category("Input/Is Gamepad Button Held Down")]
    
    [Parameter("Button", "The Gamepad button that is checked")]

    [Keywords("Key", "Active", "Down", "Press")]
    
    [Image(typeof(IconGamepad), ColorTheme.Type.Blue, typeof(OverlayDot))]
    [Serializable]
    public class ConditionInputGamepadHeldDown : Condition
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] protected GamepadButton m_Button = GamepadButton.South;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"{this.m_Button} held down";
        
        public GamepadButton Button
        {
            get => this.m_Button;
            set => this.m_Button = value;
        }
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            return Gamepad.current[this.m_Button].isPressed;
        }
    }
}
