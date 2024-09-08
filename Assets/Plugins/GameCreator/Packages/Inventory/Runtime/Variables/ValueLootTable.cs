using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Image(typeof(IconLoot), ColorTheme.Type.Red)]
    [Title("Loot Table")]
    [Category("Inventory/Loot Table")]
    
    [Serializable]
    public class ValueLootTable : TValue
    {
        public static readonly IdString TYPE_ID = new IdString("loot-table");
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private LootTable m_Value;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override IdString TypeID => TYPE_ID;
        public override Type Type => typeof(LootTable);
        
        public override bool CanSave => false;

        public override TValue Copy => new ValueLootTable
        {
            m_Value = this.m_Value
        };
        
        // CONSTRUCTORS: --------------------------------------------------------------------------
        
        public ValueLootTable() : base()
        { }

        public ValueLootTable(LootTable value) : this()
        {
            this.m_Value = value;
        }

        // OVERRIDE METHODS: ----------------------------------------------------------------------

        protected override object Get()
        {
            return this.m_Value;
        }

        protected override void Set(object value)
        {
            this.m_Value = value as LootTable;
        }
        
        public override string ToString()
        {
            return this.m_Value != null ? this.m_Value.name : "(none)";
        }
        
        // REGISTRATION METHODS: ------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void RuntimeInit() => RegisterValueType(
            TYPE_ID, 
            new TypeData(typeof(ValueLootTable), CreateValue),
            typeof(LootTable)
        );
        
        #if UNITY_EDITOR
        
        [UnityEditor.InitializeOnLoadMethod]
        private static void EditorInit() => RegisterValueType(
            TYPE_ID, 
            new TypeData(typeof(ValueLootTable), CreateValue),
            typeof(LootTable)
        );
        
        #endif

        private static ValueLootTable CreateValue(object value)
        {
            return new ValueLootTable(value as LootTable);
        }
    }
}