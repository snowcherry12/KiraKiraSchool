using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Image(typeof(IconStat), ColorTheme.Type.Red)]
    
    [Title("Stat")]
    [Category("Stats/Stat")]
    
    [Serializable]
    public class ValueStat : TValue
    {
        public static readonly IdString TYPE_ID = new IdString("stat");
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private Stat m_Value;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override IdString TypeID => TYPE_ID;
        public override Type Type => typeof(Stat);
        
        public override bool CanSave => false;

        public override TValue Copy => new ValueStat
        {
            m_Value = this.m_Value
        };
        
        // CONSTRUCTORS: --------------------------------------------------------------------------
        
        public ValueStat() : base()
        { }

        public ValueStat(Stat value) : this()
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
            this.m_Value = value as Stat;
        }
        
        public override string ToString()
        {
            return this.m_Value != null ? this.m_Value.name : "(none)";
        }
        
        // REGISTRATION METHODS: ------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void RuntimeInit() => RegisterValueType(
            TYPE_ID, 
            new TypeData(typeof(ValueStat), CreateValue),
            typeof(Stat)
        );
        
        #if UNITY_EDITOR
        
        [UnityEditor.InitializeOnLoadMethod]
        private static void EditorInit() => RegisterValueType(
            TYPE_ID, 
            new TypeData(typeof(ValueStat), CreateValue),
            typeof(Stat)
        );
        
        #endif

        private static ValueStat CreateValue(object value)
        {
            return new ValueStat(value as Stat);
        }
    }
}