using GameCreator.Runtime.Behavior;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    internal class PortToolBehaviorTree : TPortTool
    {
        private const float SIZE_FULL = 1f;
        private const float SIZE_SMALL = 0.75f;
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public PortToolBehaviorTree(TNodeTool nodeTool, SerializedProperty propertyPort)
            : base(nodeTool, propertyPort)
        { }
        
        // OVERRIDE METHODS: ----------------------------------------------------------------------
        
        protected override void GenerateInput(Painter2D painter, TInputPort port)
        {
            painter.lineJoin = LineJoin.Round;

            Rect paintRect = this.localBound;
            paintRect.position = Vector2.zero;

            float size = string.IsNullOrEmpty(this.m_Text.text)
                ? SIZE_SMALL
                : SIZE_FULL;
            
            painter.BeginPath();
            painter.Arc(paintRect.center, paintRect.width * 0.5f * size, 0, Angle.Turns(1));
            painter.ClosePath();
            
            painter.Fill();
        }

        protected override void GenerateOutput(Painter2D painter, TOutputPort port)
        {
            painter.lineJoin = LineJoin.Round;

            Rect paintBounds = this.localBound;
            paintBounds.position = Vector2.zero;

            float offset = (paintBounds.width - paintBounds.width * SIZE_SMALL) / 2f; 
            Rect paintRect = GraphUtils.Expand(paintBounds, -offset);

            painter.BeginPath();
            
            painter.MoveTo(new Vector2(paintRect.xMin, paintRect.center.y));
            painter.LineTo(new Vector2(paintRect.center.x, paintRect.yMin));
            painter.LineTo(new Vector2(paintRect.xMax, paintRect.center.y));
            painter.LineTo(new Vector2(paintRect.center.x, paintRect.yMax));
            
            painter.ClosePath();
            painter.Fill();
        }
    }
}