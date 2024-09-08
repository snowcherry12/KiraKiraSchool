using System;
using System.Globalization;
using GameCreator.Runtime.Characters;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Stats
{
    [Title("Attribute Min Value")]
    [Category("Stats/Attribute Min Value")]

    [Image(typeof(IconAttr), ColorTheme.Type.Blue)]
    [Description("Returns the minimum value of an Attribute")]
    
    [Serializable]
    public class GetStringAttributeMinValue : PropertyTypeGetString
    {
        [SerializeField] private PropertyGetGameObject m_Traits = GetGameObjectPlayer.Create();
        [SerializeField] protected PropertyGetAttribute m_Attribute = new PropertyGetAttribute();

        public override string Get(Args args)
        {
            Attribute attribute = this.m_Attribute.Get(args);
            if (attribute == null) return string.Empty;
            
            Traits traits = this.m_Traits.Get<Traits>(args);
            if (traits == null) return string.Empty;

            return traits.RuntimeAttributes
                .Get(attribute.ID)?
                .MinValue.ToString("0", CultureInfo.InvariantCulture) ?? string.Empty;
        }

        public override string String => $"{this.m_Traits}[{this.m_Attribute}]";
    }
}