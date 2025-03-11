using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Version(0, 0, 1)]
    
    [Title("Change Wind Magnitude")]
    [Description("Changes the force of the Wind keeping its current direction")]

    [Category("Shooter/Wind/Change Wind Magnitude")]
    
    [Parameter("Force", "The new force of the wind")]

    [Keywords("Wind", "Drift", "Force", "Air", "Storm")]
    [Image(typeof(IconWind), ColorTheme.Type.Green, typeof(OverlayArrowRight))]
    
    [Serializable]
    public class InstructionShooterSetWindMagnitude : Instruction
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField]
        private PropertyGetDecimal m_Force = GetDecimalDecimal.Create(50);
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Set Wind Magnitude = {this.m_Force}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            float force = (float) this.m_Force.Get(args);
            WindManager.Instance.Magnitude = force;
            
            return DefaultResult;
        }
    }
}