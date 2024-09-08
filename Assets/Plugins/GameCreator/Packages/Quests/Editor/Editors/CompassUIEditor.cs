using GameCreator.Editor.Common;
using GameCreator.Runtime.Quests.UnityUI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Quests
{
    [CustomEditor(typeof(CompassUI))]
    public class CompassUIEditor : UnityEditor.Editor
    {
        // PAINT METHODS: -------------------------------------------------------------------------
        
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();
            
            SerializedProperty character = this.serializedObject.FindProperty("m_Character");
            SerializedProperty camera = this.serializedObject.FindProperty("m_Camera");
            SerializedProperty fov = this.serializedObject.FindProperty("m_FieldOfView");
            SerializedProperty layers = this.serializedObject.FindProperty("m_Layers");
            
            root.Add(new PropertyField(character));
            root.Add(new SpaceSmaller());
            root.Add(new PropertyField(camera));
            root.Add(new SpaceSmaller());
            root.Add(new PropertyField(fov));
            root.Add(new SpaceSmaller());
            root.Add(new PropertyField(layers));

            SerializedProperty prefab = this.serializedObject.FindProperty("m_Prefab");
            SerializedProperty content = this.serializedObject.FindProperty("m_Content");

            root.Add(new SpaceSmall());
            root.Add(new PropertyField(prefab));
            root.Add(new PropertyField(content)); 
            
            SerializedProperty showHidden = this.serializedObject.FindProperty("m_HiddenQuests");
            SerializedProperty hideUntrack = this.serializedObject.FindProperty("m_HideUntracked");

            root.Add(new SpaceSmall());
            root.Add(new PropertyField(showHidden));
            root.Add(new PropertyField(hideUntrack));

            return root;
        }
    }
}