using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Inventory
{
    [Title("Global Name Variable")]
    [Category("Global Name Variable")]

    [Description("Sets the Loot Table value on a Global Name Variable")]
    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple, typeof(OverlayDot))]

    [Serializable] [HideLabelsInEditor]
    public class SetLootTableGlobalName : PropertyTypeSetLootTable
    {
        [SerializeField]
        protected FieldSetGlobalName m_Variable = new FieldSetGlobalName(ValueLootTable.TYPE_ID);

        public override void Set(LootTable value, Args args) => this.m_Variable.Set(value, args);
        public override LootTable Get(Args args) => this.m_Variable.Get(args) as LootTable;

        public static PropertySetLootTable Create => new PropertySetLootTable(
            new SetLootTableGlobalName()
        );

        public override string String => this.m_Variable.ToString();
    }
}
