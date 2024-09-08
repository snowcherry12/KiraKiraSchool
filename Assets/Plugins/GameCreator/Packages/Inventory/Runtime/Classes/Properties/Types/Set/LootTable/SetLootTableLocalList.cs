using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Inventory
{
    [Title("Local List Variable")]
    [Category("Local List Variable")]

    [Description("Sets the Loot Table value on a Local List Variable")]
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal)]

    [Serializable] [HideLabelsInEditor]
    public class SetLootTableLocalList : PropertyTypeSetLootTable
    {
        [SerializeField]
        protected FieldSetLocalList m_Variable = new FieldSetLocalList(ValueLootTable.TYPE_ID);

        public override void Set(LootTable value, Args args) => this.m_Variable.Set(value, args);
        public override LootTable Get(Args args) => this.m_Variable.Get(args) as LootTable;

        public static PropertySetLootTable Create => new PropertySetLootTable(
            new SetLootTableLocalList()
        );

        public override string String => this.m_Variable.ToString();
    }
}
