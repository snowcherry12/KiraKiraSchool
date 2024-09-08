using GameCreator.Editor.Common;
using GameCreator.Runtime.Perception;
using GameCreator.Runtime.Perception.UnityUI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Perception
{
    [CustomEditor(typeof(SmellUI))]
    public class SmellUIEditor : UnityEditor.Editor
    {
        // [SerializeField] private ScentType m_Scents = ScentType.All;
        // [SerializeField] private PropertyGetString m_Scent = GetStringId.Create("my-scent-tag");
        // [SerializeField] private ProgressSection m_Smell = new ProgressSection();
        
        // INSPECTOR METHODS: ---------------------------------------------------------------------
        
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            SerializedProperty perception = this.serializedObject.FindProperty("m_Perception");
            SerializedProperty minIntensity = this.serializedObject.FindProperty("m_MinIntensity");
            SerializedProperty maxIntensity = this.serializedObject.FindProperty("m_MaxIntensity");
            SerializedProperty smoothTime = this.serializedObject.FindProperty("m_SmoothTime");
            
            root.Add(new SpaceSmaller());
            root.Add(new PropertyField(perception));
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
            
            SerializedProperty scents = this.serializedObject.FindProperty("m_Scents");
            SerializedProperty scent = this.serializedObject.FindProperty("m_Scent");
            
            PropertyField fieldScents = new PropertyField(scents);
            PropertyField fieldScent = new PropertyField(scent);
            
            fieldScents.RegisterValueChangeCallback(changeEvent =>
            {
                int changeIndex = changeEvent.changedProperty.enumValueIndex;
                fieldScent.style.display = changeIndex == (int) SmellUI.ScentType.Specific
                    ? DisplayStyle.Flex
                    : DisplayStyle.None;
            });
            
            fieldScent.style.display = scents.enumValueIndex == (int) SmellUI.ScentType.Specific
                ? DisplayStyle.Flex
                : DisplayStyle.None;
            
            root.Add(new SpaceSmall());
            root.Add(fieldScents);
            root.Add(fieldScent);
            
            SerializedProperty smell = this.serializedObject.FindProperty("m_Smell");
            
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(smell));

            return root;
        }
    }   
}
