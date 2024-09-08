using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Stats
{
    [Serializable]
    public class PropertyGetAttribute : TPropertyGet<PropertyTypeGetAttribute, Attribute>
    {
        public PropertyGetAttribute() : base(new GetAttributeInstance())
        { }

        public PropertyGetAttribute(PropertyTypeGetAttribute defaultType) : base(defaultType)
        { }
    }
}