using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Title("Last Noise Position")]
    [Category("Perception/Last Noise Position")]
    
    [Image(typeof(IconNoise), ColorTheme.Type.Yellow, typeof(OverlayDot))]
    [Description("Returns the position where a Perception heard last a Noise identified by a tag")]

    [Serializable]
    public class GetPositionLastNoise : PropertyTypeGetPosition
    {
        [SerializeField]
        private PropertyGetGameObject m_Perception = GetGameObjectPerception.Create;

        [SerializeField]
        private PropertyGetString m_NoiseTag = GetStringId.Create("my-noise-tag");
        
        public override Vector3 Get(Args args)
        {
            Perception perception = this.m_Perception.Get<Perception>(args);
            string noiseTag = this.m_NoiseTag.Get(args);
            
            if (perception == null) return default;

            SensorHear sensorHear = perception.GetSensor<SensorHear>();
            return sensorHear?.GetPositionLastHeard(noiseTag) ?? default;
        }
        
        public static PropertyGetPosition Create() => new PropertyGetPosition(
            new GetPositionLastNoise()
        );

        public override string String => $"{this.m_Perception} Head {this.m_NoiseTag}";
    }
}