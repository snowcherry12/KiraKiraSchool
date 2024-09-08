using GameCreator.Runtime.Stats;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Stats
{
    [CustomPropertyDrawer(typeof(AttributeList))]
    public class AttributeListDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            AttributeListTool attributeListTool = new AttributeListTool(property);
            return attributeListTool;
        }
    }
}