using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Stats
{
    [Serializable]
    public class PropertyGetStat : TPropertyGet<PropertyTypeGetStat, Stat>
    {
        public PropertyGetStat() : base(new GetStatInstance())
        { }

        public PropertyGetStat(PropertyTypeGetStat defaultType) : base(defaultType)
        { }
    }
}