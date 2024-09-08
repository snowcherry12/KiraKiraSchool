using GameCreator.Editor.Common;
using GameCreator.Runtime.Quests;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Quests
{
    [CustomPropertyDrawer(typeof(CompareQuestOrAny))]
    public class CompareQuestOrAnyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();
            VisualElement head = new VisualElement();
            VisualElement body = new VisualElement();

            SerializedProperty option = property.FindPropertyRelative("m_Option");
            SerializedProperty quest = property.FindPropertyRelative("m_Quest");
            
            PropertyField fieldOption = new PropertyField(option, property.displayName);
            
            PropertyElement fieldQuest = new PropertyElement(
                quest.FindPropertyRelative(IPropertyDrawer.PROPERTY_NAME),
                string.Empty, true
            );
            
            head.Add(fieldOption);
            
            fieldOption.RegisterValueChangeCallback(changeEvent =>
            {
                body.Clear();
                if (changeEvent.changedProperty.intValue != 1) return;
                body.Add(fieldQuest);
                body.Bind(changeEvent.changedProperty.serializedObject);
            });

            if (option.intValue == 1)
            {
                body.Add(fieldQuest);
            }

            root.Add(head);
            root.Add(body);
            
            return root;
        }
    }
}
