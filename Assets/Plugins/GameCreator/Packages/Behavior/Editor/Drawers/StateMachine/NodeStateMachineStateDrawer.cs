using GameCreator.Editor.Common;
using GameCreator.Runtime.Behavior;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    [CustomPropertyDrawer(typeof(NodeStateMachineState))]
    public class NodeStateMachineStateDrawer : PropertyDrawer
    {
        private const string PROP_NAME = "m_Name";
        private const string PROP_CHECK = "m_Check";
        private const string PROP_CONDITIONS = "m_Conditions";
        private const string PROP_ON_ENTER = "m_OnEnter";
        private const string PROP_ON_EXIT = "m_OnExit";
        private const string PROP_ON_UPDATE = "m_Instructions";
        
        // PAINT METHODS: -------------------------------------------------------------------------
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            SerializedProperty name = property.FindPropertyRelative(PROP_NAME);
            SerializedProperty check = property.FindPropertyRelative(PROP_CHECK);
            SerializedProperty conditions = property.FindPropertyRelative(PROP_CONDITIONS);
            SerializedProperty onEnter = property.FindPropertyRelative(PROP_ON_ENTER);
            SerializedProperty onExit = property.FindPropertyRelative(PROP_ON_EXIT);
            SerializedProperty onUpdate = property.FindPropertyRelative(PROP_ON_UPDATE);
            
            PropertyField fieldName = new PropertyField(name);
            PropertyField fieldCheck = new PropertyField(check);
            PropertyField fieldConditions = new PropertyField(conditions);
            PropertyField fieldOnEnter = new PropertyField(onEnter);
            PropertyField fieldOnExit = new PropertyField(onExit);
            PropertyField fieldOnUpdate = new PropertyField(onUpdate);
            
            root.Add(fieldName);
            root.Add(new SpaceSmall());
            root.Add(fieldCheck);
            root.Add(new SpaceSmaller());
            root.Add(fieldConditions);
            
            root.Add(new SpaceSmall());
            root.Add(new LabelTitle("On Enter:"));
            root.Add(new SpaceSmaller());
            root.Add(fieldOnEnter);
            
            root.Add(new SpaceSmall());
            root.Add(new LabelTitle("On Exit:"));
            root.Add(new SpaceSmaller());
            root.Add(fieldOnExit);
            
            root.Add(new SpaceSmall());
            root.Add(new LabelTitle("On Update:"));
            root.Add(new SpaceSmaller());
            root.Add(fieldOnUpdate);

            return root;
        }
    }
}