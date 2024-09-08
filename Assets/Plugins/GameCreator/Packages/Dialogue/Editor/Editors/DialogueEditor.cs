using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Dialogue
{
    [CustomEditor(typeof(Runtime.Dialogue.Dialogue))]
    public class DialogueEditor : UnityEditor.Editor
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        private VisualElement m_Root;
        
        // PAINT METHODS: -------------------------------------------------------------------------
        
        public override VisualElement CreateInspectorGUI()
        {
            this.m_Root = new VisualElement();
            
            SerializedProperty story = this.serializedObject.FindProperty("m_Story");
            PropertyField fieldStory = new PropertyField(story);

            this.m_Root.Add(fieldStory);
            
            return this.m_Root;
        }

        public override bool UseDefaultMargins()
        {
            return false;
        }
        
        // CREATION MENU: -------------------------------------------------------------------------
        
        [MenuItem("GameObject/Game Creator/Dialogue/Dialogue", false, 0)]
        public static void CreateElement(MenuCommand menuCommand)
        {
            GameObject instance = new GameObject("Dialogue");
            instance.AddComponent<Runtime.Dialogue.Dialogue>();

            GameObjectUtility.SetParentAndAlign(instance, menuCommand?.context as GameObject);

            Undo.RegisterCreatedObjectUndo(instance, $"Create {instance.name}");
            Selection.activeObject = instance;
        }
    }
}