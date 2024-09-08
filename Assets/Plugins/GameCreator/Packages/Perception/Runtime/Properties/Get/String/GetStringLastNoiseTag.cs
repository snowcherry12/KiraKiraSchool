using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Title("Last Noise Tag")]
    [Category("Perception/Last Noise Tag")]
    
    [Description("Returns the last Noise tag heard by a Perception")]
    [Image(typeof(IconNoise), ColorTheme.Type.Yellow)]

    [Serializable]
    public class GetStringLastNoiseTag : PropertyTypeGetString
    {
        [SerializeField] private PropertyGetGameObject m_Perception = GetGameObjectPerception.Create;

        public override string Get(Args args)
        {
            Perception perception = this.m_Perception.Get<Perception>(args);
            if (perception == null) return string.Empty;

            SensorHear sensorHear = perception.GetSensor<SensorHear>();
            return sensorHear?.LastNoiseReceived.Tag ?? string.Empty;
        }
        
        public override string String => $"{this.m_Perception} Last Noise tag";
    }
}