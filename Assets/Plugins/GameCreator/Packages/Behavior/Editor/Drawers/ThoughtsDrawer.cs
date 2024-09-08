using GameCreator.Runtime.Behavior;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    [CustomPropertyDrawer(typeof(Thoughts))]
    public class ThoughtsDrawer : PropertyDrawer
    {
        public const string PROP_LIST = "m_List";
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            ThoughtsTool thoughtsTool = new ThoughtsTool(property);
            return thoughtsTool;
        }
    }
}