using GameCreator.Editor.Common;
using GameCreator.Runtime.Dialogue;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Dialogue
{
    [CustomPropertyDrawer(typeof(Typewriter))]
    public class TypewriterDrawer : TBoxDrawer
    {
        protected override string Name(SerializedProperty property) => "Typewriter Effect";

        protected override void CreatePropertyContent(VisualElement container, SerializedProperty property)
        {
            SerializedProperty useTypewriter = property.FindPropertyRelative("m_UseTypewriter");
            SerializedProperty frequency = property.FindPropertyRelative("m_Frequency");
            SerializedProperty gibberish = property.FindPropertyRelative("m_Gibberish");
            SerializedProperty pitch = property.FindPropertyRelative("m_Pitch");

            PropertyField fieldUseTypewriter = new PropertyField(useTypewriter);
            PropertyField fieldFrequency = new PropertyField(frequency);
            PropertyField fieldGibberish = new PropertyField(gibberish);
            PropertyField fieldPitch = new PropertyField(pitch);

            VisualElement content = new VisualElement();

            container.Add(fieldUseTypewriter);
            container.Add(content);
            content.Add(new SpaceSmall());
            content.Add(fieldFrequency);
            content.Add(new SpaceSmall());
            content.Add(fieldGibberish);
            content.Add(fieldPitch);

            fieldUseTypewriter.RegisterValueChangeCallback(eventChange =>
            {
                content.SetEnabled(eventChange.changedProperty.boolValue);
            });
            
            content.SetEnabled(useTypewriter.boolValue);
        }
    }
}