using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Title("Current Scent")]
    [Category("Perception/Current Scent")]
    
    [Image(typeof(IconScent), ColorTheme.Type.Yellow, typeof(OverlayDot))]
    [Description("Returns the Scent game object of the highest intensity Scent smelled")]

    [Serializable]
    public class GetGameObjectScentCurrent : PropertyTypeGetGameObject
    {
        [SerializeField]
        private PropertyGetGameObject m_Perception = GetGameObjectPerception.Create;

        [SerializeField]
        private PropertyGetString m_ScentTag = GetStringId.Create("my-scent-tag");

        public override GameObject Get(Args args)
        {
            Perception perception = this.m_Perception.Get<Perception>(args);
            if (perception == null) return null;
            
            SensorSmell sensorSmell = perception.GetSensor<SensorSmell>();
            string scentTag = this.m_ScentTag.Get(args);
            
            int scentId = sensorSmell.GetScentId(scentTag);
            if (scentId == Scent.SCENT_ID_NONE) return null;
            
            Scent scent = SmellManager.Instance.GetScent(scentTag, scentId);
            return scent != null ? scent.gameObject : null;
        }

        public static PropertyGetGameObject Create() => new PropertyGetGameObject(
            new GetGameObjectScentCurrent()
        );
        
        public override string String => $"{this.m_Perception}[{this.m_ScentTag}]";
    }
}