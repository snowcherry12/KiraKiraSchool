using GameCreator.Editor.Common;
using GameCreator.Runtime.Shooter;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Shooter
{
    [CustomPropertyDrawer(typeof(HumanFreeHand))]
    public class HumanFreeHandDrawer : TSectionDrawer
    {
        protected override string Name(SerializedProperty property) => property.displayName;

        protected override void CreatePropertyContent(VisualElement container, SerializedProperty property)
        {
            SerializedProperty useFreeHand = property.FindPropertyRelative("m_UseFreeHand");
            SerializedProperty attach = property.FindPropertyRelative("m_Attach");
            SerializedProperty transform = property.FindPropertyRelative("m_Transform");
            SerializedProperty position = property.FindPropertyRelative("m_Position");
            SerializedProperty rotation = property.FindPropertyRelative("m_Rotation");
            SerializedProperty pole = property.FindPropertyRelative("m_Pole");
            
            container.Add(new PropertyField(useFreeHand));

            PropertyField fieldAttach = new PropertyField(attach);
            PropertyField fieldTransform = new PropertyField(transform);
            
            container.Add(fieldAttach);
            container.Add(fieldTransform);

            fieldTransform.style.display = attach.enumValueIndex == 0
                ? DisplayStyle.None
                : DisplayStyle.Flex;
            
            fieldAttach.RegisterValueChangeCallback(changeEvent =>
            {
                fieldTransform.style.display = changeEvent.changedProperty.enumValueIndex == 0
                    ? DisplayStyle.None
                    : DisplayStyle.Flex;
            });
            
            container.Add(new PropertyField(position));
            container.Add(new PropertyField(rotation));
            container.Add(new PropertyField(pole));
        }
    }
}