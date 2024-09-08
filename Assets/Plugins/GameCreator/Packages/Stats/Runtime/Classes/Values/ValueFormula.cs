using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Image(typeof(IconFormula), ColorTheme.Type.Purple)]
    
    [Title("Formula")]
    [Category("Stats/Formula")]
    
    [Serializable]
    public class ValueFormula : TValue
    {
        public static readonly IdString TYPE_ID = new IdString("formula");
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private Formula m_Value;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override IdString TypeID => TYPE_ID;
        public override Type Type => typeof(Formula);
        
        public override bool CanSave => false;

        public override TValue Copy => new ValueFormula
        {
            m_Value = this.m_Value
        };
        
        // CONSTRUCTORS: --------------------------------------------------------------------------
        
        public ValueFormula() : base()
        { }

        public ValueFormula(Formula value) : this()
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
            this.m_Value = value as Formula;
        }
        
        public override string ToString()
        {
            return this.m_Value != null ? this.m_Value.name : "(none)";
        }
        
        // REGISTRATION METHODS: ------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void RuntimeInit() => RegisterValueType(
            TYPE_ID, 
            new TypeData(typeof(ValueFormula), CreateValue),
            typeof(Formula)
        );
        
        #if UNITY_EDITOR
        
        [UnityEditor.InitializeOnLoadMethod]
        private static void EditorInit() => RegisterValueType(
            TYPE_ID, 
            new TypeData(typeof(ValueFormula), CreateValue),
            typeof(Formula)
        );
        
        #endif

        private static ValueFormula CreateValue(object value)
        {
            return new ValueFormula(value as Formula);
        }
    }
}