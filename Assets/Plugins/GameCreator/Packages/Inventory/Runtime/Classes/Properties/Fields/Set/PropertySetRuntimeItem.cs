using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class PropertySetRuntimeItem : TPropertySet<PropertyTypeSetRuntimeItem, RuntimeItem>
    {
        public PropertySetRuntimeItem() : base(new SetRuntimeItemNone())
        { }

        public PropertySetRuntimeItem(PropertyTypeSetRuntimeItem defaultType) : base(defaultType)
        { }
    }
}