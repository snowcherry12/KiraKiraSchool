using GameCreator.Editor.Common;
using GameCreator.Runtime.Quests.UnityUI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Quests
{
    [CustomEditor(typeof(IndicatorsUI))]
    public class IndicatorsUIEditor : UnityEditor.Editor
    {
        // PAINT METHODS: -------------------------------------------------------------------------

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();
            
            SerializedProperty camera = this.serializedObject.FindProperty("m_Camera");
            SerializedProperty layers = this.serializedObject.FindProperty("m_Layers");

            root.Add(new PropertyField(camera));
            root.Add(new SpaceSmaller());
            root.Add(new PropertyField(layers));

            SerializedProperty prefab = this.serializedObject.FindProperty("m_Prefab");
            SerializedProperty content = this.serializedObject.FindProperty("m_Content");

            root.Add(new SpaceSmall());
            root.Add(new PropertyField(prefab));
            root.Add(new PropertyField(content)); 
            
            SerializedProperty showHidden = this.serializedObject.FindProperty("m_HiddenQuests");
            SerializedProperty hideUntrack = this.serializedObject.FindProperty("m_HideUntracked");
            SerializedProperty keepBounds = this.serializedObject.FindProperty("m_KeepInBounds");

            root.Add(new SpaceSmall());
            root.Add(new PropertyField(showHidden));
            root.Add(new PropertyField(hideUntrack));
            root.Add(new PropertyField(keepBounds));

            return root;
        }
    }
}