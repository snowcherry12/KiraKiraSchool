using UnityEditor;

namespace GameCreator.Editor.Behavior
{
    internal abstract class TNodeToolStateMachine : TNodeTool
    {
        protected TNodeToolStateMachine(TGraphTool graphTool, SerializedProperty property)
            : base(graphTool, property)
        { }
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public virtual bool ShowInspectorTransitions => true;
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public override string GetPortText(string portId) => string.Empty;

        protected override TPortTool CreatePort(TNodeTool nodeTool, SerializedProperty property)
        {
            return new PortToolStateMachine(nodeTool, property);
        }
    }
}