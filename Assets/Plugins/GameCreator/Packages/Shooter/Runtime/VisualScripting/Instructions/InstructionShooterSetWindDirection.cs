using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Version(0, 0, 1)]
    
    [Title("Change Wind Direction")]
    [Description("Changes the wind direction keeping its current magnitude")]

    [Category("Shooter/Wind/Change Wind Direction")]
    
    [Parameter("Direction", "The new direction of the wind")]

    [Keywords("Wind", "Drift", "Force", "Air", "Storm")]
    [Image(typeof(IconWind), ColorTheme.Type.Green, typeof(OverlayDot))]
    
    [Serializable]
    public class InstructionShooterSetWindDirection : Instruction
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField]
        private PropertyGetDirection m_Direction = GetDirectionConstantRight.Create;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Set Wind Direction = {this.m_Direction}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            Vector3 direction = this.m_Direction.Get(args);
            WindManager.Instance.Direction = direction;
            
            return DefaultResult;
        }
    }
}