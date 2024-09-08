using GameCreator.Editor.Common;
using GameCreator.Runtime.Inventory;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Inventory
{
    [CustomPropertyDrawer(typeof(Usage))]
    public class UsageDrawer : TBoxDrawer
    {
        protected override string Name(SerializedProperty property) => "Usage";

        protected override void CreatePropertyContent(VisualElement container, SerializedProperty property)
        {
            VisualElement head = new VisualElement();
            VisualElement body = new VisualElement();
            VisualElement foot = new VisualElement();

            container.Add(head);
            container.Add(body);
            container.Add(foot);
            
            SerializedProperty canUse = property.FindPropertyRelative("m_CanUse");
            SerializedProperty consumeWhenUse = property.FindPropertyRelative("m_ConsumeWhenUse");
            SerializedProperty cooldown = property.FindPropertyRelative("m_Cooldown");
            
            SerializedProperty isUse = property.FindPropertyRelative("m_ConditionsCanUse");
            SerializedProperty onUse = property.FindPropertyRelative("m_InstructionsOnUse");
            SerializedProperty inherit = property.FindPropertyRelative("m_ExecuteFromParent");

            PropertyField fieldCanUse = new PropertyField(canUse);
            PropertyField fieldConsumeWhenUse = new PropertyField(consumeWhenUse);
            PropertyField fieldCooldown = new PropertyField(cooldown);
            PropertyField fieldIsUse = new PropertyField(isUse);
            PropertyField fieldOnUse = new PropertyField(onUse);
            PropertyField fieldInherit = new PropertyField(inherit);

            head.Add(fieldCanUse);
            body.Add(fieldConsumeWhenUse);
            body.Add(fieldCooldown);

            foot.Add(new SpaceSmall());
            foot.Add(new LabelTitle("Can Use"));
            foot.Add(fieldIsUse);
            
            foot.Add(new SpaceSmall());
            foot.Add(new LabelTitle("On Use"));
            foot.Add(fieldOnUse);
            
            foot.Add(new SpaceSmall());
            foot.Add(fieldInherit);
            
            body.SetEnabled(canUse.boolValue);
            fieldCanUse.RegisterValueChangeCallback(changeEvent =>
            {
                body.SetEnabled(changeEvent.changedProperty.boolValue);
            });
        }
    }
}