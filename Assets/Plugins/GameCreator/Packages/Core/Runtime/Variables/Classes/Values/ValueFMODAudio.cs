using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Variables
{
    [Image(typeof(IconAudioClip), ColorTheme.Type.Yellow)]
    [Title("FMOD Audio")]
    [Category("References/FMOD Audio")]
    
    [Serializable]
    public class ValueFMODAudio : TValue
    {
        public static readonly IdString TYPE_ID = new IdString("fmod-audio");
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private FMODAudio m_Value;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override IdString TypeID => TYPE_ID;
        public override Type Type => typeof(FMODAudio);
        
        public override bool CanSave => false;
        
        public override TValue Copy => new ValueFMODAudio()
        {
            m_Value = this.m_Value
        };

        // CONSTRUCTORS: --------------------------------------------------------------------------
        
        public ValueFMODAudio() : base()
        { }

        public ValueFMODAudio(FMODAudio value) : this()
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
            this.m_Value = value as FMODAudio;
        }
        
        public override string ToString()
        {
            return this.m_Value != null ? this.m_Value.Audio.ToString() : "(none)";
        }
        
        // REGISTRATION METHODS: ------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void RuntimeInit() => RegisterValueType(
            TYPE_ID, 
            new TypeData(typeof(ValueFMODAudio), CreateValue),
            typeof(FMODAudio)
        );
        
        #if UNITY_EDITOR
        
        [UnityEditor.InitializeOnLoadMethod]
        private static void EditorInit() => RegisterValueType(
            TYPE_ID, 
            new TypeData(typeof(ValueFMODAudio), CreateValue),
            typeof(FMODAudio)
        );
        
        #endif

        private static ValueFMODAudio CreateValue(object value)
        {
            return new ValueFMODAudio(value as FMODAudio);
        }
    }
}