using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Last Looted")]
    [Category("Bags/Last Looted")]

    [Image(typeof(IconLoot), ColorTheme.Type.Red, typeof(OverlayDot))]
    [Description("A reference to the last Loot Table executed")]

    [Serializable] [HideLabelsInEditor]
    public class GetLootTableLastLooted : PropertyTypeGetLootTable
    {
        public override LootTable Get(Args args) => LootTable.LastLooted.LootTable;

        public override LootTable Get(GameObject gameObject) => LootTable.LastLooted.LootTable;

        public static PropertyGetLootTable Create()
        {
            GetLootTableLastLooted instance = new GetLootTableLastLooted();
            return new PropertyGetLootTable(instance);
        }

        public override string String => "Loot-Table[Last Looted]";
    }
}
