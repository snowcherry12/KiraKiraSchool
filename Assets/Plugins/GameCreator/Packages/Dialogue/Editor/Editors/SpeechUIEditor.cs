using GameCreator.Editor.Common;
using GameCreator.Runtime.Dialogue.UnityUI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Dialogue
{
    [CustomEditor(typeof(SpeechUI))]
    public class SpeechUIEditor : UnityEditor.Editor
    {
        private VisualElement m_Root;

        public override VisualElement CreateInspectorGUI()
        {
            this.m_Root = new VisualElement();
            
            SerializedProperty active = this.serializedObject.FindProperty("m_Active");
            SerializedProperty actor = this.serializedObject.FindProperty("m_ActiveActor");
            SerializedProperty actorName = this.serializedObject.FindProperty("m_ActorName");
            SerializedProperty actorDesc = this.serializedObject.FindProperty("m_ActorDescription");
            SerializedProperty activePortrait = this.serializedObject.FindProperty("m_ActivePortrait");
            SerializedProperty portrait = this.serializedObject.FindProperty("m_Portrait");
            SerializedProperty text = this.serializedObject.FindProperty("m_Text");
            SerializedProperty skip = this.serializedObject.FindProperty("m_Skip");
            
            this.m_Root.Add(new PropertyField(active));
            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(new PropertyField(actor));
            this.m_Root.Add(new PropertyField(actorName));
            this.m_Root.Add(new PropertyField(actorDesc));
            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(new PropertyField(activePortrait));
            this.m_Root.Add(new PropertyField(portrait));
            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(new PropertyField(text));
            this.m_Root.Add(new PropertyField(skip));
            
            return this.m_Root;
        }
    }
}