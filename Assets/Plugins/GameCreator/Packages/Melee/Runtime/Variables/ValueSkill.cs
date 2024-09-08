using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Image(typeof(IconMeleeSkill), ColorTheme.Type.Green)]
    [Title("Skill")]
    [Category("Melee/Skill")]
    
    [Serializable]
    public class ValueSkill : TValue
    {
        public static readonly IdString TYPE_ID = new IdString("skill");
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private Skill m_Value;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override IdString TypeID => TYPE_ID;
        public override Type Type => typeof(Skill);
        
        public override bool CanSave => false;

        public override TValue Copy => new ValueSkill
        {
            m_Value = this.m_Value
        };
        
        // CONSTRUCTORS: --------------------------------------------------------------------------
        
        public ValueSkill() : base()
        { }

        public ValueSkill(Skill value) : this()
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
            this.m_Value = value is Skill cast ? cast : null;
        }
        
        public override string ToString()
        {
            return this.m_Value != null ? this.m_Value.name : "(none)";
        }
        
        // REGISTRATION METHODS: ------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void RuntimeInit() => RegisterValueType(
            TYPE_ID, 
            new TypeData(typeof(ValueSkill), CreateValue),
            typeof(Skill)
        );
        
        #if UNITY_EDITOR
        
        [UnityEditor.InitializeOnLoadMethod]
        private static void EditorInit() => RegisterValueType(
            TYPE_ID, 
            new TypeData(typeof(ValueSkill), CreateValue),
            typeof(Skill)
        );
        
        #endif

        private static ValueSkill CreateValue(object value)
        {
            return new ValueSkill(value as Skill);
        }
    }
}