using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Stats
{
    [Title("Traits Description")]
    [Category("Stats/Traits Description")]

    [Image(typeof(IconTraits), ColorTheme.Type.Pink)]
    [Description("Returns the description of the Class associated with a Traits component")]
    
    [Serializable] [HideLabelsInEditor]
    public class GetStringTraitsDescription : PropertyTypeGetString
    {
        [SerializeField] protected PropertyGetGameObject m_Traits = GetGameObjectSelf.Create();

        public override string Get(Args args)
        {
            Traits traits = this.m_Traits.Get<Traits>(args);
            if (traits == null) return string.Empty;
            
            return traits.Class != null 
                ? traits.Class.GetDescription(args) 
                : string.Empty;
        }

        public override string String => this.m_Traits != null 
            ? $"{this.m_Traits}[Desc]" 
            : "(none)";
    }
}