using GameCreator.Runtime.Shooter;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Shooter
{
    [CustomPropertyDrawer(typeof(RunReloadSequence))]
    public class RunReloadSequenceDrawer : PropertyDrawer
    {
        public const string NAME_SEQUENCE = "m_Sequence";
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            SerializedProperty sequence = property.FindPropertyRelative(NAME_SEQUENCE);
            return new ReloadSequenceTool(sequence);
        }
    }
}