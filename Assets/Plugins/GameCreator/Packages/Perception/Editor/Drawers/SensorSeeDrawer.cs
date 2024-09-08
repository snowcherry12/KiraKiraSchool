using GameCreator.Editor.Common;
using GameCreator.Runtime.Perception;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Perception
{
    [CustomPropertyDrawer(typeof(SensorSee))]
    public class SensorSeeDrawer : PropertyDrawer
    {
        private const string TITLE_PRIMARY = "Primary";
        private const string TITLE_PERIPHERAL = "Peripheral";
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement content = new VisualElement();

            SerializedProperty update = property.FindPropertyRelative("m_Update");
            SerializedProperty interval = property.FindPropertyRelative("m_Interval");
            SerializedProperty detection = property.FindPropertyRelative("m_DetectionSpeed");
            SerializedProperty layerMask = property.FindPropertyRelative("m_LayerMask");

            PropertyField fieldUpdate = new PropertyField(update);
            PropertyField fieldInterval = new PropertyField(interval);
            PropertyField fieldDetection = new PropertyField(detection);
            PropertyField fieldLayerMask = new PropertyField(layerMask);
            
            content.Add(fieldUpdate);
            content.Add(fieldInterval);
            content.Add(fieldDetection);
            content.Add(fieldLayerMask);
            
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
            
            SerializedProperty optics = property.FindPropertyRelative("m_Optics");
            SerializedProperty offsetPosition = property.FindPropertyRelative("m_OffsetPosition");
            SerializedProperty offsetRotation = property.FindPropertyRelative("m_OffsetRotation");
            
            PropertyField fieldOptics = new PropertyField(optics);
            PropertyField fieldOffsetPosition = new PropertyField(offsetPosition, "Position");
            PropertyField fieldOffsetRotation = new PropertyField(offsetRotation, "Rotation");
            
            content.Add(new SpaceSmall());
            content.Add(fieldOptics);
            content.Add(fieldOffsetPosition);
            content.Add(fieldOffsetRotation);
            
            SerializedProperty useLuminance = property.FindPropertyRelative("m_UseLuminance");
            SerializedProperty dimThreshold = property.FindPropertyRelative("m_DimThreshold");
            SerializedProperty litThreshold = property.FindPropertyRelative("m_LitThreshold");
            
            PropertyField fieldUseLuminance = new PropertyField(useLuminance);
            PropertyField fieldDimThreshold = new PropertyField(dimThreshold);
            PropertyField fieldLitThreshold = new PropertyField(litThreshold);
            
            content.Add(new SpaceSmall());
            content.Add(fieldUseLuminance);
            content.Add(fieldDimThreshold);
            content.Add(fieldLitThreshold);

            fieldDimThreshold.style.marginLeft = new Length(10, LengthUnit.Pixel);
            fieldLitThreshold.style.marginLeft = new Length(10, LengthUnit.Pixel);
            
            fieldUseLuminance.RegisterValueChangeCallback(changeEvent =>
            {
                DisplayStyle display = changeEvent.changedProperty.boolValue
                    ? DisplayStyle.Flex
                    : DisplayStyle.None;

                fieldDimThreshold.style.display = display;
                fieldLitThreshold.style.display = display;
            });
            
            fieldDimThreshold.style.display = useLuminance.boolValue ? DisplayStyle.Flex : DisplayStyle.None;
            fieldLitThreshold.style.display = useLuminance.boolValue ? DisplayStyle.Flex : DisplayStyle.None;

            ContentBox contentPrimary = new ContentBox(TITLE_PRIMARY, false);
            ContentBox contentPeripheral = new ContentBox(TITLE_PERIPHERAL, false);
            
            SerializedProperty primaryAngle = property.FindPropertyRelative("m_PrimaryAngle");
            SerializedProperty primaryRadius = property.FindPropertyRelative("m_PrimaryRadius");
            SerializedProperty primaryHeight = property.FindPropertyRelative("m_PrimaryHeight");
            
            contentPrimary.Content.Add(new PropertyField(primaryAngle));
            contentPrimary.Content.Add(new PropertyField(primaryRadius));
            contentPrimary.Content.Add(new PropertyField(primaryHeight));
            
            SerializedProperty peripheralAngle = property.FindPropertyRelative("m_PeripheralRadius");
            SerializedProperty peripheralRadius = property.FindPropertyRelative("m_PeripheralAngle");
            SerializedProperty peripheralHeight = property.FindPropertyRelative("m_PeripheralHeight");
            
            contentPeripheral.Content.Add(new PropertyField(peripheralAngle));
            contentPeripheral.Content.Add(new PropertyField(peripheralRadius));
            contentPeripheral.Content.Add(new PropertyField(peripheralHeight));
            
            content.Add(contentPrimary);
            content.Add(contentPeripheral);
            
            return content;
        }
    }
}