using System.Collections.Generic;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Inventory;
using GameCreator.Runtime.Inventory.UnityUI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Inventory.UnityUI
{
    [CustomPropertyDrawer(typeof(EquipmentIndex))]
    public class EquipmentIndexDrawer : PropertyDrawer
    {
        private const string PROP_EQUIPMENT = "m_Equipment";
        private const string PROP_INDEX = "m_Index";
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();
            VisualElement head = new VisualElement();
            VisualElement body = new VisualElement();

            root.Add(head);
            root.Add(body);
            
            SerializedProperty equipmentAsset = property.FindPropertyRelative(PROP_EQUIPMENT);
            PropertyField fieldEquipment = new PropertyField(equipmentAsset);
            head.Add(fieldEquipment);
            
            this.Refresh(body, property);
            fieldEquipment.RegisterValueChangeCallback(_ =>
            {
                property.serializedObject.ApplyModifiedProperties();
                property.serializedObject.Update();
                
                this.Refresh(body, property);
            });
            
            return root;
        }

        private void Refresh(VisualElement body, SerializedProperty property)
        {
            body.Clear();
            
            SerializedProperty equipmentAsset = property.FindPropertyRelative(PROP_EQUIPMENT);
            SerializedProperty equipmentIndex = property.FindPropertyRelative(PROP_INDEX);

            List<int> indexes = new List<int>();
            List<string> names = new List<string>();

            Equipment equipment = equipmentAsset.objectReferenceValue != null
                ? equipmentAsset.objectReferenceValue as Equipment
                : null;
            
            if (equipment != null)
            {
                for (int i = 0; i < equipment.Slots.Length; ++i)
                {
                    EquipmentSlot slot = equipment.Slots[i];
                    if (slot.Base == null) continue;
                    
                    indexes.Add(i);
                    names.Add(TextUtils.Humanize(slot.Base.name));
                }
            }
            
            PopupField<int> fieldIndex = new PopupField<int>(
                " ",  indexes, equipmentIndex.intValue, 
                index => index < names.Count ? $"{names[index]} ({index})" : "(none)",
                index => index < names.Count ? $"{index}: {names[index]}" : "(none)"
            );

            fieldIndex.RegisterValueChangedCallback(changeEvent =>
            {
                equipmentIndex.intValue = changeEvent.newValue;
                
                equipmentIndex.serializedObject.ApplyModifiedProperties();
                equipmentIndex.serializedObject.Update();
            });
            
            fieldIndex.SetEnabled(equipment != null);
            body.Add(fieldIndex);
            
            fieldIndex.AddToClassList(AlignLabel.CLASS_UNITY_ALIGN_LABEL);
            AlignLabel.On(fieldIndex);
        }
    }
}