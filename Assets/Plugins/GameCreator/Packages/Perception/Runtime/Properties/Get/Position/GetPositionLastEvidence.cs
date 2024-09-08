using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Title("Last Evidence Noticed")]
    [Category("Perception/Last Evidence Noticed")]
    
    [Image(typeof(IconEvidence), ColorTheme.Type.Yellow, typeof(OverlayArrowRight))]
    [Description("Returns the position where a Perception saw the last Evidence")]

    [Serializable]
    public class GetPositionLastEvidence : PropertyTypeGetPosition
    {
        [SerializeField]
        private PropertyGetGameObject m_Perception = GetGameObjectPerception.Create;
        
        public override Vector3 Get(Args args)
        {
            Perception perception = this.m_Perception.Get<Perception>(args);
            if (perception == null) return default;

            return perception.LastNoticedEvidence != null
                ? perception.LastNoticedEvidence.transform.position
                : default;
        }
        
        public static PropertyGetPosition Create() => new PropertyGetPosition(
            new GetPositionLastEvidence()
        );

        public override string String => $"{this.m_Perception} Last Evidence";
    }
}