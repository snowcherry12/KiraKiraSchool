using GameCreator.Editor.Common;
using GameCreator.Runtime.Stats;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Stats
{
    [CustomPropertyDrawer(typeof(TInfo), true)]
    public class TInfoDrawer : TBoxDrawer
    {
        protected override string Name(SerializedProperty property) => "UI";

        protected override void CreatePropertyContent(VisualElement container, SerializedProperty property)
        {
            SerializedProperty name = property.FindPropertyRelative("m_Name");
            SerializedProperty acronym = property.FindPropertyRelative("m_Acronym");
            SerializedProperty description = property.FindPropertyRelative("m_Description");
            SerializedProperty icon = property.FindPropertyRelative("m_Icon");
            SerializedProperty color = property.FindPropertyRelative("m_Color");
            
            container.Add(new PropertyField(name));
            container.Add(new SpaceSmaller());
            container.Add(new PropertyField(acronym));
            container.Add(new SpaceSmaller());
            container.Add(new PropertyField(description));
            container.Add(new SpaceSmaller());
            container.Add(new PropertyField(icon));
            container.Add(new SpaceSmaller());
            container.Add(new PropertyField(color));
        }
    }
}