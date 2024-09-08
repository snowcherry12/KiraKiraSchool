using System;
using GameCreator.Runtime.Behavior;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    internal class ToolActionPlan : TGraphTool
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private readonly ActionPlan m_ActionPlan;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public ToolActionPlan(ActionPlan actionPlan, TGraphWindow window)
            : base(actionPlan, window)
        { }
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override bool AllowCycles => true;

        // OVERRIDE METHODS: ----------------------------------------------------------------------

        public override bool AcceptNode(TNode node)
        {
            return node is TNodeActionPlan;
        }

        protected override TGraphWires CreateGraphWires(TGraphTool graphTool)
        {
            return new GraphWireActionPlan(graphTool);
        }

        protected override void OnGenerateCreateMenu(
            ContextualMenuPopulateEvent eventMenu, 
            string prefix, ArgData argData)
        {
            eventMenu.menu.AppendAction(
                string.Format(prefix, "Task"),
                this.CreateNode<NodeActionPlanTaskInstructions>,
                MenuCanCreateNode,
                argData
            );
            
            eventMenu.menu.AppendAction(
                string.Format(prefix, "Graph"),
                this.CreateNode<NodeActionPlanTaskSubgraph>,
                MenuCanCreateNode,
                argData
            );
            
            eventMenu.menu.AppendAction(
                string.Format(prefix, "Pre Conditions"),
                this.CreateNode<NodeActionPlanPreConditions>,
                MenuCanCreateNode,
                argData
            );
            
            eventMenu.menu.AppendAction(
                string.Format(prefix, "Post Conditions"),
                this.CreateNode<NodeActionPlanPostConditions>,
                MenuCanCreateNode,
                argData
            );
        }

        protected override TNodeTool CreateFromNode(SerializedProperty nodeProperty)
        {
            string[] typeList = nodeProperty.managedReferenceFullTypename.Split('.');
            string typeName = typeList[^1];

            return typeName switch
            {
                nameof(NodeActionPlanRoot) => null,
                nameof(NodeActionPlanTaskInstructions) => new NodeToolActionPlanTaskInstructions(this, nodeProperty),
                nameof(NodeActionPlanTaskSubgraph) => new NodeToolActionPlanTaskSubgraph(this, nodeProperty),
                nameof(NodeActionPlanPreConditions) => new NodeToolActionPlanPreConditions(this, nodeProperty),
                nameof(NodeActionPlanPostConditions) => new NodeToolActionPlanPostConditions(this, nodeProperty),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}