using GameCreator.Editor.Common;
using GameCreator.Runtime.Melee;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Melee
{
    [CustomPropertyDrawer(typeof(SkillStrike))]
    public class SkillStrikeDrawer : TBoxDrawer
    {
        private const string EMPTY = " ";
        
        protected override string Name(SerializedProperty property) => "Strike";

        protected override void CreatePropertyContent(VisualElement container, SerializedProperty property)
        {
            SerializedProperty direction = property.FindPropertyRelative("m_Direction");
            SerializedProperty predictions = property.FindPropertyRelative("m_Predictions");

            container.Add(new PropertyField(direction));
            container.Add(new PropertyField(predictions));
            
            SerializedProperty useStrikers = property.FindPropertyRelative("m_UseStrikers");
            SerializedProperty id = property.FindPropertyRelative("m_Id");

            PropertyField fieldUseStrikers = new PropertyField(useStrikers);
            PropertyField fieldId = new PropertyField(id, EMPTY);
            
            container.Add(new SpaceSmall());
            container.Add(fieldUseStrikers);
            container.Add(fieldId);

            fieldId.style.display = useStrikers.enumValueIndex == (int) MeleeStrikers.ById
                ? DisplayStyle.Flex
                : DisplayStyle.None;

            fieldUseStrikers.RegisterValueChangeCallback(change =>
            {
                int modeIndex = change.changedProperty.enumValueIndex;
                fieldId.style.display = modeIndex == (int) MeleeStrikers.ById
                    ? DisplayStyle.Flex
                    : DisplayStyle.None;
            });
        }
    }
}