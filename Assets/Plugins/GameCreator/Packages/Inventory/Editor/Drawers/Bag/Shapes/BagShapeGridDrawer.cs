using GameCreator.Editor.Common;
using GameCreator.Runtime.Inventory;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Inventory
{
    [CustomPropertyDrawer(typeof(BagShapeGrid), true)]
    public class BagShapeGridDrawer : TBagShapeWithWeightDrawer
    {
        protected override void EditorContent(SerializedProperty property, VisualElement root)
        {
            SerializedProperty width = property.FindPropertyRelative("m_Width");
            SerializedProperty height = property.FindPropertyRelative("m_Height");

            PropertyField fieldWidth = new PropertyField(width);
            PropertyField fieldHeight = new PropertyField(height);
            
            root.Add(fieldWidth);
            root.Add(fieldHeight);
            
            root.Add(new SpaceSmall());
            base.EditorContent(property, root);
        }

        protected override void RuntimeContent(IBagShape shape, VisualElement root)
        {
            base.RuntimeContent(shape, root);
            
            string width = shape.HasInfiniteWidth ? "Infinite" : shape.MaxWidth.ToString();
            string height = shape.HasInfiniteHeight ? "Infinite" : shape.MaxHeight.ToString();
            
            TextField fieldWidth = new TextField("Width") { value = width };
            TextField fieldHeight = new TextField("Height") { value = height };
            
            fieldWidth.SetEnabled(false);
            fieldHeight.SetEnabled(false);
            
            root.Add(fieldWidth);
            root.Add(fieldHeight);
        }
    }
}