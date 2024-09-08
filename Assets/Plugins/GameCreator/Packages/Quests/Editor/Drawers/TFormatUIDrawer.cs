using GameCreator.Editor.Common;
using GameCreator.Runtime.Quests.UnityUI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Quests
{
    public abstract class TFormatUIDrawer : TBoxDrawer
    {
        protected override string Name(SerializedProperty property) => "Style Graphics";

        protected override void CreatePropertyContent(VisualElement container, SerializedProperty property)
        {
            SerializedProperty graphicStatus = property.FindPropertyRelative("m_GraphicStatus");
            SerializedProperty colorIsInactive = property.FindPropertyRelative("m_ColorIsInactive");
            SerializedProperty colorIsActive = property.FindPropertyRelative("m_ColorIsActive");
            SerializedProperty colorIsCompleted = property.FindPropertyRelative("m_ColorIsCompleted");
            SerializedProperty colorIsAbandoned = property.FindPropertyRelative("m_ColorIsAbandoned");
            SerializedProperty colorIsFailed = property.FindPropertyRelative("m_ColorIsFailed");

            SerializedProperty graphicSelected = property.FindPropertyRelative("m_GraphicSelected");
            SerializedProperty colorNormal = property.FindPropertyRelative("m_ColorNormal");
            SerializedProperty colorSelected = property.FindPropertyRelative("m_ColorSelected");
            
            container.Add(new PropertyField(graphicStatus));
            container.Add(new PropertyField(colorIsInactive));
            container.Add(new PropertyField(colorIsActive));
            container.Add(new PropertyField(colorIsCompleted));
            container.Add(new PropertyField(colorIsAbandoned));
            container.Add(new PropertyField(colorIsFailed));
            
            container.Add(new SpaceSmall());
            container.Add(new PropertyField(graphicSelected));
            container.Add(new PropertyField(colorNormal));
            container.Add(new PropertyField(colorSelected));
        }
    }
}