using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class PropertyGetItem : TPropertyGet<PropertyTypeGetItem, Item>
    {
        public PropertyGetItem() : base(new GetItemInstance())
        { }

        public PropertyGetItem(PropertyTypeGetItem defaultType) : base(defaultType)
        { }
    }
}