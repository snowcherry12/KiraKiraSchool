using System;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Behavior;
using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    internal class NodeToolActionPlanTaskSubgraph : TNodeToolActionPlan
    {
        private static readonly IIcon ICON_GRAPH = new IconGraphOutline(ColorTheme.Type.TextLight);
        private static readonly IIcon ICON_STATE_MACHINE = new IconStateMachineOutline(ColorTheme.Type.TextLight);
        private static readonly IIcon ICON_BEHAVIOR_TREE = new IconBehaviorTreeOutline(ColorTheme.Type.TextLight);
        private static readonly IIcon ICON_ACTION_PLAN   = new IconActionPlanOutline(ColorTheme.Type.TextLight);
        private static readonly IIcon ICON_UTILITY_BOARD = new IconUtilityBoardOutline(ColorTheme.Type.TextLight);

        private const string TITLE_NONE = "(none)";

        private const string ERR_REPEAT_GRAPH = "Multiple instances of {0}";
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        private ErrorMessage m_Error;
        private Button m_ButtonGraph;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        protected override float Width => 300f;

        public override bool CanMove => true;
        public override bool CanDelete => true;
        
        protected override bool ShowHead => true;
        protected override bool ShowBody => true;
        protected override bool ShowFoot => true;

        protected override bool DrawConditions => true;
        protected override bool DrawInstructions => false;

        public override string Title => this.Node is NodeActionPlanTaskSubgraph node 
            ? node.Title
            : "Unknown";

        public override Texture Icon
        {
            get
            {
                Type graphType = this.Graph != null ? this.Graph.GetType() : null;
                if (graphType == null) return ICON_GRAPH.Texture;
                
                if (graphType == typeof(StateMachine))
                {
                    return ICON_STATE_MACHINE.Texture;
                }
                
                if (graphType == typeof(BehaviorTree))
                {
                    return ICON_BEHAVIOR_TREE.Texture;
                }
                
                if (graphType == typeof(ActionPlan))
                {
                    return ICON_ACTION_PLAN.Texture;
                }
                
                if (graphType == typeof(UtilityBoard))
                {
                    return ICON_UTILITY_BOARD.Texture;
                }

                return ICON_GRAPH.Texture;
            }
        }

        private Graph Graph => this.Property?
            .FindPropertyRelative("m_Graph")
            .objectReferenceValue as Graph;

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public NodeToolActionPlanTaskSubgraph(TGraphTool graphTool, SerializedProperty property)
            : base(graphTool, property)
        { }
        
        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected override void CreateContent()
        {
            base.CreateContent();
        
            this.m_Error = new ErrorMessage(string.Empty);
            this.m_ButtonGraph = new Button(this.EnterGraph) { text = "Open" };
            
            this.m_Foot.Add(this.m_Error);
            this.m_Foot.Add(this.m_ButtonGraph);
        }

        protected override void RefreshFoot()
        {
            string error = this.GetError();
            this.m_Error.Text = error;
            this.m_Error.style.display = string.IsNullOrEmpty(error)
                ? DisplayStyle.None
                : DisplayStyle.Flex;
            
            this.m_ButtonGraph.SetEnabled(this.Graph != null);
        }

        // CALLBACK METHODS: ----------------------------------------------------------------------

        private void EnterGraph()
        {
            Graph asset = this.Graph;
            if (asset == null) return;
            
            switch (asset)
            {
                case StateMachine:
                    WindowStateMachine.Open(this.Graph as StateMachine);
                    break;
                
                case BehaviorTree:
                    WindowBehaviorTree.Open(this.Graph as BehaviorTree);
                    break;
                
                case ActionPlan:
                    this.GraphTool.Window.NewPage(asset, true);
                    this.GraphTool.Window.Overlays.Breadcrumb.Show();
                    break;
                
                case UtilityBoard:
                    WindowUtilityBoard.Open(this.Graph as UtilityBoard);
                    break;
            }
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private string GetError()
        {
            Graph graph = this.Graph;
            if (graph == null) return string.Empty;

            return GraphUtils.HasMultipleInstances(this.GraphTool.Window.FirstPage.Graph, graph) 
                ? string.Format(ERR_REPEAT_GRAPH, graph.name)
                : string.Empty;
        }
    }
}