using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Version(0, 1, 1)]

    [Title("Active Human Shooter IK")]
    [Description("Changes the active state of the Human Shooter rig IK")]

    [Category("Characters/IK/Active Human Shooter IK")]

    [Parameter("Character", "The character target")]
    [Parameter("Active", "Whether the IK system is active or not")]

    [Keywords("Inverse", "Kinematics", "IK")]
    [Image(typeof(IconIK), ColorTheme.Type.Blue)]

    [Serializable]
    public class InstructionShooterIKHumanShooterActive : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] 
        private PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();

        [SerializeField] 
        private PropertyGetBool m_Active = GetBoolValue.Create(false);
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override string Title => $"Human Shooter IK of {this.m_Character} = {this.m_Active}";

        // RUN METHOD: ----------------------------------------------------------------------------
        
        protected override Task Run(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return DefaultResult;

            if (character.IK.HasRig<RigShooterHuman>())
            {
                bool state = this.m_Active.Get(args);
                character.IK.GetRig<RigShooterHuman>().IsActive = state;
            }
            
            return DefaultResult;
        }
    }
}