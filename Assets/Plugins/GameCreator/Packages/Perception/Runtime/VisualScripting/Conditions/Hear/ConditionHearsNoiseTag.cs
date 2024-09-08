using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Title("Hears Noise Tag")]
    [Description("Checks whether the Perception component is hearing a Noise Tag")]

    [Category("Perception/Hear/Hears Noise Tag")]
    
    [Parameter("Perception", "The Perception component")]
    [Parameter("Noise Tag", "The Noise Tag to check")]
    [Parameter("Value", "The comparison to the noise value")]

    [Keywords("Sound", "Noise", "Tag", "Bell", "Intensity")]
    [Image(typeof(IconNoise), ColorTheme.Type.Yellow)]
    
    [Serializable]
    public class ConditionHearsNoiseTag : Condition
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField]
        private PropertyGetGameObject m_Perception = GetGameObjectPerception.Create;

        [SerializeField]
        private PropertyGetString m_NoiseTag = GetStringId.Create("my-noise-tag");

        [SerializeField]
        private CompareDouble m_Value = new CompareDouble();

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => 
            $"{this.m_Perception}[{this.m_NoiseTag}] Noise {this.m_Value}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Perception perception = this.m_Perception.Get<Perception>(args);
            if (perception == null) return false;

            SensorHear sensorHear = perception.GetSensor<SensorHear>();
            if (sensorHear == null) return false;

            float intensity = sensorHear.GetIntensity(this.m_NoiseTag.Get(args));
            return this.m_Value.Match(intensity, args);
        }
    }
}
