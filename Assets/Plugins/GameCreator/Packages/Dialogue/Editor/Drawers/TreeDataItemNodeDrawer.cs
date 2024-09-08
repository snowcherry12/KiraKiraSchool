using GameCreator.Runtime.Common;
using GameCreator.Runtime.Dialogue;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Dialogue
{
    [CustomPropertyDrawer(typeof(TTreeDataItem<Node>))]
    public class TreeDataItemNodeDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            SerializedProperty node = property.FindPropertyRelative(TTreeDataItem<Node>.NAME_VALUE);
            return new PropertyField(node);
        }
    }
}