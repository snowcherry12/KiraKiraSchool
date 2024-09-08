using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Stats
{
    [Title("Attribute Name")]
    [Category("Stats/Attribute Name")]

    [Image(typeof(IconAttr), ColorTheme.Type.Blue)]
    [Description("Returns the name of a Attribute")]
    
    [Serializable]
    public class GetStringAttributeName : PropertyTypeGetString
    {
        [SerializeField] protected PropertyGetAttribute m_Attribute = new PropertyGetAttribute();

        public override string Get(Args args)
        {
            Attribute attribute = this.m_Attribute.Get(args);
            
            return attribute != null
                ? attribute.GetName(args)
                : string.Empty;
        }
        
        public override string String => this.m_Attribute.ToString();
    }
}