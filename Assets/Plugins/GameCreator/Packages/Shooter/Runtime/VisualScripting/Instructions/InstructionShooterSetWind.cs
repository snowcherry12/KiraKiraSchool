using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Version(0, 0, 1)]
    
    [Title("Change Wind")]
    [Description("Changes the Direction and Force of the Wind")]

    [Category("Shooter/Wind/Change Wind")]
    
    [Parameter("Direction", "The new normalized direction of the wind in world space")]
    [Parameter("Force", "The new force of the wind")]

    [Keywords("Wind", "Drift", "Force", "Air", "Storm")]
    [Image(typeof(IconWind), ColorTheme.Type.Green)]
    
    [Serializable]
    public class InstructionShooterSetWind : Instruction
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField]
        private PropertyGetDirection m_Direction = GetDirectionConstantRight.Create;

        [SerializeField]
        private PropertyGetDecimal m_Force = GetDecimalDecimal.Create(50);
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Set Wind = {this.m_Direction} with {this.m_Force}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            Vector3 direction = this.m_Direction.Get(args);
            float force = (float) this.m_Force.Get(args);

            WindManager.Instance.Wind = direction.normalized * force;
            return DefaultResult;
        }
    }
}