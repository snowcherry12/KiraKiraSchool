using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Inventory
{
    [Title("Local Name Variable")]
    [Category("Variables/Local Name Variable")]

    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple)]
    [Description("Returns the Loot Table value of a Local Name Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetLootTableLocalName : PropertyTypeGetLootTable
    {
        [SerializeField]
        protected FieldGetLocalName m_Variable = new FieldGetLocalName(ValueLootTable.TYPE_ID);

        public override LootTable Get(Args args) => this.m_Variable.Get<LootTable>(args);

        public override string String => this.m_Variable.ToString();
    }
}
