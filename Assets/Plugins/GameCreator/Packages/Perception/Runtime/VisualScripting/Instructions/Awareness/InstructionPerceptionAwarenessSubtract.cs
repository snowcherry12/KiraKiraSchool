using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Version(0, 0, 1)]
    
    [Title("Decrease Awareness")]
    [Description("Decreases the awareness of a target on a Perception component")]

    [Category("Perception/Awareness/Decrease Awareness")]
    
    [Parameter("Decrement", "The decreasing value of awareness")]

    [Keywords("Remove", "Less")]
    [Image(typeof(IconAwareness), ColorTheme.Type.Green, typeof(OverlayArrowDown))]
    
    [Serializable]
    public class InstructionPerceptionAwarenessSubtract : TInstructionPerceptionAwareness
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField]
        private PropertyGetDecimal m_Decrement = new PropertyGetDecimal(0.1f);
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Awareness on {this.m_Perception} - {this.m_Decrement}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            Perception perception = this.m_Perception.Get<Perception>(args);
            if (perception == null) return DefaultResult;
            
            GameObject target = this.m_Target.Get(args);
            if (target == null) return DefaultResult;

            float decrement = (float) this.m_Decrement.Get(args);
            
            perception.SubtractAwareness(target, decrement);
            return DefaultResult;
        }
    }
}