using GameCreator.Editor.Common;
using GameCreator.Runtime.Inventory;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Inventory
{
    [CustomPropertyDrawer(typeof(Shape))]
    public class ShapeDrawer : TBoxDrawer
    {
        protected override string Name(SerializedProperty property) => "Shape";

        protected override void CreatePropertyContent(VisualElement container, SerializedProperty property)
        {
            SerializedProperty width = property.FindPropertyRelative("m_Width");
            SerializedProperty height = property.FindPropertyRelative("m_Height");
            SerializedProperty weight = property.FindPropertyRelative("m_Weight");
            SerializedProperty maxStack = property.FindPropertyRelative("m_MaxStack");

            PropertyField fieldWidth = new PropertyField(width);
            PropertyField fieldHeight = new PropertyField(height);
            PropertyField fieldWeight = new PropertyField(weight);
            PropertyField fieldMaxStack = new PropertyField(maxStack);

            container.Add(fieldWidth);
            container.Add(fieldHeight);
            container.Add(new SpaceSmall());
            container.Add(fieldWeight);
            container.Add(fieldMaxStack);
            
            Item item = maxStack.serializedObject.targetObject as Item;
            fieldMaxStack.SetEnabled(item != null && item.Sockets.ListLength == 0);
        }
    }
}