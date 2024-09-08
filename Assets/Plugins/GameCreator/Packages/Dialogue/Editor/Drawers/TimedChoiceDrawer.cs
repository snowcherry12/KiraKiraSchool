using GameCreator.Runtime.Dialogue;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Dialogue
{
    [CustomPropertyDrawer(typeof(TimedChoice))]
    public class TimedChoiceDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            SerializedProperty timedChoice = property.FindPropertyRelative("m_TimedChoice");
            SerializedProperty duration = property.FindPropertyRelative("m_Duration");
            SerializedProperty timeout = property.FindPropertyRelative("m_Timeout");

            PropertyField fieldTimedChoice = new PropertyField(timedChoice);
            PropertyField fieldDuration = new PropertyField(duration);
            PropertyField fieldTimeout = new PropertyField(timeout);
            
            root.Add(fieldTimedChoice);
            root.Add(fieldDuration);
            root.Add(fieldTimeout);

            fieldTimedChoice.RegisterValueChangeCallback(changeEvent =>
            {
                bool state = changeEvent.changedProperty.boolValue;
                fieldDuration.style.display = state ? DisplayStyle.Flex : DisplayStyle.None;
                fieldTimeout.style.display = state ? DisplayStyle.Flex : DisplayStyle.None;
            });
            
            bool state = timedChoice.boolValue;
            fieldDuration.style.display = state ? DisplayStyle.Flex : DisplayStyle.None;
            fieldTimeout.style.display = state ? DisplayStyle.Flex : DisplayStyle.None;
            
            return root;
        }
    }
}