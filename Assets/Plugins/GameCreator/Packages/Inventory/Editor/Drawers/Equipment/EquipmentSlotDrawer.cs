using GameCreator.Runtime.Inventory;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Inventory
{
    [CustomPropertyDrawer(typeof(EquipmentSlot))]
    public class EquipmentSlotDrawer : PropertyDrawer
    {
        public const string PROP_BASE = "m_Base";
        private const string PROP_HANDLE = "m_Handle";

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            SerializedProperty propertyBase = property.FindPropertyRelative(PROP_BASE);
            SerializedProperty propertyBone = property.FindPropertyRelative(PROP_HANDLE);
            
            PropertyField fieldBase = new PropertyField(propertyBase);
            PropertyField fieldBone = new PropertyField(propertyBone);
            
            root.Add(fieldBase);
            root.Add(fieldBone);
            
            return root;
        }
    }
}