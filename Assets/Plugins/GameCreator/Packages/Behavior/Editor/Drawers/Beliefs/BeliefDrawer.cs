using GameCreator.Runtime.Behavior;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    [CustomPropertyDrawer(typeof(Belief))]
    public class BeliefDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();
            
            SerializedProperty name = property.FindPropertyRelative("m_Name");
            SerializedProperty value = property.FindPropertyRelative("m_Value");
            
            root.Add(new PropertyField(name));
            root.Add(new PropertyField(value));
            
            return root;
        }
    }
}