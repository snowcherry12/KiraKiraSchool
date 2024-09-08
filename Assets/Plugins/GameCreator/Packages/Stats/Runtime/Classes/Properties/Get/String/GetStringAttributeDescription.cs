using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Stats
{
    [Title("Attribute Description")]
    [Category("Stats/Attribute Description")]

    [Image(typeof(IconAttr), ColorTheme.Type.Blue)]
    [Description("Returns the description text of an Attribute")]
    
    [Serializable]
    public class GetStringAttributeDescription : PropertyTypeGetString
    {
        [SerializeField] protected PropertyGetAttribute m_Attribute = new PropertyGetAttribute();

        public override string Get(Args args)
        {
            Attribute attribute = this.m_Attribute.Get(args);
            if (attribute == null) return string.Empty;
            
            return attribute != null
                ? attribute.GetDescription(args)
                : string.Empty;
        }

        public override string String => this.m_Attribute.ToString();
    }
}