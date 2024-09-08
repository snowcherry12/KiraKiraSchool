using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Stats
{
    [Serializable]
    public class PropertyGetStatusEffect : TPropertyGet<PropertyTypeGetStatusEffect, StatusEffect>
    {
        public PropertyGetStatusEffect() : base(new GetStatusEffectInstance())
        { }

        public PropertyGetStatusEffect(PropertyTypeGetStatusEffect defaultType) : base(defaultType)
        { }
    }
}