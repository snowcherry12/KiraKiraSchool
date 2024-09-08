using GameCreator.Editor.Common;
using GameCreator.Runtime.Perception.UnityUI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Perception
{
    [CustomEditor(typeof(IndicatorAwarenessUI))]
    public class IndicatorAwarenessUIEditor : UnityEditor.Editor
    {
        // INSPECTOR METHODS: ---------------------------------------------------------------------
        
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            SerializedProperty camera = this.serializedObject.FindProperty("m_Camera");
            SerializedProperty targetObject = this.serializedObject.FindProperty("m_Target");
            SerializedProperty radius = this.serializedObject.FindProperty("m_Radius");
            
            root.Add(new SpaceSmaller());
            root.Add(new PropertyField(camera));
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(targetObject));
            root.Add(new PropertyField(radius));
            
            SerializedProperty colorNone = this.serializedObject.FindProperty("m_ColorNone");
            SerializedProperty colorSuspicious = this.serializedObject.FindProperty("m_ColorSuspicious");
            SerializedProperty colorAlert = this.serializedObject.FindProperty("m_ColorAlert");
            SerializedProperty colorAware = this.serializedObject.FindProperty("m_ColorAware");
            
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(colorNone));
            root.Add(new SpaceSmaller());
            root.Add(new PropertyField(colorSuspicious));
            root.Add(new SpaceSmaller());
            root.Add(new PropertyField(colorAlert));
            root.Add(new SpaceSmaller());
            root.Add(new PropertyField(colorAware));
            
            SerializedProperty prefab = this.serializedObject.FindProperty("m_Prefab");
            SerializedProperty content = this.serializedObject.FindProperty("m_Content");
            
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(prefab));
            root.Add(new PropertyField(content));
            
            SerializedProperty keepInBounds = this.serializedObject.FindProperty("m_KeepInBounds");
            SerializedProperty filter = this.serializedObject.FindProperty("m_Filter");
            
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(keepInBounds));
            root.Add(new LabelTitle("Filter:"));
            root.Add(new SpaceSmaller());
            root.Add(new PropertyField(filter));
            
            return root;
        }
    }
}
