using GameCreator.Runtime.Stats;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Stats
{
    [CustomPropertyDrawer(typeof(OverrideAttributeData))]
    public class OverrideAttributeDataDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            SerializedProperty changePercent = property.FindPropertyRelative("m_ChangeStartPercent");
            PropertyField fieldChangePercent = new PropertyField(changePercent);
            
            root.Add(fieldChangePercent);
            return root;
        }
    }
}