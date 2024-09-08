using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [Image(typeof(IconQuestSolid), ColorTheme.Type.Yellow)]
    [Title("Quest")]
    [Category("Quests/Quest")]
    
    [Serializable]
    public class ValueQuest : TValue
    {
        public static readonly IdString TYPE_ID = new IdString("quest");
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private Quest m_Value;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override IdString TypeID => TYPE_ID;
        public override Type Type => typeof(Quest);
        
        public override bool CanSave => false;

        public override TValue Copy => new ValueQuest
        {
            m_Value = this.m_Value
        };
        
        // CONSTRUCTORS: --------------------------------------------------------------------------
        
        public ValueQuest() : base()
        { }

        public ValueQuest(Quest value) : this()
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
            this.m_Value = value is Quest cast ? cast : null;
        }
        
        public override string ToString()
        {
            return this.m_Value != null ? this.m_Value.name : "(none)";
        }
        
        // REGISTRATION METHODS: ------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void RuntimeInit() => RegisterValueType(
            TYPE_ID, 
            new TypeData(typeof(ValueQuest), CreateValue), 
            typeof(Quest)
        );
        
        #if UNITY_EDITOR
        
        [UnityEditor.InitializeOnLoadMethod]
        private static void EditorInit() => RegisterValueType(
            TYPE_ID, 
            new TypeData(typeof(ValueQuest), CreateValue),
            typeof(Quest)
        );
        
        #endif

        private static ValueQuest CreateValue(object value)
        {
            return new ValueQuest(value as Quest);
        }
    }
}