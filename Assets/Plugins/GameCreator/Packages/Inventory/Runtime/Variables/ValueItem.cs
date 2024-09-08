using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Image(typeof(IconItem), ColorTheme.Type.Green)]
    [Title("Item")]
    [Category("Inventory/Item")]
    
    [Serializable]
    public class ValueItem : TValue
    {
        public static readonly IdString TYPE_ID = new IdString("item");
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private Item m_Value;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override IdString TypeID => TYPE_ID;
        public override Type Type => typeof(Item);
        
        public override bool CanSave => false;

        public override TValue Copy => new ValueItem
        {
            m_Value = this.m_Value
        };
        
        // CONSTRUCTORS: --------------------------------------------------------------------------
        
        public ValueItem() : base()
        { }

        public ValueItem(Item value) : this()
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
            this.m_Value = value as Item;
        }
        
        public override string ToString()
        {
            return this.m_Value != null ? this.m_Value.name : "(none)";
        }
        
        // REGISTRATION METHODS: ------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void RuntimeInit() => RegisterValueType(
            TYPE_ID, 
            new TypeData(typeof(ValueItem), CreateValue),
            typeof(Item)
        );
        
        #if UNITY_EDITOR
        
        [UnityEditor.InitializeOnLoadMethod]
        private static void EditorInit() => RegisterValueType(
            TYPE_ID, 
            new TypeData(typeof(ValueItem), CreateValue),
            typeof(Item)
        );
        
        #endif

        private static ValueItem CreateValue(object value)
        {
            return new ValueItem(value as Item);
        }
    }
}