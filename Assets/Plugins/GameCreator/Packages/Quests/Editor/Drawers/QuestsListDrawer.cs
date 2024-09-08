using GameCreator.Editor.Common;
using GameCreator.Runtime.Quests;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Quests
{
    [CustomPropertyDrawer(typeof(QuestsList))]
    public class QuestsListDrawer : TTitleDrawer
    {
        protected override string Title => "Quests";

        protected override void CreateContent(VisualElement body, SerializedProperty property)
        {
            body.Add(new SpaceSmall());
            SerializedProperty quests = property.FindPropertyRelative("m_Quests");

            int itemsCount = quests.arraySize;
            for (int i = 0; i < itemsCount; ++i)
            {
                SerializedProperty item = quests.GetArrayElementAtIndex(i);
                PropertyField itemField = new PropertyField(item, string.Empty);

                itemField.SetEnabled(false);
                body.Add(itemField);
                body.Add(new SpaceSmaller());
            }
        }
    }
}