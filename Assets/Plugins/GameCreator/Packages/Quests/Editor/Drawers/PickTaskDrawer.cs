using GameCreator.Runtime.Quests;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Quests
{
    [CustomPropertyDrawer(typeof(PickTask))]
    public class PickTaskDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            return new PickTaskTool(property);
        }
    }
}