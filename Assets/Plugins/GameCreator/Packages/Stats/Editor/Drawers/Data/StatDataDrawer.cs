using GameCreator.Runtime.Stats;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Stats
{
    [CustomPropertyDrawer(typeof(StatData))]
    public class StatDataDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            SerializedProperty value = property.FindPropertyRelative("m_Base");
            SerializedProperty formula = property.FindPropertyRelative("m_Formula");
            
            PropertyField fieldValue = new PropertyField(value, "Base (value)");
            PropertyField fieldFormula = new PropertyField(formula);
            
            root.Add(fieldValue);
            root.Add(fieldFormula);
            
            return root;
        }
    }
}