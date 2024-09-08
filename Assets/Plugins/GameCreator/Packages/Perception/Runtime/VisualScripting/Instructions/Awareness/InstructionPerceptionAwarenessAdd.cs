using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Version(0, 0, 1)]
    
    [Title("Increase Awareness")]
    [Description("Increases the awareness of a target on a Perception component")]

    [Category("Perception/Awareness/Increase Awareness")]
    
    [Parameter("Increment", "The increment value of awareness")]
    [Parameter("Maximum Level", "The maximum Awareness this increment can reach")]
    
    [Example(
        "Use the Maximum Level if you want to increase the Awareness up to a certain threshold. " +
        "For example, throwing a bottle nearby will make guards suspicious but never reach the " +
        "state of Aware"
    )]

    [Keywords("Add", "Sum")]
    [Image(typeof(IconAwareness), ColorTheme.Type.Green, typeof(OverlayArrowUp))]
    
    [Serializable]
    public class InstructionPerceptionAwarenessAdd : TInstructionPerceptionAwareness
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField]
        private PropertyGetDecimal m_Increment = new PropertyGetDecimal(0.1f);
        
        [SerializeField]
        private PropertyGetDecimal m_MaxLevel = new PropertyGetDecimal(1f);
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Awareness on {this.m_Perception} + {this.m_Increment}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            Perception perception = this.m_Perception.Get<Perception>(args);
            if (perception == null) return DefaultResult;
            
            GameObject target = this.m_Target.Get(args);
            if (target == null) return DefaultResult;

            float increment = (float) this.m_Increment.Get(args);
            float maxLevel = (float) this.m_MaxLevel.Get(args);
            
            perception.AddAwareness(target, increment, maxLevel);
            return DefaultResult;
        }
    }
}