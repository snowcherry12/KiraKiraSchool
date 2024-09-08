using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameCreator.Runtime.Perception
{
    [Version(0, 0, 1)]
    
    [Title("Set Dissipation")]
    [Description("Changes the global ambient Dissipation value")]

    [Category("Perception/Smell/Set Dissipation")]
    
    [Parameter("Dissipation", "The new dissipation value. Values over zero reduce the smell dispel duration")]
    [Parameter("Transition", "A set of options that allow to change the value over time")]
    
    [Example(
        "Dissipation values increase the time a scent dispels. A value of 0 means it doesn't " +
        "affect the dispel time. A value of 1 means it doubles the time it takes to dispel it"
    )]

    [Keywords("Odor", "Smell", "Scent", "Nose", "Wind")]
    [Image(typeof(IconDissipation), ColorTheme.Type.Yellow)]
    
    [Serializable]
    public class InstructionPerceptionDissipation : Instruction
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField]
        private PropertyGetDecimal m_Dissipation = GetDecimalDecimal.Create(0.75f);
        
        [SerializeField] 
        private Transition m_Transition = new Transition();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Dissipation = {this.m_Dissipation}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override async Task Run(Args args)
        {
            float valueSource = SmellManager.Instance.Dissipation;
            float valueTarget = (float) this.m_Dissipation.Get(args);

            ITweenInput tween = new TweenInput<float>(
                valueSource,
                valueTarget,
                this.m_Transition.Duration,
                (a, b, t) => SmellManager.Instance.Dissipation = Mathf.Lerp(a, b, t),
                Tween.GetHash(typeof(SmellManager), "dissipation"),
                this.m_Transition.EasingType,
                this.m_Transition.Time
            );
            
            Tween.To(SmellManager.Instance.gameObject, tween);
            if (this.m_Transition.WaitToComplete) await this.Until(() => tween.IsFinished);
        }
    }
}