using GameCreator.Runtime.Behavior;
using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEngine;

namespace GameCreator.Editor.Behavior
{
    internal class NodeToolStateMachineState : TNodeToolStateMachine
    {
        private static readonly IIcon ICON = new IconNodeInstructions(ColorTheme.Type.TextLight);

        private const string PROP_NAME = "m_Name"; 
        
        // PROPERTIES: ----------------------------------------------------------------------------

        protected override float Width => 300f;

        public override bool CanMove => true;
        public override bool CanDelete => true;
        
        protected override bool ShowHead => true;
        protected override bool ShowBody => true;
        protected override bool ShowFoot => true;

        protected override bool DrawConditions => true;
        protected override bool DrawInstructions => true;

        public override string Title
        {
            get
            {
                string value = this.Property.FindPropertyRelative(PROP_NAME).stringValue;
                return string.IsNullOrEmpty(value) ? "State" : value;
            }
        }

        public override Texture Icon => ICON.Texture;

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public NodeToolStateMachineState(TGraphTool graphTool, SerializedProperty property)
            : base(graphTool, property)
        { }
    }
}