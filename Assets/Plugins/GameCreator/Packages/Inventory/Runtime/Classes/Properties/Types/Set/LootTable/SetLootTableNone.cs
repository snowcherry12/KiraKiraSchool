using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("None")]
    [Category("None")]
    [Description("Don't save on anything")]
    
    [Image(typeof(IconNull), ColorTheme.Type.TextLight)]

    [Serializable]
    public class SetLootTableNone : PropertyTypeSetLootTable
    {
        public override void Set(LootTable value, Args args)
        { }

        public override void Set(LootTable value, GameObject gameObject)
        { }

        public static PropertySetLootTable Create => new PropertySetLootTable(
            new SetLootTableNone()
        );

        public override string String => "(none)";
    }
}
