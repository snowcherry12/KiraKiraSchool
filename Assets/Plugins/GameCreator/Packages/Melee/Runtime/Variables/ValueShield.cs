using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Image(typeof(IconShieldSolid), ColorTheme.Type.Red)]
    [Title("Shield")]
    [Category("Melee/Shield")]
    
    [Serializable]
    public class ValueShield : TValue
    {
        public static readonly IdString TYPE_ID = new IdString("shield");
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private Shield m_Value;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override IdString TypeID => TYPE_ID;
        public override Type Type => typeof(Shield);
        
        public override bool CanSave => false;

        public override TValue Copy => new ValueShield
        {
            m_Value = this.m_Value
        };
        
        // CONSTRUCTORS: --------------------------------------------------------------------------
        
        public ValueShield() : base()
        { }

        public ValueShield(Shield value) : this()
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
            this.m_Value = value is Shield cast ? cast : null;
        }
        
        public override string ToString()
        {
            return this.m_Value != null ? this.m_Value.name : "(none)";
        }
        
        // REGISTRATION METHODS: ------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void RuntimeInit() => RegisterValueType(
            TYPE_ID, 
            new TypeData(typeof(ValueShield), CreateValue),
            typeof(Shield)
        );
        
        #if UNITY_EDITOR
        
        [UnityEditor.InitializeOnLoadMethod]
        private static void EditorInit() => RegisterValueType(
            TYPE_ID, 
            new TypeData(typeof(ValueShield), CreateValue),
            typeof(Shield)
        );
        
        #endif

        private static ValueShield CreateValue(object value)
        {
            return new ValueShield(value as Shield);
        }
    }
}