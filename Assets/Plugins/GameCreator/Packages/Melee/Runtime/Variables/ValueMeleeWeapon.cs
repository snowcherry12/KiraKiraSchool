using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Image(typeof(IconMeleeSword), ColorTheme.Type.Blue)]
    [Title("Melee Weapon")]
    [Category("Melee/Melee Weapon")]
    
    [Serializable]
    public class ValueMeleeWeapon : TValue
    {
        public static readonly IdString TYPE_ID = new IdString("melee-weapon");
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private MeleeWeapon m_Value;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override IdString TypeID => TYPE_ID;
        public override Type Type => typeof(MeleeWeapon);
        
        public override bool CanSave => false;

        public override TValue Copy => new ValueMeleeWeapon
        {
            m_Value = this.m_Value
        };
        
        // CONSTRUCTORS: --------------------------------------------------------------------------
        
        public ValueMeleeWeapon() : base()
        { }

        public ValueMeleeWeapon(MeleeWeapon value) : this()
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
            this.m_Value = value is MeleeWeapon cast ? cast : null;
        }
        
        public override string ToString()
        {
            return this.m_Value != null ? this.m_Value.name : "(none)";
        }
        
        // REGISTRATION METHODS: ------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void RuntimeInit() => RegisterValueType(
            TYPE_ID, 
            new TypeData(typeof(ValueMeleeWeapon), CreateValue),
            typeof(MeleeWeapon)
        );
        
        #if UNITY_EDITOR
        
        [UnityEditor.InitializeOnLoadMethod]
        private static void EditorInit() => RegisterValueType(
            TYPE_ID, 
            new TypeData(typeof(ValueMeleeWeapon), CreateValue),
            typeof(MeleeWeapon)
        );
        
        #endif

        private static ValueMeleeWeapon CreateValue(object value)
        {
            return new ValueMeleeWeapon(value as MeleeWeapon);
        }
    }
}