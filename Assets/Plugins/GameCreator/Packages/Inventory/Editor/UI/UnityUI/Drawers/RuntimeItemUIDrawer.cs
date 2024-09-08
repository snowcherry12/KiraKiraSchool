using GameCreator.Editor.Common;
using GameCreator.Runtime.Inventory.UnityUI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Inventory.UnityUI
{
    [CustomPropertyDrawer(typeof(RuntimeItemUI))]
    public class RuntimeItemUIDrawer : TItemUIDrawer
    {
        protected override void AddBefore(VisualElement root, SerializedProperty property)
        {
            base.AddBefore(root, property);
            SerializedProperty activeContent = property.FindPropertyRelative("m_ActiveContent");
            SerializedProperty activeEquipped = property.FindPropertyRelative("m_ActiveEquipped");
            SerializedProperty activeNotEquipped = property.FindPropertyRelative("m_ActiveNotEquipped");

            root.Add(new PropertyField(activeContent));
            root.Add(new PropertyField(activeEquipped));
            root.Add(new PropertyField(activeNotEquipped));
            root.Add(new SpaceSmall());
        }

        protected override void AddAfter(VisualElement root, SerializedProperty property)
        {
            base.AddAfter(root, property);
            SerializedProperty cooldownProgress = property.FindPropertyRelative("m_CooldownProgress");
            SerializedProperty activeInCooldown = property.FindPropertyRelative("m_ActiveInCooldown");
            SerializedProperty activeNotCooldown = property.FindPropertyRelative("m_ActiveNotCooldown");
            
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(cooldownProgress));
            root.Add(new PropertyField(activeInCooldown));
            root.Add(new PropertyField(activeNotCooldown));
        }
    }
}
