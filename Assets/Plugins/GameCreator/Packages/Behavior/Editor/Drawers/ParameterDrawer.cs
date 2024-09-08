using GameCreator.Runtime.Behavior;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    [CustomPropertyDrawer(typeof(Parameter))]
    public class ParameterDrawer : PropertyDrawer
    {
        public const string PROP_NAME = "m_Name";
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement content = new VisualElement();
            
            string name = property.FindPropertyRelative(PROP_NAME).stringValue;
            if (string.IsNullOrEmpty(name)) name = "(no-name)";
            
            SerializedProperty propertyValue = property.FindPropertyRelative("m_Value");
        
            PropertyField fieldValue = new PropertyField(propertyValue, name);
            content.Add(fieldValue);
        
            return content;
        }
    }
}