using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Title("Scent At")]
    [Category("Perception/Scent At")]
    
    [Description("The specific scent intensity caught by a Perception component")]
    [Image(typeof(IconScent), ColorTheme.Type.Green)]

    [Serializable]
    public class GetDecimalScentAt : PropertyTypeGetDecimal
    {
        [SerializeField] private PropertyGetGameObject m_Perception = GetGameObjectPerception.Create;

        [SerializeField] private CompareStringOrAny m_ScentTag = new CompareStringOrAny(
            new PropertyGetString("my-scent-tag")
        );

        public override double Get(Args args)
        {
            Perception perception = this.m_Perception.Get<Perception>(args);
            if (perception == null) return 0f;

            SensorSmell sensorSmell = perception.GetSensor<SensorSmell>();
            if (sensorSmell == null) return 0f;
            
            return this.m_ScentTag.Any
                ? sensorSmell.GetIntensity()
                : sensorSmell.GetIntensity(this.m_ScentTag.Get(args));
        }

        public override string String => this.m_ScentTag.Any
            ? $"{this.m_Perception} Scent"
            : $"{this.m_Perception}[{this.m_ScentTag}] Scent";
    }
}