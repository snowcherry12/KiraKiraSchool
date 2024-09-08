using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Version(0, 0, 1)]
    
    [Title("Global Din")]
    [Description("Changes the Global ambient din value")]

    [Category("Perception/Hear/Global Din")]
    
    [Parameter("Din", "The new value for the ambient background noise")]
    [Parameter("Transition", "A set of options that allow to change the value over time")]

    [Keywords("Sound", "Noise", "Ambient", "Aural", "Hear", "Deaf")]
    [Image(typeof(IconStorm), ColorTheme.Type.Yellow)]
    
    [Serializable]
    public class InstructionPerceptionGlobalDin : Instruction
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField]
        private PropertyGetDecimal m_Din = GetDecimalDecimal.Create(0.75f);
        
        [SerializeField] 
        private Transition m_Transition = new Transition();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Global Din = {this.m_Din}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override async Task Run(Args args)
        {
            float valueSource = HearManager.Instance.GlobalDin;
            float valueTarget = (float) this.m_Din.Get(args);

            ITweenInput tween = new TweenInput<float>(
                valueSource,
                valueTarget,
                this.m_Transition.Duration,
                (a, b, t) => HearManager.Instance.GlobalDin = Mathf.Lerp(a, b, t),
                Tween.GetHash(typeof(HearManager), "global-din"),
                this.m_Transition.EasingType,
                this.m_Transition.Time
            );
            
            Tween.To(HearManager.Instance.gameObject, tween);
            if (this.m_Transition.WaitToComplete) await this.Until(() => tween.IsFinished);
        }
    }
}