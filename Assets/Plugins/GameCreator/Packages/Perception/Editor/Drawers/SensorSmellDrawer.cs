using GameCreator.Editor.Common;
using GameCreator.Runtime.Perception;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Perception
{
    [CustomPropertyDrawer(typeof(SensorSmell))]
    public class SensorSmellDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement content = new VisualElement();

            SerializedProperty update = property.FindPropertyRelative("m_Update");
            SerializedProperty interval = property.FindPropertyRelative("m_Interval");
            SerializedProperty radius = property.FindPropertyRelative("m_Radius");
            SerializedProperty useObstruction = property.FindPropertyRelative("m_UseObstruction");
            SerializedProperty minIntensity = property.FindPropertyRelative("m_MinIntensity");

            PropertyField fieldUpdate = new PropertyField(update);
            PropertyField fieldInterval = new PropertyField(interval);
            PropertyField fieldRadius = new PropertyField(radius);
            PropertyField fieldUseObstruction = new PropertyField(useObstruction);
            PropertyField fieldMinIntensity = new PropertyField(minIntensity);
            
            content.Add(fieldUpdate);
            content.Add(fieldInterval);
            content.Add(new SpaceSmall());
            content.Add(fieldRadius);
            content.Add(fieldUseObstruction);
            content.Add(fieldMinIntensity);
            
            fieldUpdate.RegisterValueChangeCallback(changeEvent =>
            {
                int updateMode = changeEvent.changedProperty.enumValueIndex;
                fieldInterval.style.display = updateMode == (int) UpdateMode.Interval
                    ? DisplayStyle.Flex
                    : DisplayStyle.None;
            });
            
            fieldInterval.style.display = update.enumValueIndex == (int) UpdateMode.Interval
                ? DisplayStyle.Flex
                : DisplayStyle.None;
            
            return content;
        }
    }
}