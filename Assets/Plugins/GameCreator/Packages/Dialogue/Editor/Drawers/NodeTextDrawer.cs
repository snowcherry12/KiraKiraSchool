using GameCreator.Editor.Common;
using GameCreator.Runtime.Dialogue;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Dialogue
{
    [CustomPropertyDrawer(typeof(NodeText))]
    public class NodeTextDrawer : PropertyDrawer
    {
        private const string PROPERTY_TEXT = "m_Text";

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();
            SerializedProperty text = property.FindPropertyRelative(PROPERTY_TEXT);

            root.Add(new PropertyField(text));
            root.Add(new SpaceSmaller());
            root.Add(new NodeTextValuesTool(property));

            return root;
        }
    }
}