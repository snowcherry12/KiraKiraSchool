using GameCreator.Editor.Common;
using GameCreator.Runtime.Quests.UnityUI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Quests
{
    [CustomPropertyDrawer(typeof(FormatQuestUI))]
    public class FormatQuestUIDrawer : TFormatUIDrawer
    {
        protected override void CreatePropertyContent(VisualElement container, SerializedProperty property)
        {
            base.CreatePropertyContent(container, property);
            
            SerializedProperty graphicTracking = property.FindPropertyRelative("m_GraphicTracking");
            SerializedProperty colorIsTracking = property.FindPropertyRelative("m_ColorIsTracking");
            SerializedProperty colorIsNotTracking = property.FindPropertyRelative("m_ColorIsNotTracking");

            container.Add(new SpaceSmall());
            container.Add(new PropertyField(graphicTracking));
            container.Add(new PropertyField(colorIsTracking));
            container.Add(new PropertyField(colorIsNotTracking));
        }
    }
}