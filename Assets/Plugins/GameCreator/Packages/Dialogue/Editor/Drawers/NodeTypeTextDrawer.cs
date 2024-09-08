using GameCreator.Runtime.Dialogue;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Dialogue
{
    [CustomPropertyDrawer(typeof(NodeTypeText))]
    public class NodeTypeTextDrawer : TNodeTypeDrawer
    {
        protected override void SetupBody(SerializedProperty property, VisualElement body)
        { }
    }
}