using GameCreator.Runtime.Dialogue;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Dialogue
{
    [CustomPropertyDrawer(typeof(NodeTypeRandom))]
    public class NodeTypeRandomDrawer : TNodeTypeDrawer
    {
        protected override void SetupBody(SerializedProperty property, VisualElement body)
        {
            SerializedProperty allowRepeat = property.FindPropertyRelative("m_AllowRepeat");
            body.Add(new PropertyField(allowRepeat));
        }
    }
}