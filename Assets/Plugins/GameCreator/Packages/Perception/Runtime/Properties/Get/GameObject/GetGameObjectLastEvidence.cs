using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Title("Last Evidence Noticed")]
    [Category("Perception/Last Evidence Noticed")]
    
    [Image(typeof(IconEvidence), ColorTheme.Type.Yellow, typeof(OverlayArrowRight))]
    [Description("Returns the last Evidence game object noticed by a Perception component")]

    [Serializable]
    public class GetGameObjectLastEvidence : PropertyTypeGetGameObject
    {
        [SerializeField]
        private PropertyGetGameObject m_Perception = GetGameObjectPerception.Create;

        public override GameObject Get(Args args)
        {
            Perception perception = this.m_Perception.Get<Perception>(args);
            return perception != null ? perception.LastNoticedEvidence : null;
        }

        public static PropertyGetGameObject Create() => new PropertyGetGameObject(
            new GetGameObjectLastEvidence()
        );
        
        public override string String => $"{this.m_Perception} Last Evidence";
    }
}