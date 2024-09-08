using GameCreator.Runtime.Behavior;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    [CustomPropertyDrawer(typeof(NodeActionPlanPreConditions))]
    public class NodeActionPlanPreConditionsDrawer : PropertyDrawer
    {
        private const string PROP_BELIEFS = "m_Beliefs";

        // PAINT METHODS: -------------------------------------------------------------------------
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            SerializedProperty beliefs = property.FindPropertyRelative(PROP_BELIEFS);
            
            PropertyField fieldBeliefs = new PropertyField(beliefs);
            root.Add(fieldBeliefs);

            return root;
        }
    }
}