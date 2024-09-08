using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Inventory
{
    [Title("Local Name Variable")]
    [Category("Local Name Variable")]

    [Description("Sets the Loot Table value on a Local Name Variable")]
    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple)]

    [Serializable] [HideLabelsInEditor]
    public class SetLootTableLocalName : PropertyTypeSetLootTable
    {
        [SerializeField]
        protected FieldSetLocalName m_Variable = new FieldSetLocalName(ValueLootTable.TYPE_ID);

        public override void Set(LootTable value, Args args) => this.m_Variable.Set(value, args);
        public override LootTable Get(Args args) => this.m_Variable.Get(args) as LootTable;

        public static PropertySetLootTable Create => new PropertySetLootTable(
            new SetLootTableLocalName()
        );

        public override string String => this.m_Variable.ToString();
    }
}
