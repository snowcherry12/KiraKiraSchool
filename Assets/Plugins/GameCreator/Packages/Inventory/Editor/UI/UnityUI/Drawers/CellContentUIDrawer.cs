using GameCreator.Editor.Common;
using GameCreator.Runtime.Inventory.UnityUI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Inventory.UnityUI
{
    [CustomPropertyDrawer(typeof(CellContentUI))]
    public class CellContentUIDrawer : RuntimeItemUIDrawer
    {
        protected override void AddBefore(VisualElement root, SerializedProperty property)
        {
            SerializedProperty activeCornerTL = property.FindPropertyRelative("m_ActiveCornerTopLeft");
            SerializedProperty activeCornerTR = property.FindPropertyRelative("m_ActiveCornerTopRight");
            SerializedProperty activeCornerBL = property.FindPropertyRelative("m_ActiveCornerBottomLeft");
            SerializedProperty activeCornerBR = property.FindPropertyRelative("m_ActiveCornerBottomRight");
            
            root.Add(new PropertyField(activeCornerTL));
            root.Add(new PropertyField(activeCornerTR));
            root.Add(new PropertyField(activeCornerBL));
            root.Add(new PropertyField(activeCornerBR));
            
            root.Add(new SpaceSmall());
            base.AddBefore(root, property);
        }

        protected override void AddAfter(VisualElement root, SerializedProperty property)
        {
            base.AddAfter(root, property);
            
            SerializedProperty displayStack = property.FindPropertyRelative("m_DisplayStack");
            SerializedProperty stackContent = property.FindPropertyRelative("m_StackContent");
            SerializedProperty stackCount = property.FindPropertyRelative("m_StackCount");
            
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(displayStack));
            root.Add(new PropertyField(stackContent));
            root.Add(new PropertyField(stackCount));
        }
    }
}
