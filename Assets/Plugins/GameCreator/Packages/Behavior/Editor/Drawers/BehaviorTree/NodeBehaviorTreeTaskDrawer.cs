using GameCreator.Editor.Common;
using GameCreator.Runtime.Behavior;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    [CustomPropertyDrawer(typeof(NodeBehaviorTreeTask))]
    public class NodeBehaviorTreeTaskDrawer : PropertyDrawer
    {
        private const string PROP_STOP = "m_Stop";
        private const string PROP_CONDITIONS = "m_Conditions";
        private const string PROP_INSTRUCTIONS = "m_Instructions";
        
        // PAINT METHODS: -------------------------------------------------------------------------
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            SerializedProperty stop = property.FindPropertyRelative(PROP_STOP);
            SerializedProperty conditions = property.FindPropertyRelative(PROP_CONDITIONS);
            SerializedProperty instructions = property.FindPropertyRelative(PROP_INSTRUCTIONS);
            
            PropertyField fieldStop = new PropertyField(stop);
            PropertyField fieldConditions = new PropertyField(conditions);
            PropertyField fieldInstructions = new PropertyField(instructions);
            
            root.Add(fieldStop);
            root.Add(new SpaceSmaller());
            root.Add(fieldConditions);
            
            root.Add(new SpaceSmall());
            root.Add(new LabelTitle("Instructions:"));
            root.Add(new SpaceSmaller());
            root.Add(fieldInstructions);

            return root;
        }
    }
}