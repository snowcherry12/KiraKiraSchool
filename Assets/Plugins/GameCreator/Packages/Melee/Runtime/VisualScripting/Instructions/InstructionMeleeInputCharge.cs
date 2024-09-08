using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Version(0, 0, 1)]
    
    [Title("Input Charge")]
    [Description("Queues a charging Melee input command on a Character")]

    [Category("Melee/Input/Input Charge")]
    
    [Parameter("Character", "The Character reference")]
    [Parameter("Key", "The Input key value")]

    [Keywords("Melee", "Combat")]
    [Image(typeof(IconMeleeInputCharge), ColorTheme.Type.Blue)]
    
    [Serializable]
    public class InstructionMeleeInputCharge : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();
        [SerializeField] private MeleeKey m_Key = MeleeKey.A;

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Input Charge[{this.m_Key}] on {this.m_Character}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return DefaultResult;

            character.Combat
                .RequestStance<MeleeStance>()
                .InputCharge(this.m_Key);
            
            return DefaultResult;
        }
    }
}