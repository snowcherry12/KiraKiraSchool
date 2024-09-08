using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Inventory
{
    [Title("Local List Variable")]
    [Category("Variables/Local List Variable")]

    [Image(typeof(IconListVariable), ColorTheme.Type.Teal)]
    [Description("Returns the Loot Table value of a Local List Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetLootTableLocalList : PropertyTypeGetLootTable
    {
        [SerializeField]
        protected FieldGetLocalList m_Variable = new FieldGetLocalList(ValueLootTable.TYPE_ID);

        public override LootTable Get(Args args) => this.m_Variable.Get<LootTable>(args);

        public override string String => this.m_Variable.ToString();
    }
}
