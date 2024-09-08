using GameCreator.Editor.Common;
using GameCreator.Runtime.Behavior;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    [CustomPropertyDrawer(typeof(NodeActionPlanTaskSubgraph))]
    public class NodeActionPlanTaskSubgraphDrawer : PropertyDrawer
    {
        private const string PROP_COST = "m_Cost";
        private const string PROP_CONDITIONS = "m_Conditions";
        private const string PROP_GRAPH = "m_Graph";

        // PAINT METHODS: -------------------------------------------------------------------------
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();
            
            SerializedProperty cost = property.FindPropertyRelative(PROP_COST);
            SerializedProperty conditions = property.FindPropertyRelative(PROP_CONDITIONS);
            SerializedProperty graph = property.FindPropertyRelative(PROP_GRAPH);
            
            PropertyField fieldCost = new PropertyField(cost);
            PropertyField fieldConditions = new PropertyField(conditions);
            PropertyField fieldGraph = new PropertyField(graph);
            
            root.Add(fieldCost);
            root.Add(new SpaceSmall());
            root.Add(fieldConditions);
            
            root.Add(new SpaceSmall());
            root.Add(fieldGraph);

            return root;
        }
    }
}