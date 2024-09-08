using GameCreator.Runtime.Behavior;
using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    internal class GraphWireBehaviorTree : TGraphWires
    {
        // PROPERTIES: ----------------------------------------------------------------------------

        protected override bool DrawDirection => false;

        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public GraphWireBehaviorTree(TGraphTool graphTool) : base(graphTool)
        { }
        
        // OVERRIDE METHODS: ----------------------------------------------------------------------

        protected override Color GetWireColor(TNodeTool nodeTool)
        {
            Color color = GraphUtils.Blue;
            
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                IdString nodeId = new IdString(nodeTool.NodeId);
                Status status = TargetUtils.Get?.GetStatus(nodeId) ?? Status.Ready;
                
                color = GraphUtils.GetColor(status, GraphUtils.Gray);
            }
            
            return color;
        }

        protected override void AfterDrawConnection(MeshGenerationContext context, Vector2 a, Vector2 b, int index)
        { }
    }
}