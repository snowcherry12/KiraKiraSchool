using GameCreator.Runtime.Dialogue;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Dialogue
{
    [CustomPropertyDrawer(typeof(ShotCameraEntry))]
    public class ShotCameraEntryDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            SerializedProperty shot = property.FindPropertyRelative("m_ShotCamera");
            return new PropertyField(shot, string.Empty);
        }
    }
}