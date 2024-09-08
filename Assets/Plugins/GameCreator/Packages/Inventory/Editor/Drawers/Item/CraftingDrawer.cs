using GameCreator.Editor.Common;
using GameCreator.Runtime.Inventory;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Inventory
{
    [CustomPropertyDrawer(typeof(Crafting))]
    public class CraftingDrawer : TBoxDrawer
    {
        protected override string Name(SerializedProperty property) => "Crafting";

        protected override void CreatePropertyContent(VisualElement container, SerializedProperty property)
        {
            IngredientsTool fieldIngredients = new IngredientsTool(property, "m_Ingredients");
            
            container.Add(new LabelTitle("Ingredients"));
            container.Add(fieldIngredients);
            
            VisualElement contentCraft = new VisualElement();
            VisualElement contentDismantle = new VisualElement();

            SerializedProperty canCraft = property.FindPropertyRelative("m_CanCraft");
            SerializedProperty ifCraft = property.FindPropertyRelative("m_ConditionsCraft");
            SerializedProperty onCraft = property.FindPropertyRelative("m_InstructionsOnCraft");
            SerializedProperty canDismantle = property.FindPropertyRelative("m_CanDismantle");
            SerializedProperty ifDismantle = property.FindPropertyRelative("m_ConditionsDismantle");
            SerializedProperty onDismantle = property.FindPropertyRelative("m_InstructionsOnDismantle");

            PropertyField fieldCanCraft = new PropertyField(canCraft, "Craft");
            PropertyField fieldIfCraft = new PropertyField(ifCraft);
            PropertyField fieldOnCraft = new PropertyField(onCraft);
            PropertyField fieldCanDismantle = new PropertyField(canDismantle, "Dismantle");
            PropertyField fieldIfDismantle = new PropertyField(ifDismantle);
            PropertyField fieldOnDismantle = new PropertyField(onDismantle);

            fieldCanCraft.AddToClassList("gc-bold");
            fieldCanDismantle.AddToClassList("gc-bold");

            container.Add(new SpaceSmall());
            container.Add(fieldCanCraft);
            container.Add(contentCraft);
            contentCraft.Add(fieldIfCraft);
            contentCraft.Add(fieldOnCraft);
            
            container.Add(new SpaceSmall());
            container.Add(fieldCanDismantle);
            container.Add(contentDismantle);
            
            contentDismantle.Add(fieldIfDismantle);
            contentDismantle.Add(fieldOnDismantle);
            
            contentCraft.SetEnabled(canCraft.boolValue);
            fieldCanCraft.RegisterValueChangeCallback(changeEvent =>
            {
                contentCraft.SetEnabled(changeEvent.changedProperty.boolValue);
            });
            
            contentDismantle.SetEnabled(canDismantle.boolValue);
            fieldCanDismantle.RegisterValueChangeCallback(changeEvent =>
            {
                contentDismantle.SetEnabled(changeEvent.changedProperty.boolValue);
            });
        }
    }
}