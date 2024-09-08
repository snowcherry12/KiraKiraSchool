using GameCreator.Editor.Common;
using GameCreator.Runtime.Quests.UnityUI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Quests
{
    [CustomEditor(typeof(CompassItemUI))]
    public class CompassItemUIEditor : UnityEditor.Editor
    {
        // PAINT METHODS: -------------------------------------------------------------------------
        
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();
            
            SerializedProperty text = this.serializedObject.FindProperty("m_Text");
            SerializedProperty sprite = this.serializedObject.FindProperty("m_Sprite");
            SerializedProperty color = this.serializedObject.FindProperty("m_Color");

            root.Add(new PropertyField(text));
            root.Add(new PropertyField(sprite));
            root.Add(new PropertyField(color));

            SerializedProperty mode = this.serializedObject.FindProperty("m_Mode");
            SerializedProperty opacity = this.serializedObject.FindProperty("m_Opacity");

            root.Add(new SpaceSmall());
            root.Add(new PropertyField(mode));
            root.Add(new PropertyField(opacity));
            
            SerializedProperty distance = this.serializedObject.FindProperty("m_Distance");
            SerializedProperty multiplier = this.serializedObject.FindProperty("m_Multiplier");
            SerializedProperty unit = this.serializedObject.FindProperty("m_Unit");
            
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(distance));
            root.Add(new SpaceSmaller());
            root.Add(new PropertyField(multiplier));
            root.Add(new SpaceSmaller());
            root.Add(new PropertyField(unit));

            return root;
        }
    }
}