using GameCreator.Editor.Common;
using GameCreator.Runtime.Quests.UnityUI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Quests
{
    [CustomEditor(typeof(QuestListUI))]
    public class QuestListUIEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            SerializedProperty journal = this.serializedObject.FindProperty("m_Journal");
            SerializedProperty filter = this.serializedObject.FindProperty("m_Filter");
            
            root.Add(new PropertyField(journal));
            root.Add(new SpaceSmaller());
            root.Add(new PropertyField(filter));
            
            SerializedProperty content = this.serializedObject.FindProperty("m_Content");
            SerializedProperty prefab = this.serializedObject.FindProperty("m_Prefab");
            
            root.Add(new SpaceSmaller());
            root.Add(new PropertyField(content));
            root.Add(new PropertyField(prefab));

            return root;
        }
    }
}