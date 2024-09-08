using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Loot Table")]
    [Category("Loot Table")]
    
    [Image(typeof(IconLoot), ColorTheme.Type.Red)]
    [Description("A reference to a Loot Table asset")]

    [Serializable] [HideLabelsInEditor]
    public class GetLootTableInstance : PropertyTypeGetLootTable
    {
        [SerializeField] protected LootTable m_LootTable;

        public override LootTable Get(Args args) => this.m_LootTable;
        public override LootTable Get(GameObject gameObject) => this.m_LootTable;

        public static PropertyGetLootTable Create(LootTable lootTable = null)
        {
            GetLootTableInstance instance = new GetLootTableInstance
            {
                m_LootTable = lootTable
            };
            
            return new PropertyGetLootTable(instance);
        }

        public override string String => this.m_LootTable != null
            ? this.m_LootTable.name
            : "(none)";
    }
}