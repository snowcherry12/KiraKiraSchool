using System;
using GameCreator.Runtime.Behavior;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    internal class ToolBehaviorTree : TGraphTool
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private readonly BehaviorTree m_BehaviorTree;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public ToolBehaviorTree(BehaviorTree behaviorTree, TGraphWindow window)
            : base(behaviorTree, window)
        { }
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override bool AllowCycles => false;

        // OVERRIDE METHODS: ----------------------------------------------------------------------

        public override bool AcceptNode(TNode node)
        {
            return node is TNodeBehaviorTree;
        }
        
        protected override TGraphWires CreateGraphWires(TGraphTool graphTool)
        {
            return new GraphWireBehaviorTree(graphTool);
        }

        protected override void OnGenerateCreateMenu(
            ContextualMenuPopulateEvent eventMenu, 
            string prefix, ArgData argData)
        {
            eventMenu.menu.AppendAction(
                string.Format(prefix, "Decorator"),
                this.CreateNode<NodeBehaviorTreeDecorator>,
                MenuCanCreateNode,
                argData
            );
            
            eventMenu.menu.AppendAction(
                string.Format(prefix, "Composite"),
                this.CreateNode<NodeBehaviorTreeComposite>,
                MenuCanCreateNode,
                argData
            );
            
            eventMenu.menu.AppendAction(
                string.Format(prefix, "Graph"),
                this.CreateNode<NodeBehaviorTreeSubgraph>,
                MenuCanCreateNode,
                argData
            );
            
            eventMenu.menu.AppendAction(
                string.Format(prefix, "Task"),
                this.CreateNode<NodeBehaviorTreeTask>,
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
                nameof(NodeBehaviorTreeEntry) => new NodeToolBehaviorTreeEntry(this, nodeProperty),
                nameof(NodeBehaviorTreeTask) => new NodeToolBehaviorTreeTask(this, nodeProperty),
                nameof(NodeBehaviorTreeComposite) => new NodeToolBehaviorTreeComposite(this, nodeProperty),
                nameof(NodeBehaviorTreeDecorator) => new NodeToolBehaviorTreeDecorator(this, nodeProperty),
                nameof(NodeBehaviorTreeSubgraph) => new NodeToolBehaviorTreeSubgraph(this, nodeProperty),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}