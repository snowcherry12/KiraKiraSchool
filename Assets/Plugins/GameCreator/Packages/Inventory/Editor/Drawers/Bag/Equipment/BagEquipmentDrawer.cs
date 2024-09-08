using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Inventory;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Inventory
{
    [CustomPropertyDrawer(typeof(BagEquipment), true)]
    public class BagEquipmentDrawer : PropertyDrawer
    {
        private const string PROP_EQUIPMENT = "m_Equipment";
        private const string PROP_EQUIPMENT_RUNTIME = "m_EquipmentRuntime";

        private const string NAME_EQUIP = "GC-Inventory-Bag-Runtime-Equipment-Slot";
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();
            VisualElement head = new VisualElement();
            VisualElement body = new VisualElement();

            root.Add(head);
            root.Add(body);

            SerializedProperty equipment = property.FindPropertyRelative(PROP_EQUIPMENT);
            PropertyField fieldEquipment = new PropertyField(equipment);
            
            head.Add(new SpaceSmall());
            head.Add(fieldEquipment);
            
            fieldEquipment.SetEnabled(!EditorApplication.isPlayingOrWillChangePlaymode);
            fieldEquipment.RegisterValueChangeCallback(_ =>
            {
                this.PaintBody(property, body);
            });
            
            this.PaintBody(property, body);

            return root;
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void PaintBody(SerializedProperty property, VisualElement body)
        {
            property.serializedObject.ApplyModifiedProperties();
            property.serializedObject.Update();
            
            body.Clear();

            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                Bag bag = property.serializedObject.targetObject as Bag;
                if (bag != null) this.RuntimeContent(bag.Equipment, body);
            }
            else
            {
                this.EditorContent(property, body);   
            }
        }
        
        // ABSTRACT METHODS: ----------------------------------------------------------------------

        protected virtual void EditorContent(SerializedProperty property, VisualElement body)
        {
            SerializedProperty runtime = property.FindPropertyRelative(PROP_EQUIPMENT_RUNTIME);
            PropertyField fieldRuntime = new PropertyField(runtime);
            
            body.Add(fieldRuntime);
            body.Bind(property.serializedObject);
        }

        protected virtual void RuntimeContent(IBagEquipment equipment, VisualElement body)
        {
            int equipmentSlotCount = equipment.Count;
            if (equipmentSlotCount <= 0) return;

            VisualElement content = new VisualElement { name = NAME_EQUIP };
            for (int i = 0; i < equipmentSlotCount; ++i)
            {
                IdString baseItemID = equipment.GetSlotBaseID(i);
                IdString equippedRuntimeItemID = equipment.GetSlotRootRuntimeItemID(i);

                if (string.IsNullOrEmpty(baseItemID.String)) continue;
                TextField text = new TextField
                {
                    label = baseItemID.String,
                    value = equippedRuntimeItemID.String
                };

                text.SetEnabled(false);
                content.Add(text);
            }
            
            body.Add(content);
        }
    }
}