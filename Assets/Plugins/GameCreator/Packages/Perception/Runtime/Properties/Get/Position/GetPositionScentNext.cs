using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Title("Follow Scent")]
    [Category("Perception/Follow Scent")]
    
    [Image(typeof(IconScent), ColorTheme.Type.Yellow, typeof(OverlayArrowRight))]
    [Description("Returns the position of the follow-up most intense scent position")]

    [Serializable]
    public class GetPositionScentNext : PropertyTypeGetPosition
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
            if (scentId == Scent.SCENT_ID_NONE) return default;
            
            Scent scent = SmellManager.Instance.GetScent(scentTag, scentId);
            if (scent == null || scent.NextScentId == Scent.SCENT_ID_NONE) return default;
            
            Scent nextScent = SmellManager.Instance.GetScent(scentTag, scent.NextScentId);
            return nextScent != null ? nextScent.transform.position : scent.transform.position;
        }

        public static PropertyGetPosition Create() => new PropertyGetPosition(
            new GetPositionScentNext()
        );
        
        public override string String => $"{this.m_Perception}[{this.m_ScentTag}] Next";
    }
}