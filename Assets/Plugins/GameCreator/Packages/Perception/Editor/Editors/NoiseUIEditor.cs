using GameCreator.Editor.Common;
using GameCreator.Runtime.Perception;
using GameCreator.Runtime.Perception.UnityUI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Perception
{
    [CustomEditor(typeof(NoiseUI))]
    public class NoiseUIEditor : UnityEditor.Editor
    {
        // INSPECTOR METHODS: ---------------------------------------------------------------------
        
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            SerializedProperty perception = this.serializedObject.FindProperty("m_Perception");
            SerializedProperty minIntensity = this.serializedObject.FindProperty("m_MinIntensity");
            SerializedProperty maxIntensity = this.serializedObject.FindProperty("m_MaxIntensity");
            
            root.Add(new SpaceSmaller());
            root.Add(new PropertyField(perception));
            root.Add(new SpaceSmaller());
            root.Add(new PropertyField(minIntensity));
            root.Add(new PropertyField(maxIntensity));
            
            SerializedProperty update = this.serializedObject.FindProperty("m_Update");
            SerializedProperty interval = this.serializedObject.FindProperty("m_Interval");
            
            PropertyField fieldUpdate = new PropertyField(update);
            PropertyField fieldInterval = new PropertyField(interval);
            
            fieldUpdate.RegisterValueChangeCallback(changeEvent =>
            {
                int changeIndex = changeEvent.changedProperty.enumValueIndex;
                fieldInterval.style.display = changeIndex == (int) UpdateInterval.Interval
                    ? DisplayStyle.Flex
                    : DisplayStyle.None;
            });
            
            fieldInterval.style.display = update.enumValueIndex == (int) UpdateInterval.Interval
                ? DisplayStyle.Flex
                : DisplayStyle.None;
            
            root.Add(new SpaceSmall());
            root.Add(fieldUpdate);
            root.Add(fieldInterval);
            
            SerializedProperty din = this.serializedObject.FindProperty("m_Din");
            SerializedProperty noise = this.serializedObject.FindProperty("m_Noise");
            
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(din));
            root.Add(new PropertyField(noise));

            return root;
        }
    }   
}
