using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Inventory
{
    [Title("Global Name Variable")]
    [Category("Variables/Global Name Variable")]

    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple, typeof(OverlayDot))]
    [Description("Returns the Loot Table value of a Global Name Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetLootTableGlobalName : PropertyTypeGetLootTable
    {
        [SerializeField]
        protected FieldGetGlobalName m_Variable = new FieldGetGlobalName(ValueLootTable.TYPE_ID);

        public override LootTable Get(Args args) => this.m_Variable.Get<LootTable>(args);

        public override string String => this.m_Variable.ToString();
    }
}
