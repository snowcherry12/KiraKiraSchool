using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameCreator.Runtime.Perception
{
    [Version(0, 0, 1)]
    
    [Title("Emit Noise")]
    [Description("Emits a Noise Stimulus that other Perception components can process")]

    [Category("Perception/Hear/Emit Noise")]
    
    [Parameter("Position", "The center of the noise emitted")]
    [Parameter("Radius", "The radius of the noise emitted")]
    
    [Parameter("Tag", "The name identifier of the noise")]
    [Parameter("Intensity", "The strength value used for the noise emitted")]

    [Keywords("Sound", "Noise", "Distract", "Alert", "Aural", "Hear")]
    [Image(typeof(IconNoise), ColorTheme.Type.Green)]
    
    [Serializable]
    public class InstructionPerceptionEmitNoise : Instruction
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField]
        private PropertyGetPosition m_Position = GetPositionCharactersPlayer.Create;

        [SerializeField]
        private PropertyGetDecimal m_Radius = GetDecimalDecimal.Create(10f);

        [SerializeField]
        private PropertyGetString m_Tag = GetStringId.Create("my-noise-tag");

        [SerializeField]
        private PropertyGetDecimal m_Intensity = GetDecimalDecimal.Create(0.5f);
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        private readonly List<ISpatialHash> m_ListPerceptions = new List<ISpatialHash>();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Noise {this.m_Tag} of {this.m_Intensity}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            Vector3 position = this.m_Position.Get(args);
            float radius = (float) this.m_Radius.Get(args);
            
            string tag = this.m_Tag.Get(args);
            float intensity = (float) this.m_Intensity.Get(args);
            
            HearManager.Instance.AddGizmoNoise(position, radius);
            SpatialHashPerception.Find(position, radius, this.m_ListPerceptions);
            
            foreach (ISpatialHash spatialHash in this.m_ListPerceptions)
            {
                Perception perception = spatialHash as Perception;
                if (perception == null) continue;

                 SensorHear sensorHear = perception.GetSensor<SensorHear>();
                 if (sensorHear == null) continue;

                 StimulusNoise stimulus = new StimulusNoise(
                     tag,
                     position,
                     radius,
                     intensity
                 );
                 
                 sensorHear.OnReceiveNoise(stimulus);
            }
            
            return DefaultResult;
        }
    }
}