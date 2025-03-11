using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Version(0, 0, 1)]
    
    [Title("Character Lean")]
    [Description("Leans a character towards either side")]

    [Category("Shooter/Sights/Character Lean")]

    [Parameter("Character", "The Character reference with a Weapon equipped")]
    [Parameter("Angle", "How much (in degrees) the Character leans")]
    [Parameter("Speed", "How fast the Character leans")]
    
    [Keywords("Shooter", "Peek", "Snap", "Corner", "Cover")]
    [Image(typeof(IconRotationRoll), ColorTheme.Type.Red)]
    
    [Example("The Character must be a Humanoid")]
    
    [Serializable]
    public class InstructionShooterLean : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField]
        private PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();

        [SerializeField] private PropertyGetDecimal m_Angle = GetDecimalDecimal.Create(30f);
        [SerializeField] private PropertyGetDecimal m_Speed = GetDecimalDecimal.Create(0.1);

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"{this.m_Character} Lean = {this.m_Angle}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return DefaultResult;

            RigShooterHuman humanShooter = character.IK.RequireRig<RigShooterHuman>();
            humanShooter.LeanAmount = (float) this.m_Angle.Get(args);
            humanShooter.LeanDecay = (float) this.m_Speed.Get(args);
            
            return DefaultResult;
        }
    }
}