using GameCreator.Runtime.Dialogue;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Dialogue
{
    [CustomPropertyDrawer(typeof(Expressions))]
    public class ExpressionsDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            ExpressionsTool tool = new ExpressionsTool(property);
            return tool;
        }
    }
}