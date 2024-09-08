using GameCreator.Runtime.Behavior;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    [CustomPropertyDrawer(typeof(Connection))]
    public class ConnectionDrawer : PropertyDrawer
    {
        public const string PROP_VALUE = "m_Value";

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            string text = property.FindPropertyRelative(PROP_VALUE).stringValue;
            return new Label(text);
        }
    }
}