using GameCreator.Editor.Common;
using GameCreator.Runtime.Inventory;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Inventory
{
    [CustomPropertyDrawer(typeof(Equip))]
    public class EquipDrawer : TBoxDrawer
    {
        protected override string Name(SerializedProperty property) => "Equipment";

        protected override void CreatePropertyContent(VisualElement container, SerializedProperty property)
        {
            VisualElement head = new VisualElement();
            VisualElement body = new VisualElement();
            VisualElement foot = new VisualElement();

            container.Add(head);
            container.Add(body);
            container.Add(foot);
            
            SerializedProperty isEquippable = property.FindPropertyRelative("m_IsEquippable");
            SerializedProperty prefab = property.FindPropertyRelative("m_Prefab");
            SerializedProperty doEquip = property.FindPropertyRelative("m_ConditionsEquip");
            SerializedProperty onEquip = property.FindPropertyRelative("m_InstructionsOnEquip");
            SerializedProperty onUnequip = property.FindPropertyRelative("m_InstructionsOnUnequip");
            SerializedProperty inherit = property.FindPropertyRelative("m_ExecuteFromParent");

            PropertyField fieldIsEquippable = new PropertyField(isEquippable);
            PropertyField fieldPrefab = new PropertyField(prefab);
            PropertyField fieldDoEquip = new PropertyField(doEquip);
            PropertyField fieldOnEquip = new PropertyField(onEquip);
            PropertyField fieldOnUnequip = new PropertyField(onUnequip);
            PropertyField fieldInherit = new PropertyField(inherit);

            head.Add(fieldIsEquippable);
            body.Add(fieldPrefab);

            foot.Add(new SpaceSmall());
            foot.Add(new LabelTitle("Can Equip"));
            foot.Add(fieldDoEquip);
            
            foot.Add(new SpaceSmall());
            foot.Add(new LabelTitle("On Equip"));
            foot.Add(fieldOnEquip);
            
            foot.Add(new SpaceSmall());
            foot.Add(new LabelTitle("On Unequip"));
            foot.Add(fieldOnUnequip);
            
            foot.Add(new SpaceSmall());
            foot.Add(fieldInherit);
            
            body.SetEnabled(isEquippable.boolValue);
            fieldIsEquippable.RegisterValueChangeCallback(changeEvent =>
            {
                body.SetEnabled(changeEvent.changedProperty.boolValue);
            });
        }
    }
}