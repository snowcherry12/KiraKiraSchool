using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Dialogue
{
    [Title("Actor Name")]
    [Category("Dialogue/Actor Name")]
    
    [Image(typeof(IconBust), ColorTheme.Type.Yellow)]
    [Description("Returns the name of the Actor asset")]
    
    [Serializable]
    public class GetStringActorName : PropertyTypeGetString
    {
        [SerializeField] private Actor m_Actor;
        
        public override string Get(Args args) => this.m_Actor != null 
            ? this.m_Actor.GetName(args) 
            : string.Empty;

        public static PropertyGetString Create => new PropertyGetString(
            new GetStringActorName()
        );

        public override string String => string.Format(
            "{0} Name", 
            this.m_Actor != null ? this.m_Actor.name : "(none)"
        );
    }
}