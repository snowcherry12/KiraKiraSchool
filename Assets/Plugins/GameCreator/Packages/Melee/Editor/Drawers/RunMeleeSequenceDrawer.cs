using GameCreator.Runtime.Melee;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Melee
{
    [CustomPropertyDrawer(typeof(RunMeleeSequence))]
    public class RunMeleeSequenceDrawer : PropertyDrawer
    {
        public const string NAME_SEQUENCE = "m_Sequence";
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            SerializedProperty sequence = property.FindPropertyRelative(NAME_SEQUENCE);
            return new MeleeSequenceTool(sequence);
        }
    }
}