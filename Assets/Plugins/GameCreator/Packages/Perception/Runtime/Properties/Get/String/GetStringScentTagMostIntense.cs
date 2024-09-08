using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Title("Main Scent Tag")]
    [Category("Perception/Main Scent Tag")]
    
    [Description("Returns the scent tag with the highest intensity smelled by a Perception")]
    [Example("If the Perception component does not smell any scent it returns an empty string")]
    
    [Image(typeof(IconScent), ColorTheme.Type.Yellow)]

    [Serializable]
    public class GetStringScentTagMostIntense : PropertyTypeGetString
    {
        [SerializeField] private PropertyGetGameObject m_Perception = GetGameObjectPerception.Create;

        public override string Get(Args args)
        {
            Perception perception = this.m_Perception.Get<Perception>(args);
            if (perception == null) return string.Empty;

            SensorSmell sensorSmell = perception.GetSensor<SensorSmell>();
            return sensorSmell?.GetMostIntenseScent();
        }
        
        public override string String => $"{this.m_Perception} Main Scent";
    }
}