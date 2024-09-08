using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Image(typeof(IconAttr), ColorTheme.Type.Blue)]
    
    [Title("Attribute")]
    [Category("Stats/Attribute")]
    
    [Serializable]
    public class ValueAttribute : TValue
    {
        public static readonly IdString TYPE_ID = new IdString("attribute");
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private Attribute m_Value;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override IdString TypeID => TYPE_ID;
        public override Type Type => typeof(Attribute);
        
        public override bool CanSave => false;

        public override TValue Copy => new ValueAttribute
        {
            m_Value = this.m_Value
        };
        
        // CONSTRUCTORS: --------------------------------------------------------------------------
        
        public ValueAttribute() : base()
        { }

        public ValueAttribute(Attribute value) : this()
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
            this.m_Value = value as Attribute;
        }
        
        public override string ToString()
        {
            return this.m_Value != null ? this.m_Value.name : "(none)";
        }
        
        // REGISTRATION METHODS: ------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void RuntimeInit() => RegisterValueType(
            TYPE_ID, 
            new TypeData(typeof(ValueAttribute), CreateValue),
            typeof(Attribute)
        );
        
        #if UNITY_EDITOR
        
        [UnityEditor.InitializeOnLoadMethod]
        private static void EditorInit() => RegisterValueType(
            TYPE_ID, 
            new TypeData(typeof(ValueAttribute), CreateValue),
            typeof(Attribute)
        );
        
        #endif

        private static ValueAttribute CreateValue(object value)
        {
            return new ValueAttribute(value as Attribute);
        }
    }
}