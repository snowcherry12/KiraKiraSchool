using GameCreator.Runtime.Behavior;
using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEngine;

namespace GameCreator.Editor.Behavior
{
    internal class NodeToolBehaviorTreeEntry : TNodeToolBehaviorTree
    {
        private static readonly IIcon ICON = new IconNodeArrowDown(ColorTheme.Type.TextLight);
        
        // PROPERTIES: ----------------------------------------------------------------------------

        protected override float Width => 150f;

        public override bool CanMove => true;
        public override bool CanDelete => false;

        protected override bool ShowHead => true;
        protected override bool ShowBody => false;
        protected override bool ShowFoot => false;
        
        protected override bool DrawConditions => false;
        protected override bool DrawInstructions => false;

        public override string Title => "Entry";
        public override Texture Icon => ICON.Texture;

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public NodeToolBehaviorTreeEntry(TGraphTool graphTool, SerializedProperty property)
            : base(graphTool, property)
        { }
    }
}