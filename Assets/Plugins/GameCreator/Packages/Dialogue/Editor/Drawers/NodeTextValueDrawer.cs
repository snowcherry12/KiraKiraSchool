using GameCreator.Editor.Common;
using GameCreator.Runtime.Dialogue;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Dialogue
{
    [CustomPropertyDrawer(typeof(NodeText.NodeTextValue))]
    public class NodeTextValueDrawer : PropertyDrawer
    {
        private const string PROPERTY_VALUE = "m_Value";
        private const string PROPERTY_IN_BOLD = "m_InBold";
        private const string PROPERTY_IN_ITALIC = "m_InItalic";
        private const string PROPERTY_USE_COLOR = "m_UseColor";
        private const string PROPERTY_COLOR = "m_Color";

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            return MakePropertyGUI(property);
        }

        public static VisualElement MakePropertyGUI(SerializedProperty property)
        {
            SerializedProperty value = property.FindPropertyRelative(PROPERTY_VALUE);
            SerializedProperty inBold = property.FindPropertyRelative(PROPERTY_IN_BOLD);
            SerializedProperty inItalic = property.FindPropertyRelative(PROPERTY_IN_ITALIC);
            SerializedProperty useColor = property.FindPropertyRelative(PROPERTY_USE_COLOR);
            SerializedProperty color = property.FindPropertyRelative(PROPERTY_COLOR);

            VisualElement root = new VisualElement();
            
            root.Add(new PropertyField(value));
            root.Add(new SpaceSmaller());
            root.Add(new PropertyField(inBold));
            root.Add(new PropertyField(inItalic));

            PropertyField fieldUseColor = new PropertyField(useColor);
            PropertyField fieldColor = new PropertyField(color);

            root.Add(new SpaceSmaller());
            root.Add(fieldUseColor);
            root.Add(fieldColor);
            
            fieldUseColor.RegisterValueChangeCallback(changeEvent =>
            {
                fieldColor.style.display = changeEvent.changedProperty.boolValue
                    ? DisplayStyle.Flex
                    : DisplayStyle.None;
            });

            fieldColor.style.display = useColor.boolValue
                ? DisplayStyle.Flex
                : DisplayStyle.None;

            return root;
        }
    }
}