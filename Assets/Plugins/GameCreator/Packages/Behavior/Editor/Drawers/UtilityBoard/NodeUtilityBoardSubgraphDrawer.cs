using GameCreator.Editor.Common;
using GameCreator.Runtime.Behavior;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    [CustomPropertyDrawer(typeof(NodeUtilityBoardSubgraph))]
    public class NodeUtilityBoardSubgraphDrawer : PropertyDrawer
    {
        private const string PROP_SCORE = "m_Score";
        private const string PROP_CONDITIONS = "m_Conditions";
        
        private const string PROP_GRAPH = "m_Graph";

        // PAINT METHODS: -------------------------------------------------------------------------
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            SerializedProperty scoreCurve = property.FindPropertyRelative(PROP_SCORE);
            SerializedProperty conditions = property.FindPropertyRelative(PROP_CONDITIONS);
            SerializedProperty graph = property.FindPropertyRelative(PROP_GRAPH);
            
            PropertyField FieldScoreCurve = new PropertyField(scoreCurve);
            PropertyField fieldConditions = new PropertyField(conditions);
            PropertyField fieldGraph = new PropertyField(graph);
            
            root.Add(FieldScoreCurve);
            root.Add(new SpaceSmaller());
            root.Add(fieldConditions);
            
            root.Add(new SpaceSmall());
            root.Add(fieldGraph);

            return root;
        }
    }
}