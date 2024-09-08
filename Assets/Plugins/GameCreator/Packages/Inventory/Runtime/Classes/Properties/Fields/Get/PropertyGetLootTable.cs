using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class PropertyGetLootTable : TPropertyGet<PropertyTypeGetLootTable, LootTable>
    {
        public PropertyGetLootTable() : base(new GetLootTableInstance())
        { }

        public PropertyGetLootTable(PropertyTypeGetLootTable defaultType) : base(defaultType)
        { }
    }
}