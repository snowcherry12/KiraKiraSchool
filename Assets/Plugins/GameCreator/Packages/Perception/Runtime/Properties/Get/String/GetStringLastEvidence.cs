using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Title("Last Evidence Tag")]
    [Category("Perception/Last Evidence Tag")]
    
    [Description("Returns the last Evidence tag noticed by a Perception component")]
    [Image(typeof(IconEvidence), ColorTheme.Type.Yellow)]

    [Serializable]
    public class GetStringLastEvidence : PropertyTypeGetString
    {
        [SerializeField] private PropertyGetGameObject m_Perception = GetGameObjectPerception.Create;

        public override string Get(Args args)
        {
            Perception perception = this.m_Perception.Get<Perception>(args);
            
            if (perception == null) return string.Empty;
            if (perception.LastNoticedEvidence == null) return string.Empty;

            Evidence evidence = perception.LastNoticedEvidence.Get<Evidence>();
            return evidence != null ? evidence.GetTag(args.Target) : string.Empty;
        }
        
        public override string String => $"{this.m_Perception} Last Evidence Tag";
    }
}