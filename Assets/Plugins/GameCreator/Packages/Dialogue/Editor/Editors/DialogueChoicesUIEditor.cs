using GameCreator.Editor.Common;
using GameCreator.Runtime.Dialogue.UnityUI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Dialogue
{
    [CustomEditor(typeof(DialogueUnitChoicesUI))]
    public class DialogueChoicesUIEditor : UnityEditor.Editor
    {
        private VisualElement m_Root;
        
        public override VisualElement CreateInspectorGUI()
        {
            this.m_Root = new VisualElement();

            SerializedProperty active = this.serializedObject.FindProperty("m_Active");
            SerializedProperty content = this.serializedObject.FindProperty("m_ContentChoice");
            SerializedProperty prefab = this.serializedObject.FindProperty("m_PrefabChoice");
                
            this.m_Root.Add(new PropertyField(active));
            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(new PropertyField(content));
            this.m_Root.Add(new PropertyField(prefab));

            return this.m_Root;
        }
    }
}