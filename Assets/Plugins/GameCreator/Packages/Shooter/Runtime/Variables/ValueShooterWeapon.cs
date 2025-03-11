using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Image(typeof(IconPistol), ColorTheme.Type.Blue)]
    [Title("Shooter Weapon")]
    [Category("Shooter/Shooter Weapon")]
    
    [Serializable]
    public class ValueShooterWeapon : TValue
    {
        public static readonly IdString TYPE_ID = new IdString("shooter-weapon");
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private ShooterWeapon m_Value;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override IdString TypeID => TYPE_ID;
        public override Type Type => typeof(ShooterWeapon);
        
        public override bool CanSave => false;

        public override TValue Copy => new ValueShooterWeapon
        {
            m_Value = this.m_Value
        };
        
        // CONSTRUCTORS: --------------------------------------------------------------------------
        
        public ValueShooterWeapon() : base()
        { }

        public ValueShooterWeapon(ShooterWeapon value) : this()
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
            this.m_Value = value is ShooterWeapon cast ? cast : null;
        }
        
        public override string ToString()
        {
            return this.m_Value != null ? this.m_Value.name : "(none)";
        }
        
        // REGISTRATION METHODS: ------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void RuntimeInit() => RegisterValueType(
            TYPE_ID, 
            new TypeData(typeof(ValueShooterWeapon), CreateValue),
            typeof(ShooterWeapon)
        );
        
        #if UNITY_EDITOR
        
        [UnityEditor.InitializeOnLoadMethod]
        private static void EditorInit() => RegisterValueType(
            TYPE_ID, 
            new TypeData(typeof(ValueShooterWeapon), CreateValue),
            typeof(ShooterWeapon)
        );
        
        #endif

        private static ValueShooterWeapon CreateValue(object value)
        {
            return new ValueShooterWeapon(value as ShooterWeapon);
        }
    }
}