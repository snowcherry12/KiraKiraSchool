using GameCreator.Runtime.Inventory;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Inventory
{
    [CustomPropertyDrawer(typeof(AnyOrItem))]
    public class AnyOrItemDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            SerializedProperty option = property.FindPropertyRelative("m_Option");
            SerializedProperty item = property.FindPropertyRelative("m_Item");
            
            PropertyField fieldOption = new PropertyField(option, property.displayName);
            PropertyField fieldItem = new PropertyField(item);
            
            root.Add(fieldOption);
            root.Add(fieldItem);
            
            fieldOption.RegisterValueChangeCallback(changeEvent =>
            {
                fieldItem.style.display = changeEvent.changedProperty.enumValueIndex == 0
                    ? DisplayStyle.None
                    : DisplayStyle.Flex;
            });

            fieldItem.style.display = option.enumValueIndex == 0
                ? DisplayStyle.None
                : DisplayStyle.Flex;
            
            return root;
        }
    }
}
