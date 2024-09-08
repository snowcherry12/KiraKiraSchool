using GameCreator.Editor.Common;
using GameCreator.Runtime.Melee;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Melee
{
    [CustomPropertyDrawer(typeof(SkillTrail))]
    public class SkillTrailDrawer : TBoxDrawer
    {
        protected override string Name(SerializedProperty property) => "Trail";

        protected override void CreatePropertyContent(VisualElement container, SerializedProperty property)
        {
            SerializedProperty isActive = property.FindPropertyRelative("m_IsActive");
            SerializedProperty length = property.FindPropertyRelative("m_Length");
            SerializedProperty quads = property.FindPropertyRelative("m_Quads");
            SerializedProperty material = property.FindPropertyRelative("m_Material");

            PropertyField fieldIsActive = new PropertyField(isActive);
            PropertyField fieldLength = new PropertyField(length);
            PropertyField fieldQuads = new PropertyField(quads);
            PropertyField fieldMaterial = new PropertyField(material);
            
            VisualElement content = new VisualElement();
            
            container.Add(fieldIsActive);
            container.Add(new SpaceSmallest());
            container.Add(content);
            
            content.Add(fieldLength);
            content.Add(fieldQuads);
            content.Add(fieldMaterial);
            
            content.SetEnabled(isActive.boolValue);
            fieldIsActive.RegisterValueChangeCallback(changeEvent =>
            {
                content.SetEnabled(changeEvent.changedProperty.boolValue);
            });
        }
    }
}