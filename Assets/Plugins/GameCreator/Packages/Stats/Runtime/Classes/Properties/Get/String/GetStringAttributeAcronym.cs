using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Stats
{
    [Title("Attribute Acronym")]
    [Category("Stats/Attribute Acronym")]

    [Image(typeof(IconAttr), ColorTheme.Type.Blue)]
    [Description("Returns the acronym of an Attribute")]
    
    [Serializable]
    public class GetStringAttributeAcronym : PropertyTypeGetString
    {
        [SerializeField] protected PropertyGetAttribute m_Attribute = new PropertyGetAttribute();

        public override string Get(Args args)
        {
            Attribute attribute = this.m_Attribute.Get(args);
            return attribute != null
                ? attribute.GetAcronym(args)
                : string.Empty;
        }

        public override string String => this.m_Attribute.ToString();
    }
}