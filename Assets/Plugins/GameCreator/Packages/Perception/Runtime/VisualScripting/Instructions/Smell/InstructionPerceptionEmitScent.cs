using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Version(0, 0, 1)]
    
    [Title("Emit Scent")]
    [Description("Emits a Scent Stimulus that other Perception components can process")]

    [Category("Perception/Smell/Emit Scent")]
    
    [Parameter("Source", "The game object emitting the scent")]
    [Parameter("Dispel Duration", "The seconds it takes for the odor to fade out")]
    [Parameter("Diffusion Rate", "The growth factor of the smell per second")]
    
    [Parameter("Tag", "The name identifier of the noise")]
    [Parameter("Intensity", "The strength value used for the noise emitted")]

    [Keywords("Odor", "Smell", "Distract", "Alert", "Diffuse", "Dispel")]
    [Image(typeof(IconScent), ColorTheme.Type.Green)]
    
    [Serializable]
    public class InstructionPerceptionEmitScent : Instruction
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField]
        private PropertyGetGameObject m_Source = GetGameObjectPlayer.Create();
        
        [SerializeField]
        private PropertyGetString m_Tag = GetStringId.Create("my-scent-tag");

        [SerializeField]
        private PropertyGetDecimal m_Duration = GetDecimalDecimal.Create(5f);

        [SerializeField]
        private PropertyGetDecimal m_Intensity = GetDecimalConstantOne.Create;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Scent {this.m_Tag} of {this.m_Intensity}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            GameObject source = this.m_Source.Get(args);
            float duration = (float) this.m_Duration.Get(args);
            
            string tag = this.m_Tag.Get(args);
            float intensity = (float) this.m_Intensity.Get(args);
            
            SmellManager.Instance.Emit(source, tag, duration, intensity);
            return DefaultResult;
        }
    }
}