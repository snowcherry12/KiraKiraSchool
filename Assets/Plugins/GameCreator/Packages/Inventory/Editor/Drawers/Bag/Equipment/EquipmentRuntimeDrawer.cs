using GameCreator.Runtime.Common;
using GameCreator.Runtime.Inventory;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Inventory
{
    [CustomPropertyDrawer(typeof(EquipmentRuntime))]
    public class EquipmentRuntimeDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            Bag bag = property.serializedObject.targetObject as Bag;
            if (bag == null) return root;

            SerializedProperty values = property.FindPropertyRelative(EquipmentRuntime.NAME_VALUES);

            for (int i = 0; i < values.arraySize; ++i)
            {
                SerializedProperty item = values.GetArrayElementAtIndex(i);
                string label = bag.Equipment.GetSlotBaseName(i);
                if (string.IsNullOrEmpty(label)) continue;

                VisualElement fieldItem = EquipmentRuntimeSlotDrawer.CreatePropertyGUI(
                    item, $"Override {TextUtils.Humanize(label)}" 
                );

                root.Add(fieldItem);
            }

            return root;
        }
    }
}
