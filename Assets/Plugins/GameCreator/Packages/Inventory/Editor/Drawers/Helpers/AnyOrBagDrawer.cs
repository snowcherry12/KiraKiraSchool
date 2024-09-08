using GameCreator.Runtime.Inventory;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Inventory
{
    [CustomPropertyDrawer(typeof(AnyOrBag))]
    public class AnyOrBagDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            SerializedProperty option = property.FindPropertyRelative("m_Option");
            SerializedProperty bag = property.FindPropertyRelative("m_Bag");
            
            PropertyField fieldOption = new PropertyField(option, property.displayName);
            PropertyField fieldBag = new PropertyField(bag);
            
            root.Add(fieldOption);
            root.Add(fieldBag);
            
            fieldOption.RegisterValueChangeCallback(changeEvent =>
            {
                fieldBag.style.display = changeEvent.changedProperty.enumValueIndex == 0
                    ? DisplayStyle.None
                    : DisplayStyle.Flex;
            });

            fieldBag.style.display = option.enumValueIndex == 0
                ? DisplayStyle.None
                : DisplayStyle.Flex;
            
            return root;
        }
    }
}
