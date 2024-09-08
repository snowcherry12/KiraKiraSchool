using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Stats
{
    [Serializable]
    public class PropertySetStat : TPropertySet<PropertyTypeSetStat, Stat>
    {
        public PropertySetStat() : base(new SetStatNone())
        { }

        public PropertySetStat(PropertyTypeSetStat defaultType) : base(defaultType)
        { }
    }
}