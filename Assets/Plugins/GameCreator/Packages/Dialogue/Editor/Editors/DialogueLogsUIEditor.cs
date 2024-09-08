using GameCreator.Editor.Common;
using GameCreator.Runtime.Dialogue.UnityUI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Dialogue
{
    [CustomEditor(typeof(DialogueUnitLogsUI))]
    public class DialogueLogsUIEditor : UnityEditor.Editor
    {
        private VisualElement m_Root;

        public override VisualElement CreateInspectorGUI()
        {
            this.m_Root = new VisualElement();
            
            SerializedProperty content = this.serializedObject.FindProperty("m_Content");
            SerializedProperty prefab = this.serializedObject.FindProperty("m_Prefab");
            SerializedProperty keepAmount = this.serializedObject.FindProperty("m_KeepAmount");
            SerializedProperty maxAmount = this.serializedObject.FindProperty("m_MaxAmount");
            
            this.m_Root.Add(new PropertyField(content));
            this.m_Root.Add(new PropertyField(prefab));

            PropertyField fieldKeepAmount = new PropertyField(keepAmount, "Max Amount");
            PropertyField fieldMaxAmount = new PropertyField(maxAmount, " ");
            
            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(fieldKeepAmount);
            this.m_Root.Add(fieldMaxAmount);

            fieldKeepAmount.RegisterValueChangeCallback(changeEvent =>
            {
                fieldMaxAmount.style.display = changeEvent.changedProperty.boolValue
                    ? DisplayStyle.Flex
                    : DisplayStyle.None;
            });
            
            fieldMaxAmount.style.display = keepAmount.boolValue
                ? DisplayStyle.Flex
                : DisplayStyle.None;

            return this.m_Root;
        }
    }
}