using System.Collections.Generic;
using GameCreator.Runtime.Behavior;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    internal class PortToolStateMachine : TPortTool
    {
        private const float SIZE = 0.5f;

        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public PortToolStateMachine(TNodeTool nodeTool, SerializedProperty propertyPort)
            : base(nodeTool, propertyPort)
        { }
        
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
    }
}