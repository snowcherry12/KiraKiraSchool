using GameCreator.Runtime.Behavior;
using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEngine;

namespace GameCreator.Editor.Behavior
{
    internal class NodeToolStateMachineExit : TNodeToolStateMachine
    {
        private static readonly IIcon ICON = new IconNodeArrowRight(ColorTheme.Type.Red);
        
        // PROPERTIES: ----------------------------------------------------------------------------

        protected override float Width => 100f;

        public override bool CanMove => true;
        public override bool CanDelete => false;

        protected override bool ShowHead => true;
        protected override bool ShowBody => false;
        protected override bool ShowFoot => false;

        protected override bool DrawConditions => false;
        protected override bool DrawInstructions => false;

        public override string Title => "Exit";
        public override Texture Icon => ICON.Texture;

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public NodeToolStateMachineExit(TGraphTool graphTool, SerializedProperty property)
            : base(graphTool, property)
        { }
    }
}