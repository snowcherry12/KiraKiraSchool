using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Stats
{
    [Serializable]
    public class PropertySetAttribute : TPropertySet<PropertyTypeSetAttribute, Attribute>
    {
        public PropertySetAttribute() : base(new SetAttributeNone())
        { }

        public PropertySetAttribute(PropertyTypeSetAttribute defaultType) : base(defaultType)
        { }
    }
}