using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Title("Can Hear Noise")]
    [Description("Checks whether the Perception component can hear a new Noise stimulus")]

    [Category("Perception/Hear/Can Hear Noise")]
    
    [Parameter("Perception", "The Perception component")]
    [Parameter("Position", "The position of the Noise stimulus")]
    [Parameter("Radius", "The radius of the Noise stimulus")]
    [Parameter("Intensity", "The intensity of the Noise stimulus")]

    [Keywords("Sound", "Noise", "Bell", "Intensity", "Stimulus")]
    [Image(typeof(IconNoise), ColorTheme.Type.Green)]
    
    [Serializable]
    public class ConditionCanHearNoise : Condition
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField]
        private PropertyGetGameObject m_Perception = GetGameObjectPerception.Create;

        [SerializeField] private PropertyGetPosition m_Position = GetPositionCharactersPlayer.Create;
        [SerializeField] private PropertyGetDecimal m_Radius = GetDecimalDecimal.Create(10f);
        [SerializeField] private PropertyGetDecimal m_Intensity = GetDecimalConstantOne.Create;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"{this.m_Perception} hear Noise at {this.m_Position}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Perception perception = this.m_Perception.Get<Perception>(args);
            if (perception == null) return false;

            SensorHear sensorHear = perception.GetSensor<SensorHear>();
            if (sensorHear == null) return false;

            StimulusNoise stimulus = new StimulusNoise(
                string.Empty,
                this.m_Position.Get(args),
                (float) this.m_Radius.Get(args),
                (float) this.m_Intensity.Get(args)
            );
            
            return sensorHear.CanHear(stimulus);
        }
    }
}
