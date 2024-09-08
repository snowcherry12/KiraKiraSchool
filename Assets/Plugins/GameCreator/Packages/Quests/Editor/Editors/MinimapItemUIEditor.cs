using GameCreator.Runtime.Quests.UnityUI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Quests
{
    [CustomEditor(typeof(MinimapItemUI))]
    public class MinimapItemUIEditor : UnityEditor.Editor
    {
        // PAINT METHODS: -------------------------------------------------------------------------
        
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();
            
            SerializedProperty text = this.serializedObject.FindProperty("m_Text");
            SerializedProperty sprite = this.serializedObject.FindProperty("m_Sprite");
            SerializedProperty color = this.serializedObject.FindProperty("m_Color");
            SerializedProperty opacity = this.serializedObject.FindProperty("m_Opacity");
            
            root.Add(new PropertyField(text));
            root.Add(new PropertyField(sprite));
            root.Add(new PropertyField(color));
            root.Add(new PropertyField(opacity));

            return root;
        }
    }
}