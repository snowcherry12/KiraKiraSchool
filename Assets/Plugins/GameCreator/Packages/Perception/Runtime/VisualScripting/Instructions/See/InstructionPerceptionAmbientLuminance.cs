using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Version(0, 0, 1)]
    
    [Title("Change Global Luminance")]
    [Description("Changes the global ambient Luminance value")]

    [Category("Perception/See/Change Global Luminance")]
    
    [Parameter("Luminance", "The new value for the global ambient Luminance")]
    [Parameter("Transition", "A set of options that allow to change the value over time")]

    [Keywords("Light", "Bright", "Dark", "Dim", "Night", "Sun")]
    [Image(typeof(IconLuminance), ColorTheme.Type.Yellow)]
    
    [Serializable]
    public class InstructionPerceptionAmbientLuminance : Instruction
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField]
        private PropertyGetDecimal m_Luminance = GetDecimalDecimal.Create(1f);
        
        [SerializeField] 
        private Transition m_Transition = new Transition();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Global Luminance = {this.m_Luminance}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override async Task Run(Args args)
        {
            float valueSource = LuminanceManager.Instance.AmbientIntensity;
            float valueTarget = (float) this.m_Luminance.Get(args);

            ITweenInput tween = new TweenInput<float>(
                valueSource,
                valueTarget,
                this.m_Transition.Duration,
                (a, b, t) => LuminanceManager.Instance.AmbientIntensity = Mathf.Lerp(a, b, t),
                Tween.GetHash(typeof(LuminanceManager), "global-ambient-luminance"),
                this.m_Transition.EasingType,
                this.m_Transition.Time
            );
            
            Tween.To(LuminanceManager.Instance.gameObject, tween);
            if (this.m_Transition.WaitToComplete) await this.Until(() => tween.IsFinished);
        }
    }
}