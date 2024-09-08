using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Stats
{
    [Serializable]
    public class PropertySetStatusEffect : TPropertySet<PropertyTypeSetStatusEffect, StatusEffect>
    {
        public PropertySetStatusEffect() : base(new SetStatusEffectNone())
        { }

        public PropertySetStatusEffect(PropertyTypeSetStatusEffect defaultType) : base(defaultType)
        { }
    }
}