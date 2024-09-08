using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Stats
{
    [Title("Attribute Color")]
    [Category("Stats/Attribute Color")]
    
    [Image(typeof(IconAttr), ColorTheme.Type.Blue)]
    [Description("Returns the Color value of an Attribute")]

    [Serializable] [HideLabelsInEditor]
    public class GetColorAttribute : PropertyTypeGetColor
    {
        [SerializeField] private PropertyGetAttribute m_Attribute = new PropertyGetAttribute();

        public override Color Get(Args args)
        {
            Attribute attribute = this.m_Attribute.Get(args);
            return attribute != null
                ? attribute.Color
                : Color.black;
        }

        public override string String => this.m_Attribute.ToString();
    }
}