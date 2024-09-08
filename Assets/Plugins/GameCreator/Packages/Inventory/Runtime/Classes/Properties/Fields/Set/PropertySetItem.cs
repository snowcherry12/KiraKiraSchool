using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class PropertySetItem : TPropertySet<PropertyTypeSetItem, Item>
    {
        public PropertySetItem() : base(new SetItemNone())
        { }

        public PropertySetItem(PropertyTypeSetItem defaultType) : base(defaultType)
        { }
    }
}