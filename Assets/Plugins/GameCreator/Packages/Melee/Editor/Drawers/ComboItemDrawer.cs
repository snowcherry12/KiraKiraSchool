using GameCreator.Editor.Common;
using GameCreator.Runtime.Melee;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Melee
{
    [CustomPropertyDrawer(typeof(ComboItem))]
    public class ComboItemDrawer : PropertyDrawer
    {
        public const string PROP_IS_DISABLED = "m_IsDisabled";
        
        private const string ERR_SKILL = "A combo requires a Skill asset reference";
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            return Paint(property, true);
        }

        public static VisualElement Paint(SerializedProperty property, bool enableDelay)
        {
            VisualElement root = new VisualElement();

            SerializedProperty key = property.FindPropertyRelative("m_Key");
            SerializedProperty mode = property.FindPropertyRelative("m_Mode");

            PropertyField fieldKey = new PropertyField(key, string.Empty);
            PropertyField fieldMode = new PropertyField(mode, string.Empty);

            root.Add(fieldKey);
            root.Add(fieldMode);

            SerializedProperty autoRelease = property.FindPropertyRelative("m_AutoRelease");
            SerializedProperty timeout = property.FindPropertyRelative("m_Timeout");
            SerializedProperty hasDelay = property.FindPropertyRelative("m_HasDelay");

            PropertyField fieldAutoRelease = new PropertyField(autoRelease);
            PropertyField fieldTimeout = new PropertyField(timeout);
            PropertyField fieldHasDelay = new PropertyField(hasDelay);
            
            if (!enableDelay) fieldHasDelay.SetEnabled(false);

            root.Add(fieldAutoRelease);
            root.Add(fieldTimeout);
            root.Add(fieldHasDelay);
            
            fieldMode.RegisterValueChangeCallback(changeEvent =>
            {
                fieldAutoRelease.style.display = changeEvent.changedProperty.enumValueIndex == 1
                    ? DisplayStyle.Flex
                    : DisplayStyle.None;
                
                fieldTimeout.style.display = changeEvent.changedProperty.enumValueIndex == 1
                    ? DisplayStyle.Flex
                    : DisplayStyle.None;
            });

            fieldAutoRelease.style.display = mode.enumValueIndex == 1
                ? DisplayStyle.Flex
                : DisplayStyle.None;
            
            fieldTimeout.style.display = mode.enumValueIndex == 1
                ? DisplayStyle.Flex
                : DisplayStyle.None;
            
            SerializedProperty conditions = property.FindPropertyRelative("m_Conditions");
            
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(conditions));
            
            SerializedProperty when = property.FindPropertyRelative("m_When");
            SerializedProperty skill = property.FindPropertyRelative("m_Skill");
            
            PropertyField fieldWhen = new PropertyField(when, string.Empty);
            ErrorMessage errorFieldSkill = new ErrorMessage(ERR_SKILL);
            PropertyField fieldSkill = new PropertyField(skill, string.Empty);
            
            root.Add(new SpaceSmall());
            root.Add(fieldWhen);
            root.Add(errorFieldSkill);
            root.Add(fieldSkill);

            fieldSkill.RegisterValueChangeCallback(changeEvent =>
            {
                Object reference = changeEvent.changedProperty.objectReferenceValue; 
                errorFieldSkill.style.display = reference == null
                    ? DisplayStyle.Flex
                    : DisplayStyle.None;
            });
            
            errorFieldSkill.style.display = skill.objectReferenceValue == null
                ? DisplayStyle.Flex
                : DisplayStyle.None;
            
            return root;
        }
    }
}