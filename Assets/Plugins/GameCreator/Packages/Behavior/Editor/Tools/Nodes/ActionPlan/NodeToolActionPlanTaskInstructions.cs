using GameCreator.Editor.Common;
using GameCreator.Runtime.Behavior;
using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEngine;

namespace GameCreator.Editor.Behavior
{
    internal class NodeToolActionPlanTaskInstructions : TNodeToolActionPlan
    {
        private static readonly IIcon ICON = new IconNodeInstructions(ColorTheme.Type.TextLight);

        // PROPERTIES: ----------------------------------------------------------------------------

        protected override float Width => 300f;

        public override bool CanMove => true;
        public override bool CanDelete => true;
        
        protected override bool ShowHead => true;
        protected override bool ShowBody => true;
        protected override bool ShowFoot => true;

        protected override bool DrawConditions => true;
        protected override bool DrawInstructions => true;

        public override string Title => this.Node is NodeActionPlanTaskInstructions node 
            ? node.Title
            : "Unknown";

        public override Texture Icon => ICON.Texture;

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public NodeToolActionPlanTaskInstructions(TGraphTool graphTool, SerializedProperty property)
            : base(graphTool, property)
        { }
    }
}