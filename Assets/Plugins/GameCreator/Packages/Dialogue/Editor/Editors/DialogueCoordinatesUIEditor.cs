using GameCreator.Editor.Common;
using GameCreator.Runtime.Dialogue.UnityUI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Dialogue
{
    [CustomEditor(typeof(DialogueCoordinatesUI))]
    public class DialogueCoordinatesUIEditor : UnityEditor.Editor
    {
        private VisualElement m_Root;
        
        public override VisualElement CreateInspectorGUI()
        {
            this.m_Root = new VisualElement();

            SerializedProperty alignX = this.serializedObject.FindProperty("m_DefaultVertical");
            SerializedProperty alignY = this.serializedObject.FindProperty("m_DefaultHorizontal");
            
            this.m_Root.Add(new PropertyField(alignX));
            this.m_Root.Add(new PropertyField(alignY));
            
            SerializedProperty offset = this.serializedObject.FindProperty("m_Offset");
            SerializedProperty keepParent = this.serializedObject.FindProperty("m_KeepInParent");
            
            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(new PropertyField(offset));
            this.m_Root.Add(new PropertyField(keepParent));
            
            SerializedProperty anchorTo = this.serializedObject.FindProperty("m_AnchorTo");
            SerializedProperty anchor = this.serializedObject.FindProperty("m_Anchor");

            PropertyField anchorToField = new PropertyField(anchorTo);
            PropertyField anchorField = new PropertyField(anchor)
            {
                style =
                {
                    display = anchorTo.enumValueIndex == 0
                        ? DisplayStyle.None
                        : DisplayStyle.Flex
                }
            };

            anchorToField.RegisterValueChangeCallback(changeEvent =>
            {
                anchorField.style.display = changeEvent.changedProperty.enumValueIndex == 0
                    ? DisplayStyle.None
                    : DisplayStyle.Flex;
            });

            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(anchorToField);
            this.m_Root.Add(anchorField);

            return this.m_Root;
        }
    }
}