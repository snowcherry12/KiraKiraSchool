using GameCreator.Runtime.Stats;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Stats
{
    [CustomPropertyDrawer(typeof(StatList))]
    public class StatListDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            StatListTool statListTool = new StatListTool(property);
            return statListTool;
        }
    }
}