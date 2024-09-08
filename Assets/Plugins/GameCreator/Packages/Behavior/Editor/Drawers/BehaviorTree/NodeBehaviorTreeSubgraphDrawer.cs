using GameCreator.Editor.Common;
using GameCreator.Runtime.Behavior;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    [CustomPropertyDrawer(typeof(NodeBehaviorTreeSubgraph))]
    public class NodeBehaviorTreeSubgraphDrawer : PropertyDrawer
    {
        private const string PROP_STOP = "m_Stop";
        private const string PROP_CONDITIONS = "m_Conditions";
        private const string PROP_GRAPH = "m_Graph";
        
        // PAINT METHODS: -------------------------------------------------------------------------
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            SerializedProperty stop = property.FindPropertyRelative(PROP_STOP);
            SerializedProperty conditions = property.FindPropertyRelative(PROP_CONDITIONS);
            SerializedProperty graph = property.FindPropertyRelative(PROP_GRAPH);
            
            PropertyField fieldStop = new PropertyField(stop);
            PropertyField fieldConditions = new PropertyField(conditions);
            PropertyField fieldGraph = new PropertyField(graph);
            
            root.Add(fieldStop);
            root.Add(new SpaceSmaller());
            root.Add(fieldConditions);
            
            root.Add(new SpaceSmall());
            root.Add(fieldGraph);

            return root;
        }
    }
}