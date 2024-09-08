using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Inventory
{
    [Title("Global List Variable")]
    [Category("Global List Variable")]

    [Description("Sets the Loot Table value on a Global List Variable")]
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal, typeof(OverlayDot))]

    [Serializable] [HideLabelsInEditor]
    public class SetLootTableGlobalList : PropertyTypeSetLootTable
    {
        [SerializeField]
        protected FieldSetGlobalList m_Variable = new FieldSetGlobalList(ValueLootTable.TYPE_ID);

        public override void Set(LootTable value, Args args) => this.m_Variable.Set(value, args);
        public override LootTable Get(Args args) => this.m_Variable.Get(args) as LootTable;

        public static PropertySetLootTable Create => new PropertySetLootTable(
            new SetLootTableGlobalList()
        );

        public override string String => this.m_Variable.ToString();
    }
}
