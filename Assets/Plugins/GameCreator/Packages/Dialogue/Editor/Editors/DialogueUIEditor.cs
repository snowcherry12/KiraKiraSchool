using GameCreator.Runtime.Dialogue.UnityUI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Dialogue
{
    [CustomEditor(typeof(DialogueUI))]
    public class DialogueUIEditor : UnityEditor.Editor
    {
        private VisualElement m_Root;
        
        public override VisualElement CreateInspectorGUI()
        {
            this.m_Root = new VisualElement();

            SerializedProperty container = this.serializedObject.FindProperty("m_SpeechContainer");
            SerializedProperty speech = this.serializedObject.FindProperty("m_DefaultSpeech");
            
            this.m_Root.Add(new PropertyField(container, "Speech"));
            this.m_Root.Add(new PropertyField(speech, "Default"));

            return this.m_Root;
        }
    }
}