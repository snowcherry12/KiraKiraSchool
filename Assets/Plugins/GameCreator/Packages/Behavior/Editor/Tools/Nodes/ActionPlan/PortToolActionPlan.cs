using System;
using GameCreator.Runtime.Behavior;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    internal class PortToolActionPlan : TPortTool
    {
        private const float SIZE = 0.5f;

        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public PortToolActionPlan(TNodeTool nodeTool, SerializedProperty propertyPort)
            : base(nodeTool, propertyPort)
        { }
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override Action<MouseUpEvent, string, Vector2> AutoCreateNode =>
            this.NodeTool.GetType() == typeof(NodeToolActionPlanTaskInstructions) ||
            this.NodeTool.GetType() == typeof(NodeToolActionPlanTaskSubgraph)
                ? this.CreateBeliefNode
                : null;

        // OVERRIDE METHODS: ----------------------------------------------------------------------
        
        protected override void GenerateInput(Painter2D painter, TInputPort port)
        {
            painter.lineJoin = LineJoin.Round;

            Rect paintRect = this.localBound;
            paintRect.position = Vector2.zero;

            painter.BeginPath();
            painter.Arc(paintRect.center, paintRect.width * 0.5f * SIZE, 0, Angle.Turns(1));
            painter.ClosePath();
            
            painter.Fill();
        }

        protected override void GenerateOutput(Painter2D painter, TOutputPort port)
        {
            painter.lineJoin = LineJoin.Round;

            Rect paintRect = this.localBound;
            paintRect.position = Vector2.zero;

            painter.BeginPath();
            painter.Arc(paintRect.center, paintRect.width * 0.5f * SIZE, 0, Angle.Turns(1));
            painter.ClosePath();
            
            painter.Fill();
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private void CreateBeliefNode(MouseUpEvent eventMouse, string fromPortId, Vector2 direction)
        {
            ArgData argData = new ArgData(
                this.GraphTool.View.ChangeToContentCoordinatesFrom(
                    this.GraphTool.Window.rootVisualElement, 
                    eventMouse.mousePosition
                ),
                direction,
                fromPortId,
                ManipulatorGraphMenu.OnCreateNode
            );

            TNode fromNode = this.GraphTool.PortTools[fromPortId].NodeTool.Node;
            this.GraphTool.CreateNode(fromNode.Ports.Outputs[0].Id.Value == fromPortId
                ? typeof(NodeActionPlanPreConditions)
                : typeof(NodeActionPlanPostConditions), argData);
        }
    }
}