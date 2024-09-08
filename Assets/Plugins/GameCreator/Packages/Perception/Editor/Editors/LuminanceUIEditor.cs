using GameCreator.Editor.Common;
using GameCreator.Runtime.Perception;
using GameCreator.Runtime.Perception.UnityUI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Perception
{
    [CustomEditor(typeof(LuminanceUI))]
    public class LuminanceUIEditor : UnityEditor.Editor
    {
        // INSPECTOR METHODS: ---------------------------------------------------------------------
        
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            SerializedProperty targetObject = this.serializedObject.FindProperty("m_Target");
            SerializedProperty minIntensity = this.serializedObject.FindProperty("m_MinIntensity");
            SerializedProperty maxIntensity = this.serializedObject.FindProperty("m_MaxIntensity");
            SerializedProperty smoothTime = this.serializedObject.FindProperty("m_SmoothTime");
            
            root.Add(new SpaceSmaller());
            root.Add(new PropertyField(targetObject));
            root.Add(new SpaceSmaller());
            root.Add(new PropertyField(minIntensity));
            root.Add(new PropertyField(maxIntensity));
            root.Add(new PropertyField(smoothTime));
            
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
            
            SerializedProperty lit = this.serializedObject.FindProperty("m_Lit");
            SerializedProperty dim = this.serializedObject.FindProperty("m_Dim");
            
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(lit));
            root.Add(new PropertyField(dim));

            return root;
        }
    }
}
