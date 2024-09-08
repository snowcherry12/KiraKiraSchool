using GameCreator.Editor.Common;
using GameCreator.Runtime.Quests.UnityUI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Quests
{
    public abstract class TTaskUIEditor : UnityEditor.Editor
    {
        private const string ERR_PREFAB = "Task Prefab does not contain a 'Task UI' component";

        protected abstract string Message { get; }
        
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            InfoMessage info = new InfoMessage(this.Message);
            root.Add(info);

            SerializedProperty title = this.serializedObject.FindProperty("m_Title");
            SerializedProperty description = this.serializedObject.FindProperty("m_Description");
            SerializedProperty color = this.serializedObject.FindProperty("m_Color");
            SerializedProperty sprite = this.serializedObject.FindProperty("m_Sprite");

            root.Add(new SpaceSmaller());
            root.Add(new PropertyField(title));
            root.Add(new SpaceSmaller());
            root.Add(new PropertyField(description));
            root.Add(new SpaceSmaller());
            root.Add(new PropertyField(color));
            root.Add(new SpaceSmaller());
            root.Add(new PropertyField(sprite));
            
            SerializedProperty countTo = this.serializedObject.FindProperty("m_CountTo");
            SerializedProperty currentCount = this.serializedObject.FindProperty("m_CurrentCount");
            SerializedProperty fillCounter = this.serializedObject.FindProperty("m_FillCounter");
            
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(countTo));
            root.Add(new PropertyField(currentCount));
            root.Add(new PropertyField(fillCounter));
            
            SerializedProperty styleGraphics = this.serializedObject.FindProperty("m_StyleGraphics");
            SerializedProperty activeElements = this.serializedObject.FindProperty("m_ActiveElements");
            SerializedProperty interactions = this.serializedObject.FindProperty("m_Interactions");
            
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(styleGraphics));
            root.Add(new PropertyField(activeElements));
            root.Add(new PropertyField(interactions));
            
            SerializedProperty show = this.serializedObject.FindProperty("m_Show");
            SerializedProperty showHidden = this.serializedObject.FindProperty("m_ShowHidden");
            
            root.Add(new SpaceSmaller());
            root.Add(new PropertyField(show));
            root.Add(new PropertyField(showHidden));
            
            SerializedProperty subtasksContent = this.serializedObject.FindProperty("m_SubtasksContent");
            SerializedProperty subtaskPrefab = this.serializedObject.FindProperty("m_SubtaskPrefab");

            PropertyField fieldTasksContent = new PropertyField(subtasksContent);
            ErrorMessage fieldErrorTaskPrefab = new ErrorMessage(ERR_PREFAB);
            PropertyField fieldTaskPrefab = new PropertyField(subtaskPrefab);
            
            root.Add(new SpaceSmaller());
            root.Add(fieldTasksContent);
            root.Add(fieldErrorTaskPrefab);
            root.Add(fieldTaskPrefab);

            fieldTaskPrefab.RegisterValueChangeCallback(changeEvent =>
            {
                GameObject prefab = changeEvent.changedProperty.objectReferenceValue as GameObject;
                fieldErrorTaskPrefab.style.display =
                    prefab != null && prefab.GetComponent<TaskUI>() == null
                        ? DisplayStyle.Flex
                        : DisplayStyle.None;
            });
            
            GameObject prefab = subtaskPrefab.objectReferenceValue as GameObject;
            fieldErrorTaskPrefab.style.display =
                prefab != null && prefab.GetComponent<TaskUI>() == null
                    ? DisplayStyle.Flex
                    : DisplayStyle.None;

            return root;
        }

        protected virtual void CreateAdditionalProperties(VisualElement root)
        { }
    }
}