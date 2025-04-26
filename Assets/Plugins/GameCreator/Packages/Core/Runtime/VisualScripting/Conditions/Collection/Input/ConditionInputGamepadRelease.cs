using System;
using GameCreator.Runtime.Common;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace GameCreator.Runtime.VisualScripting
{
    [Title("Is Gamepad Button Released")]
    [Description("Returns true if the Gamepad button is released during this frame")]

    [Category("Input/Is Gamepad Button Released")]
    
    [Parameter("Button", "The Gamepad button that is checked")]

    [Keywords("Key", "Up")]
    
    [Image(typeof(IconGamepad), ColorTheme.Type.Green, typeof(OverlayArrowRight))]
    [Serializable]
    public class ConditionInputGamepadRelease : Condition
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] protected GamepadButton m_Button = GamepadButton.South;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"{this.m_Button} just released";
        
        public GamepadButton Button
        {
            get => this.m_Button;
            set => this.m_Button = value;
        }
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            return Gamepad.current[this.m_Button].wasReleasedThisFrame;
        }
    }
}
