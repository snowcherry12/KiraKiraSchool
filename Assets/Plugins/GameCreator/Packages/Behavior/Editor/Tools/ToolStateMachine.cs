using System;
using GameCreator.Runtime.Behavior;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    internal class ToolStateMachine : TGraphTool
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private readonly StateMachine m_StateMachine;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public ToolStateMachine(StateMachine stateMachine, TGraphWindow window)
            : base(stateMachine, window)
        { }
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override bool AllowCycles => true;

        // OVERRIDE METHODS: ----------------------------------------------------------------------

        public override bool AcceptNode(TNode node)
        {
            return node is TNodeStateMachine;
        }
        
        protected override TGraphWires CreateGraphWires(TGraphTool graphTool)
        {
            return new GraphWireStateMachine(graphTool);
        }

        protected override void OnGenerateCreateMenu(
            ContextualMenuPopulateEvent eventMenu, 
            string prefix, ArgData argData)
        {
            eventMenu.menu.AppendAction(
                string.Format(prefix, "State"),
                this.CreateNode<NodeStateMachineState>,
                MenuCanCreateNode,
                argData
            );
            
            eventMenu.menu.AppendAction(
                string.Format(prefix, "Conditions"),
                this.CreateNode<NodeStateMachineConditions>,
                MenuCanCreateNode,
                argData
            );
            
            eventMenu.menu.AppendAction(
                string.Format(prefix, "Subgraph"),
                this.CreateNode<NodeStateMachineSubgraph>,
                MenuCanCreateNode,
                argData
            );
            
            eventMenu.menu.AppendAction(
                string.Format(prefix, "Elbow"),
                this.CreateNode<NodeStateMachineElbow>,
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
                nameof(NodeStateMachineEnter) => new NodeToolStateMachineEnter(this, nodeProperty),
                nameof(NodeStateMachineExit) => new NodeToolStateMachineExit(this, nodeProperty),
                nameof(NodeStateMachineState) => new NodeToolStateMachineState(this, nodeProperty),
                nameof(NodeStateMachineSubgraph) => new NodeToolStateMachineSubgraph(this, nodeProperty),
                nameof(NodeStateMachineConditions) => new NodeToolStateMachineConditions(this, nodeProperty),
                nameof(NodeStateMachineElbow) => new NodeToolStateMachineElbow(this, nodeProperty),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}