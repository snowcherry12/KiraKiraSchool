using GameCreator.Runtime.Dialogue;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Dialogue
{
    [CustomPropertyDrawer(typeof(Story))]
    public class StoryDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();
            
            SerializedProperty content = property.FindPropertyRelative("m_Content");
            PropertyField fieldContent = new PropertyField(content);
            
            root.Add(fieldContent);

            return root;
        }
    }
}