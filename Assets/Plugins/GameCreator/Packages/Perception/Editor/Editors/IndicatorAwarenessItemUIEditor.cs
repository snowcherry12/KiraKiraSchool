using GameCreator.Editor.Common;
using GameCreator.Runtime.Perception.UnityUI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Perception
{
    [CustomEditor(typeof(IndicatorAwarenessItemUI))]
    public class IndicatorAwarenessItemUIEditor : UnityEditor.Editor
    {
        private const string TITLE = "Configured by parent Indicator Awareness UI component";
        
        // INSPECTOR METHODS: ---------------------------------------------------------------------
        
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            InfoMessage message = new InfoMessage(TITLE);
            root.Add(new SpaceSmaller());
            root.Add(message);
            
            SerializedProperty color = this.serializedObject.FindProperty("m_Color");
            SerializedProperty opacity = this.serializedObject.FindProperty("m_Opacity");
            
            root.Add(new SpaceSmaller());
            root.Add(new PropertyField(color));
            root.Add(new PropertyField(opacity));
            
            SerializedProperty activeIfNotZero = this.serializedObject.FindProperty("m_ActiveIfNotZero");
            SerializedProperty activeIfSuspicious = this.serializedObject.FindProperty("m_ActiveIfSuspicious");
            SerializedProperty activeIfAlert = this.serializedObject.FindProperty("m_ActiveIfAlert");
            SerializedProperty activeIfAware = this.serializedObject.FindProperty("m_ActiveIfAware");
            
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(activeIfNotZero));
            root.Add(new PropertyField(activeIfSuspicious));
            root.Add(new PropertyField(activeIfAlert));
            root.Add(new PropertyField(activeIfAware));
            
            SerializedProperty activeIfOnscreen = this.serializedObject.FindProperty("m_ActiveIfOnscreen");
            SerializedProperty activeIfOffscreen = this.serializedObject.FindProperty("m_ActiveIfOffscreen");
            
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(activeIfOnscreen));
            root.Add(new PropertyField(activeIfOffscreen));
            
            SerializedProperty rotation = this.serializedObject.FindProperty("m_RotateTo");
            
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(rotation));
            
            SerializedProperty awareness = this.serializedObject.FindProperty("m_Awareness");
            
            root.Add(new SpaceSmaller());
            root.Add(new PropertyField(awareness));
            
            return root;
        }
    }
}
