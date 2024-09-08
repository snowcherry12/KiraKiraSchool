using GameCreator.Editor.Common;
using GameCreator.Runtime.Behavior;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    [CustomPropertyDrawer(typeof(NodeStateMachineConditions))]
    public class NodeStateMachineConditionsDrawer : PropertyDrawer
    {
        private const string PROP_NAME = "m_Name";
        private const string PROP_CONDITIONS = "m_Conditions";

        // PAINT METHODS: -------------------------------------------------------------------------
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            SerializedProperty name = property.FindPropertyRelative(PROP_NAME);
            SerializedProperty conditions = property.FindPropertyRelative(PROP_CONDITIONS);

            PropertyField fieldName = new PropertyField(name);
            PropertyField fieldConditions = new PropertyField(conditions);

            root.Add(fieldName);
            root.Add(new SpaceSmall());
            root.Add(fieldConditions);

            return root;
        }
    }
}