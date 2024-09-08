using GameCreator.Runtime.Behavior;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    [CustomPropertyDrawer(typeof(Parameters))]
    public class ParametersDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            return new ParametersTool(property);
        }
    }
}