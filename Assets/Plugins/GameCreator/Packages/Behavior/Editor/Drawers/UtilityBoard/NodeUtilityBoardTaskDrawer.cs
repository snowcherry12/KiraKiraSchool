using GameCreator.Editor.Common;
using GameCreator.Runtime.Behavior;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    [CustomPropertyDrawer(typeof(NodeUtilityBoardTask))]
    public class NodeUtilityBoardTaskDrawer : PropertyDrawer
    {
        private const string PROP_NAME = "m_Name";
        private const string PROP_SCORE = "m_Score";
        private const string PROP_CONDITIONS = "m_Conditions";
        private const string PROP_ON_EXIT = "m_OnExit";
        private const string PROP_INSTRUCTIONS = "m_Instructions";
        
        // PAINT METHODS: -------------------------------------------------------------------------
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            SerializedProperty name = property.FindPropertyRelative(PROP_NAME);
            SerializedProperty scoreCurve = property.FindPropertyRelative(PROP_SCORE);
            SerializedProperty conditions = property.FindPropertyRelative(PROP_CONDITIONS);
            SerializedProperty onExit = property.FindPropertyRelative(PROP_ON_EXIT);
            SerializedProperty onUpdate = property.FindPropertyRelative(PROP_INSTRUCTIONS);
            
            PropertyField fieldName = new PropertyField(name);
            PropertyField fieldScoreCurve = new PropertyField(scoreCurve);
            PropertyField fieldConditions = new PropertyField(conditions);
            PropertyField fieldOnExit = new PropertyField(onExit);
            PropertyField fieldOnUpdate = new PropertyField(onUpdate);
            
            root.Add(fieldName);
            root.Add(new SpaceSmallest());
            root.Add(fieldScoreCurve);
            root.Add(new SpaceSmall());
            root.Add(fieldConditions);
            
            root.Add(new SpaceSmall());
            root.Add(new LabelTitle("On Execute:"));
            root.Add(new SpaceSmaller());
            root.Add(fieldOnUpdate);
            
            root.Add(new SpaceSmall());
            root.Add(new LabelTitle("On Exit:"));
            root.Add(new SpaceSmaller());
            root.Add(fieldOnExit);

            return root;
        }
    }
}