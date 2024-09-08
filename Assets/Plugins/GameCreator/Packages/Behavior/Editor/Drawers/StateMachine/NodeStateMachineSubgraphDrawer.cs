using GameCreator.Editor.Common;
using GameCreator.Runtime.Behavior;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    [CustomPropertyDrawer(typeof(NodeStateMachineSubgraph))]
    public class NodeStateMachineSubgraphDrawer : PropertyDrawer
    {
        private const string PROP_CHECK = "m_Check";
        private const string PROP_CONDITIONS = "m_Conditions";
        private const string PROP_GRAPH = "m_Graph";
        
        // PAINT METHODS: -------------------------------------------------------------------------
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();
            
            SerializedProperty check = property.FindPropertyRelative(PROP_CHECK);
            SerializedProperty conditions = property.FindPropertyRelative(PROP_CONDITIONS);
            SerializedProperty graph = property.FindPropertyRelative(PROP_GRAPH);
            
            PropertyField fieldCheck = new PropertyField(check);
            PropertyField fieldConditions = new PropertyField(conditions);
            PropertyField fieldGraph = new PropertyField(graph);
            
            root.Add(fieldCheck);
            root.Add(new SpaceSmaller());
            root.Add(fieldConditions);
            
            root.Add(new SpaceSmall());
            root.Add(fieldGraph);

            return root;
        }
    }
}