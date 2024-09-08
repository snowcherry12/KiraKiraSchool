using GameCreator.Runtime.Stats;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Stats
{
    [CustomPropertyDrawer(typeof(OverrideStatData))]
    public class OverrideStatDataDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            SerializedProperty changeBase = property.FindPropertyRelative("m_ChangeBase");
            PropertyField fieldChangeBase = new PropertyField(changeBase);
            
            root.Add(fieldChangeBase);
            return root;
        }
    }
}