using GameCreator.Runtime.Dialogue;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Dialogue
{
    [CustomPropertyDrawer(typeof(Expressing))]
    public class ExpressingDrawer : PropertyDrawer
    {
        internal const string PROPERTY_ACTOR = "m_Actor";
        internal const string PROPERTY_EXPRESSION = "m_Expression";

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            return new ExpressingTool(property);
        }
    }
}