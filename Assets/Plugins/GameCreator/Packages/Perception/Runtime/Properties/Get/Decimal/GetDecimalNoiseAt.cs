using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Title("Noise At")]
    [Category("Perception/Noise At")]
    
    [Description("The noise value heard by a specific Perception component")]
    [Image(typeof(IconNoise), ColorTheme.Type.Green)]

    [Serializable]
    public class GetDecimalNoiseAt : PropertyTypeGetDecimal
    {
        [SerializeField] private PropertyGetGameObject m_Perception = GetGameObjectPerception.Create;

        [SerializeField] private CompareStringOrAny m_NoiseTag = new CompareStringOrAny(
            new PropertyGetString("my-noise-tag")
        );

        public override double Get(Args args)
        {
            Perception perception = this.m_Perception.Get<Perception>(args);
            if (perception == null) return 0f;

            SensorHear sensorHear = perception.GetSensor<SensorHear>();
            if (sensorHear == null) return 0f;
            
            return this.m_NoiseTag.Any
                ? sensorHear.HighestIntensity
                : sensorHear.GetIntensity(this.m_NoiseTag.Get(args));
        }

        public override string String => this.m_NoiseTag.Any
            ? $"{this.m_Perception} Noise"
            : $"{this.m_Perception}[{this.m_NoiseTag}] Noise";
    }
}