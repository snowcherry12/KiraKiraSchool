using GameCreator.Editor.Common;
using GameCreator.Runtime.Stats;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Stats
{
    [CustomPropertyDrawer(typeof(StatusEffectData))]
    public class StatusEffectDataDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            SerializedProperty type = property.FindPropertyRelative("m_Type");
            SerializedProperty maxStack = property.FindPropertyRelative("m_MaxStack");
            
            SerializedProperty isHidden = property.FindPropertyRelative("m_IsHidden");
            SerializedProperty hasDuration = property.FindPropertyRelative("m_HasDuration");
            SerializedProperty duration = property.FindPropertyRelative("m_Duration");

            PropertyField fieldType = new PropertyField(type);
            PropertyField fieldMaxStack = new PropertyField(maxStack);
            PropertyField fieldIsHidden = new PropertyField(isHidden);
            PropertyField fieldHasDuration = new PropertyField(hasDuration);
            PropertyField fieldDuration = new PropertyField(duration);

            VisualElement durationContent = new VisualElement();
            
            root.Add(fieldType);
            root.Add(fieldMaxStack);
            root.Add(new SpaceSmall());
            root.Add(fieldIsHidden);
            root.Add(fieldHasDuration);
            root.Add(durationContent);

            if (hasDuration.boolValue) durationContent.Add(fieldDuration);
            fieldHasDuration.RegisterValueChangeCallback(changeEvent =>
            {
                durationContent.Clear();
                if (changeEvent.changedProperty.boolValue)
                {
                    durationContent.Add(fieldDuration);
                    fieldDuration.Bind(property.serializedObject);
                }
            });
            
            return root;
        }
    }
}