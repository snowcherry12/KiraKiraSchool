using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Title("Last Seen Position")]
    [Category("Perception/Last Seen Position")]
    
    [Image(typeof(IconEye), ColorTheme.Type.Yellow, typeof(OverlayArrowRight))]
    [Description("Returns the position where a Perception saw last the targeted object")]

    [Serializable]
    public class GetPositionLastSeen : PropertyTypeGetPosition
    {
        [SerializeField]
        private PropertyGetGameObject m_Perception = GetGameObjectPerception.Create;

        [SerializeField]
        private PropertyGetGameObject m_Target = GetGameObjectPlayer.Create();
        
        public override Vector3 Get(Args args)
        {
            Perception perception = this.m_Perception.Get<Perception>(args);
            GameObject target = this.m_Target.Get(args);
            
            if (perception == null) return default;
            if (target == null) return default;

            SensorSee sensorSee = perception.GetSensor<SensorSee>();
            return sensorSee?.GetPositionLastSeen(target) ?? default;
        }
        
        public static PropertyGetPosition Create() => new PropertyGetPosition(
            new GetPositionLastSeen()
        );

        public override string String => $"{this.m_Perception} Seen {this.m_Target}";
    }
}