using System;
using GameCreator.Runtime.Behavior;
using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEngine;

namespace GameCreator.Editor.Behavior
{
    internal class NodeToolStateMachineElbow : TNodeToolStateMachine
    {
        private static readonly IIcon ICON_T = new IconNodeArrowUp(ColorTheme.Type.TextLight);
        private static readonly IIcon ICON_R = new IconNodeArrowRight(ColorTheme.Type.TextLight);
        private static readonly IIcon ICON_B = new IconNodeArrowDown(ColorTheme.Type.TextLight);
        private static readonly IIcon ICON_L = new IconNodeArrowLeft(ColorTheme.Type.TextLight);
        
        // PROPERTIES: ----------------------------------------------------------------------------

        protected override float Width => 36f;

        public override bool CanMove => true;
        public override bool CanDelete => true;

        protected override bool ShowHead => true;
        protected override bool ShowBody => false;
        protected override bool ShowFoot => false;

        protected override bool DrawConditions => false;
        protected override bool DrawInstructions => false;

        public override string Title
        {
            get
            {
                if (this.OutputPortTools.Length == 0) return string.Empty;
                SerializedProperty connections = this.OutputPortTools[0].Connections;

                if (connections.arraySize == 0) return string.Empty;

                string connectionId = connections
                    .GetArrayElementAtIndex(0)
                    .FindPropertyRelative(ConnectionDrawer.PROP_VALUE).stringValue;

                if (string.IsNullOrEmpty(connectionId)) return string.Empty;
                return this.GraphTool.PortTools.TryGetValue(connectionId, out TPortTool inputPort)
                    ? inputPort.NodeTool.Title
                    : string.Empty;
            }
        }

        public override Texture Icon => this.Property
                .FindPropertyRelative(NodeStateMachineElbowDrawer.PROP_DIRECTION)
                .enumValueIndex switch
            {
                (int) PortPosition.Top => ICON_T.Texture,
                (int) PortPosition.Right => ICON_R.Texture,
                (int) PortPosition.Bottom => ICON_B.Texture,
                (int) PortPosition.Left => ICON_L.Texture,
                _ => throw new ArgumentOutOfRangeException()
            };

        public override bool ShowTitle => false;

        public override bool ShowInspectorTransitions => false;

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public NodeToolStateMachineElbow(TGraphTool graphTool, SerializedProperty property)
            : base(graphTool, property)
        { }
        
        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected override void RefreshPorts()
        {
            base.RefreshPorts();

            foreach (TPortTool inputPortTool in this.InputPortTools)
            {
                inputPortTool.RemoveFromHierarchy();
            }
            
            foreach (TPortTool outputPortTool in this.OutputPortTools)
            {
                outputPortTool.RemoveFromHierarchy();
            }

            foreach (TPortTool inputPortTool in this.InputPortTools)
            {
                int index = (int) inputPortTool.Port.Position;
                this.m_Ports[index].Insert(this.m_Ports[index].childCount - 1, inputPortTool);
            }
            
            foreach (TPortTool outputPortTool in this.OutputPortTools)
            {
                int index = (int) outputPortTool.Port.Position;
                this.m_Ports[index].Insert(this.m_Ports[index].childCount - 1, outputPortTool);
            }
        }
    }
}