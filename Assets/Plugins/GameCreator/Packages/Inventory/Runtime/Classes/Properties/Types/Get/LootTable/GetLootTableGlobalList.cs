using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Inventory
{
    [Title("Global List Variable")]
    [Category("Variables/Global List Variable")]

    [Image(typeof(IconListVariable), ColorTheme.Type.Teal, typeof(OverlayDot))]
    [Description("Returns the Loot Table value of a Global List Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetLootTableGlobalList : PropertyTypeGetLootTable
    {
        [SerializeField]
        protected FieldGetGlobalList m_Variable = new FieldGetGlobalList(ValueLootTable.TYPE_ID);

        public override LootTable Get(Args args) => this.m_Variable.Get<LootTable>(args);

        public override string String => this.m_Variable.ToString();
    }
}
