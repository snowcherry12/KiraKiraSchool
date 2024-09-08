using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class PropertyGetRuntimeItem : TPropertyGet<PropertyTypeGetRuntimeItem, RuntimeItem>
    {
        public PropertyGetRuntimeItem() : base(new GetRuntimeItemCreateInstance())
        { }

        public PropertyGetRuntimeItem(PropertyTypeGetRuntimeItem defaultType) : base(defaultType)
        { }
    }
}