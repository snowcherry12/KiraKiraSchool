using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("None")]
    [Category("None")]

    [Image(typeof(IconEmpty), ColorTheme.Type.TextLight)]
    [Description("A empty reference to no Loot Table")]

    [Serializable]
    public class GetLootTableNone : PropertyTypeGetLootTable
    {
        public override LootTable Get(Args args) => null;
        public override LootTable Get(GameObject gameObject) => null;

        public static PropertyGetLootTable Create()
        {
            GetLootTableNone instance = new GetLootTableNone();
            return new PropertyGetLootTable(instance);
        }

        public override string String => "None";
    }
}
