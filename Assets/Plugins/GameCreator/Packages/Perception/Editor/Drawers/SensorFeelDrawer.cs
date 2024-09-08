using GameCreator.Editor.Common;
using GameCreator.Runtime.Perception;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Perception
{
    [CustomPropertyDrawer(typeof(SensorFeel))]
    public class SensorFeelDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement content = new VisualElement();

            SerializedProperty update = property.FindPropertyRelative("m_Update");
            SerializedProperty interval = property.FindPropertyRelative("m_Interval");

            PropertyField fieldUpdate = new PropertyField(update);
            PropertyField fieldInterval = new PropertyField(interval);
            
            content.Add(fieldUpdate);
            content.Add(fieldInterval);
            
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
            
            SerializedProperty detection = property.FindPropertyRelative("m_DetectionSpeed");
            SerializedProperty radius = property.FindPropertyRelative("m_Radius");
            SerializedProperty layerMask = property.FindPropertyRelative("m_LayerMask");
            
            PropertyField fieldDetection = new PropertyField(detection);
            PropertyField fieldRadius = new PropertyField(radius);
            PropertyField fieldLayerMask = new PropertyField(layerMask);
            
            content.Add(new SpaceSmall());
            content.Add(fieldDetection);
            content.Add(fieldRadius);
            content.Add(fieldLayerMask);
            
            return content;
        }
    }
}