using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Image(typeof(IconItem), ColorTheme.Type.Blue)]
    [Title("Runtime Item")]
    [Category("Inventory/Runtime Item")]
    
    [Serializable]
    public class ValueRuntimeItem : TValue
    {
        public static readonly IdString TYPE_ID = new IdString("runtime-item");
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private Item m_Value;

        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private RuntimeItem m_Instance;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override IdString TypeID => TYPE_ID;
        public override Type Type => typeof(RuntimeItem);
        
        public override bool CanSave => false;

        public override TValue Copy => new ValueRuntimeItem
        {
            m_Value = this.m_Value,
            m_Instance = this.m_Instance
        };
        
        // CONSTRUCTORS: --------------------------------------------------------------------------
        
        public ValueRuntimeItem() : base()
        { }

        public ValueRuntimeItem(Item value) : this()
        {
            this.m_Value = value;
            this.m_Instance = value != null ? value.CreateRuntimeItem(Args.EMPTY) : null;
        }

        // OVERRIDE METHODS: ----------------------------------------------------------------------

        protected override object Get()
        {
            if (this.m_Instance == null && this.m_Value != null)
            {
                this.m_Instance = this.m_Value.CreateRuntimeItem(Args.EMPTY);
            }
            
            return this.m_Instance;
        }

        protected override void Set(object value)
        {
            RuntimeItem runtimeItem = value as RuntimeItem;

            this.m_Value = runtimeItem?.Item;
            this.m_Instance = runtimeItem;
        }
        
        public override string ToString()
        {
            return this.m_Value != null ? this.m_Value.name : "(none)";
        }
        
        // REGISTRATION METHODS: ------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void RuntimeInit() => RegisterValueType(
            TYPE_ID, 
            new TypeData(typeof(ValueRuntimeItem), CreateValue),
            typeof(RuntimeItem)
        );
        
        #if UNITY_EDITOR
        
        [UnityEditor.InitializeOnLoadMethod]
        private static void EditorInit() => RegisterValueType(
            TYPE_ID, 
            new TypeData(typeof(ValueRuntimeItem), CreateValue),
            typeof(RuntimeItem)
        );
        
        #endif

        private static ValueRuntimeItem CreateValue(object value)
        {
            return new ValueRuntimeItem(value as Item);
        }
    }
}