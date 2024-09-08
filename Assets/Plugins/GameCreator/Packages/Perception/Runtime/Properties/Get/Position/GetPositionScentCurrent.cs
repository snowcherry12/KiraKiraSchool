using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Title("Current Scent")]
    [Category("Perception/Current Scent")]
    
    [Image(typeof(IconScent), ColorTheme.Type.Yellow, typeof(OverlayDot))]
    [Description("Returns the position of the highest intensity Scent smelled")]

    [Serializable]
    public class GetPositionScentCurrent : PropertyTypeGetPosition
    {
        [SerializeField]
        private PropertyGetGameObject m_Perception = GetGameObjectPerception.Create;

        [SerializeField]
        private PropertyGetString m_ScentTag = GetStringId.Create("my-scent-tag");

        public override Vector3 Get(Args args)
        {
            Perception perception = this.m_Perception.Get<Perception>(args);
            if (perception == null) return default;
            
            SensorSmell sensorSmell = perception.GetSensor<SensorSmell>();
            string scentTag = this.m_ScentTag.Get(args);
            
            int scentId = sensorSmell.GetScentId(scentTag);
            Scent scent = SmellManager.Instance.GetScent(scentTag, scentId);
            
            return scent != null ? scent.transform.position : default;
        }

        public static PropertyGetPosition Create() => new PropertyGetPosition(
            new GetPositionScentCurrent()
        );

        public override string String => $"{this.m_Perception}[{this.m_ScentTag}]";
    }
}