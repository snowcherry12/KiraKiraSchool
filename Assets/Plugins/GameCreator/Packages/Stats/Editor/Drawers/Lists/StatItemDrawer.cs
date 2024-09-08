using GameCreator.Runtime.Stats;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Stats
{
    [CustomPropertyDrawer(typeof(StatItem))]
    public class StatItemDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            SerializedProperty stat = property.FindPropertyRelative("m_Stat");
            SerializedProperty changeValue = property.FindPropertyRelative("m_ChangeBase");
            SerializedProperty changeFormula = property.FindPropertyRelative("m_ChangeFormula");
            
            PropertyField fieldStat = new PropertyField(stat);
            PropertyField fieldChangeValue = new PropertyField(changeValue);
            PropertyField fieldChangeFormula = new PropertyField(changeFormula);
            
            root.Add(fieldStat);
            root.Add(fieldChangeValue);
            root.Add(fieldChangeFormula);

            return root;
        }
    }
}