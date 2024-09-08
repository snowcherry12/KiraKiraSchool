using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Version(0, 0, 1)]
    
    [Title("Stop Blocking")]
    [Description("Attempts to stop blocking with the Melee stance")]

    [Category("Melee/Defense/Stop Blocking")]
    
    [Parameter("Character", "The Character that stops blocking")]

    [Keywords("Melee", "Combat", "Shield", "Parry", "Deflect", "Block")]
    [Image(typeof(IconShieldOutline), ColorTheme.Type.Red)]
    
    [Serializable]
    public class InstructionMeleeStopBlocking : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Stop Melee blocking on {this.m_Character}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return DefaultResult;

            character.Combat.Block.LowerGuard();
            return DefaultResult;
        }
    }
}