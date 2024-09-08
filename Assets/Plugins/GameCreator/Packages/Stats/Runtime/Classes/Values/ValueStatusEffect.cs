using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Image(typeof(IconStatusEffect), ColorTheme.Type.Green)]
    
    [Title("Status Effect")]
    [Category("Stats/Status Effect")]
    
    [Serializable]
    public class ValueStatusEffect : TValue
    {
        public static readonly IdString TYPE_ID = new IdString("status-effect");
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private StatusEffect m_Value;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override IdString TypeID => TYPE_ID;
        public override Type Type => typeof(StatusEffect);
        
        public override bool CanSave => false;

        public override TValue Copy => new ValueStatusEffect
        {
            m_Value = this.m_Value
        };
        
        // CONSTRUCTORS: --------------------------------------------------------------------------
        
        public ValueStatusEffect() : base()
        { }

        public ValueStatusEffect(StatusEffect value) : this()
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
            this.m_Value = value as StatusEffect;
        }
        
        public override string ToString()
        {
            return this.m_Value != null ? this.m_Value.name : "(none)";
        }
        
        // REGISTRATION METHODS: ------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void RuntimeInit() => RegisterValueType(
            TYPE_ID, 
            new TypeData(typeof(ValueStatusEffect), CreateValue),
            typeof(StatusEffect)
        );
        
        #if UNITY_EDITOR
        
        [UnityEditor.InitializeOnLoadMethod]
        private static void EditorInit() => RegisterValueType(
            TYPE_ID, 
            new TypeData(typeof(ValueStatusEffect), CreateValue),
            typeof(StatusEffect)
        );
        
        #endif

        private static ValueStatusEffect CreateValue(object value)
        {
            return new ValueStatusEffect(value as StatusEffect);
        }
    }
}