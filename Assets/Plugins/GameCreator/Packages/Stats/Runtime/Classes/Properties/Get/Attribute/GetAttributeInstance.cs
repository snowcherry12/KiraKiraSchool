using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Title("Attribute")]
    [Category("Attribute")]
    
    [Image(typeof(IconAttr), ColorTheme.Type.Blue)]
    [Description("A direct reference to the Attribute value")]

    [Serializable] [HideLabelsInEditor]
    public class GetAttributeInstance : PropertyTypeGetAttribute
    {
        [SerializeField] protected Attribute m_Attribute;

        public override Attribute Get(Args args) => this.m_Attribute;
        public override Attribute Get(GameObject gameObject) => this.m_Attribute;

        public override string String => this.m_Attribute != null
            ? $"{this.m_Attribute.name}"
            : "(none)";
    }
}