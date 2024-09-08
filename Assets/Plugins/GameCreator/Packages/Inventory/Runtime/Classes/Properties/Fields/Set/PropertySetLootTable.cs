using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class PropertySetLootTable : TPropertySet<PropertyTypeSetLootTable, LootTable>
    {
        public PropertySetLootTable() : base(new SetLootTableNone())
        { }

        public PropertySetLootTable(PropertyTypeSetLootTable defaultType) : base(defaultType)
        { }
    }
}