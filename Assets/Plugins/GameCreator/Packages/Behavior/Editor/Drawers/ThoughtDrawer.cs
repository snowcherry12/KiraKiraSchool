using GameCreator.Editor.Common;
using GameCreator.Runtime.Behavior;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    [CustomPropertyDrawer(typeof(Thought))]
    public class ThoughtDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement content = new VisualElement();

            SerializedProperty name = property.FindPropertyRelative("m_Name");
            SerializedProperty value = property.FindPropertyRelative("m_Value");
            
            PropertyField fieldName = new PropertyField(name);
            PropertyField fieldValue = new PropertyField(value);
            
            content.Add(fieldName);
            content.Add(new SpaceSmallest());
            content.Add(fieldValue);
        
            return content;
        }
    }
}