using GameCreator.Runtime.Stats;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Stats
{
    [CustomPropertyDrawer(typeof(AttributeItem))]
    public class AttributeItemDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            SerializedProperty attr = property.FindPropertyRelative("m_Attribute");
            SerializedProperty changeStartPercent = property.FindPropertyRelative("m_ChangeStartPercent");

            PropertyField fieldAttr = new PropertyField(attr);
            PropertyField fieldChangeStartPercent = new PropertyField(changeStartPercent);
            
            root.Add(fieldAttr);
            root.Add(fieldChangeStartPercent);

            return root;
        }
    }
}