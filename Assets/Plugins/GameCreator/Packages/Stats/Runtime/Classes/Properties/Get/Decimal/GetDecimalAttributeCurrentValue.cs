using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Title("Attribute Value")]
    [Category("Stats/Attribute Value")]

    [Image(typeof(IconAttr), ColorTheme.Type.Blue)]
    [Description("The Attribute's current value of a game object's Traits component")]

    [Serializable]
    public class GetDecimalAttributeCurrentValue : PropertyTypeGetDecimal
    {
        [SerializeField] private PropertyGetGameObject m_Traits = GetGameObjectPlayer.Create();
        [SerializeField] private PropertyGetAttribute m_Attribute = new PropertyGetAttribute();

        public override double Get(Args args)
        {
            if (this.m_Attribute == null) return 0f;
            
            Traits traits = this.m_Traits.Get<Traits>(args);
            if (traits == null) return 0f;

            Attribute attribute = this.m_Attribute.Get(args);
            if (attribute == null) return 0f;
            
            return traits.RuntimeAttributes.Get(attribute.ID)?.Value ?? 0f;
        }

        public static PropertyGetDecimal Create => new PropertyGetDecimal(
            new GetDecimalAttributeCurrentValue()
        );

        public override string String => $"{this.m_Traits}[{this.m_Attribute}]";
    }
}