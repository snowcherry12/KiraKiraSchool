using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Version(0, 0, 1)]
    
    [Title("Start Blocking")]
    [Description("Attempts to start blocking with the Melee stance")]

    [Category("Melee/Defense/Start Blocking")]
    
    [Parameter("Character", "The Character that starts blocking")]

    [Keywords("Melee", "Combat", "Shield", "Parry", "Deflect", "Block")]
    [Image(typeof(IconShieldSolid), ColorTheme.Type.Green)]
    
    [Serializable]
    public class InstructionMeleeStartBlocking : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Start Melee blocking on {this.m_Character}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return DefaultResult;
            
            character.Combat.Block.RaiseGuard();
            return DefaultResult;
        }
    }
}