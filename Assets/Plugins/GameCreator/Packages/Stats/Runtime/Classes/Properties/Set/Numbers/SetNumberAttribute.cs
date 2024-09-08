using System;
using GameCreator.Runtime.Characters;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Stats
{
    [Title("Attribute Value")]
    [Category("Stats/Attribute Value")]
    
    [Description("Sets the value of an Attribute on a game object's Traits component")]
    [Image(typeof(IconAttr), ColorTheme.Type.Blue)]

    [Serializable]
    public class SetNumberAttribute : PropertyTypeSetNumber
    {
        [SerializeField] private PropertyGetGameObject m_Traits = GetGameObjectPlayer.Create();
        [SerializeField] private PropertyGetAttribute m_Attribute = new PropertyGetAttribute();

        public override void Set(double value, Args args)
        {
            Attribute attribute = this.m_Attribute.Get(args);
            if (attribute == null) return;
            
            GameObject gameObject = this.m_Traits.Get(args);
            if (gameObject == null) return;

            Traits traits = gameObject.Get<Traits>();
            if (traits == null) return;

            traits.RuntimeAttributes.Get(attribute.ID).Value = (float) value;
        }

        public override double Get(Args args)
        {
            Attribute attribute = this.m_Attribute.Get(args);
            if (attribute == null) return 0f;
            
            GameObject gameObject = this.m_Traits.Get(args);
            if (gameObject == null) return 0f;

            Traits traits = gameObject.Get<Traits>();
            return traits != null ? traits.RuntimeAttributes.Get(attribute.ID).Value : 0f;
        }

        public static PropertySetNumber Create => new PropertySetNumber(
            new SetNumberAttribute()
        );
        
        public override string String => $"{this.m_Traits}[{this.m_Attribute}]";
    }
}