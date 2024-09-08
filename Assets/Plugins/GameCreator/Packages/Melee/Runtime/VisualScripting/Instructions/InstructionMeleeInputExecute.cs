using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Version(0, 0, 1)]
    
    [Title("Input Execute")]
    [Description("Queues an execution Melee input command on a particular Character")]

    [Category("Melee/Input/Input Execute")]
    
    [Parameter("Character", "The Character reference")]
    [Parameter("Key", "The Input key value")]

    [Keywords("Melee", "Combat")]
    [Image(typeof(IconMeleeInputExecute), ColorTheme.Type.Green)]
    
    [Serializable]
    public class InstructionMeleeInputExecute : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();
        [SerializeField] private MeleeKey m_Key = MeleeKey.A;

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Input Execute[{this.m_Key}] on {this.m_Character}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return DefaultResult;

            character.Combat
                .RequestStance<MeleeStance>()
                .InputExecute(this.m_Key);
            
            return DefaultResult;
        }
    }
}