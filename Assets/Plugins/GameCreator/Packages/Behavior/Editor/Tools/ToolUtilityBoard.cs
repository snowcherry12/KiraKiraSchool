using System;
using GameCreator.Runtime.Behavior;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    internal class ToolUtilityBoard : TGraphTool
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private readonly UtilityBoard m_UtilityBoard;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public ToolUtilityBoard(UtilityBoard utilityBoard, TGraphWindow window)
            : base(utilityBoard, window)
        { }
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override bool AllowCycles => true;

        // OVERRIDE METHODS: ----------------------------------------------------------------------

        public override bool AcceptNode(TNode node)
        {
            return node is TNodeUtilityBoard;
        }
        
        protected override TGraphWires CreateGraphWires(TGraphTool graphTool)
        {
            return new GraphWireUtilityBoard(graphTool);
        }

        protected override void OnGenerateCreateMenu(
            ContextualMenuPopulateEvent eventMenu, 
            string prefix, ArgData argData)
        {
            eventMenu.menu.AppendAction(
                string.Format(prefix, "Task"),
                this.CreateNode<NodeUtilityBoardTask>,
                MenuCanCreateNode,
                argData
            );

            eventMenu.menu.AppendAction(
                string.Format(prefix, "Subgraph"),
                this.CreateNode<NodeUtilityBoardSubgraph>,
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
                nameof(NodeUtilityBoardRoot) => null,
                nameof(NodeUtilityBoardTask) => new NodeToolUtilityBoardTask(this, nodeProperty),
                nameof(NodeUtilityBoardSubgraph) => new NodeToolUtilityBoardSubgraph(this, nodeProperty),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}