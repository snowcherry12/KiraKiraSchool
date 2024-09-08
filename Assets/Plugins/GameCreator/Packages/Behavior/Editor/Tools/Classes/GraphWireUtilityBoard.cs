using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    internal class GraphWireUtilityBoard : TGraphWires
    {
        // PROPERTIES: ----------------------------------------------------------------------------

        protected override bool DrawDirection => false;

        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public GraphWireUtilityBoard(TGraphTool graphTool) : base(graphTool)
        { }
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        private static Color BackgroundColor => EditorApplication.isPlayingOrWillChangePlaymode 
            ? GraphUtils.Gray
            : GraphUtils.Blue;

        // OVERRIDE METHODS: ----------------------------------------------------------------------

        protected override Color GetWireColor(TNodeTool nodeTool) => BackgroundColor;

        protected override void AfterDrawConnection(MeshGenerationContext context, Vector2 a, Vector2 b, int index)
        { }
    }
}