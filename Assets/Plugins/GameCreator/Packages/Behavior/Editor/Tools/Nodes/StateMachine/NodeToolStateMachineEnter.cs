using GameCreator.Runtime.Behavior;
using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEngine;

namespace GameCreator.Editor.Behavior
{
    internal class NodeToolStateMachineEnter : TNodeToolStateMachine
    {
        private static readonly IIcon ICON = new IconNodeArrowRight(ColorTheme.Type.Green);
        
        // PROPERTIES: ----------------------------------------------------------------------------

        protected override float Width => 100f;

        public override bool CanMove => true;
        public override bool CanDelete => false;

        protected override bool ShowHead => true;
        protected override bool ShowBody => false;
        protected override bool ShowFoot => false;
        
        protected override bool DrawConditions => false;
        protected override bool DrawInstructions => false;

        public override string Title => "Enter";
        public override Texture Icon => ICON.Texture;

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public NodeToolStateMachineEnter(TGraphTool graphTool, SerializedProperty property)
            : base(graphTool, property)
        { }
    }
}