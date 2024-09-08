using GameCreator.Editor.Common;
using GameCreator.Runtime.Behavior;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    [CustomPropertyDrawer(typeof(NodeActionPlanTaskInstructions))]
    public class NodeActionPlanTaskInstructionsDrawer : PropertyDrawer
    {
        private const string PROP_NAME = "m_Name";
        private const string PROP_COST = "m_Cost";
        private const string PROP_CONDITIONS = "m_Conditions";
        private const string PROP_INSTRUCTIONS = "m_Instructions";

        // PAINT METHODS: -------------------------------------------------------------------------
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            SerializedProperty name = property.FindPropertyRelative(PROP_NAME);
            SerializedProperty cost = property.FindPropertyRelative(PROP_COST);
            SerializedProperty conditions = property.FindPropertyRelative(PROP_CONDITIONS);
            SerializedProperty onUpdate = property.FindPropertyRelative(PROP_INSTRUCTIONS);
            
            PropertyField fieldName = new PropertyField(name);
            PropertyField fieldCost = new PropertyField(cost);
            PropertyField fieldConditions = new PropertyField(conditions);
            PropertyField fieldOnUpdate = new PropertyField(onUpdate);
            
            root.Add(fieldName);
            root.Add(new SpaceSmallest());
            root.Add(fieldCost);
            root.Add(new SpaceSmall());
            root.Add(fieldConditions);
            
            root.Add(new SpaceSmall());
            root.Add(new LabelTitle("On Execute:"));
            root.Add(new SpaceSmaller());
            root.Add(fieldOnUpdate);

            return root;
        }
    }
}