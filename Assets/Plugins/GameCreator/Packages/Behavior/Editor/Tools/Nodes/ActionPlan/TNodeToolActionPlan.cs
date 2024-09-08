using UnityEditor;

namespace GameCreator.Editor.Behavior
{
    internal abstract class TNodeToolActionPlan : TNodeTool
    {
        protected TNodeToolActionPlan(TGraphTool graphTool, SerializedProperty property)
            : base(graphTool, property)
        { }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public override string GetPortText(string portId) => string.Empty;

        protected override TPortTool CreatePort(TNodeTool nodeTool, SerializedProperty property)
        {
            return new PortToolActionPlan(nodeTool, property);
        }
    }
}