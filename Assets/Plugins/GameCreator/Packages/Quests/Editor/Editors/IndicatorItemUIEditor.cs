using GameCreator.Editor.Common;
using GameCreator.Runtime.Quests.UnityUI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Quests
{
    [CustomEditor(typeof(IndicatorItemUI))]
    public class IndicatorItemUIEditor : UnityEditor.Editor
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
            
            SerializedProperty activeIfOnscreen = this.serializedObject.FindProperty("m_ActiveIfOnscreen");
            SerializedProperty activeIfOffscreen = this.serializedObject.FindProperty("m_ActiveIfOffscreen");
            SerializedProperty rotateTo = this.serializedObject.FindProperty("m_RotateTo");
            
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(activeIfOnscreen));
            root.Add(new PropertyField(activeIfOffscreen));
            root.Add(new PropertyField(rotateTo));

            return root;
        }
    }
}