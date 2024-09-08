using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    internal static class WireUtils
    {
        public const int WIRE_THICKNESS = 2;
        private const float PORT_OFFSET = 0f;
        
        private const float BEZIER_DIAMETER = 16.0f;
        private const float BEZIER_RADIUS = BEZIER_DIAMETER * 0.5f;
        
        private const float ARROW_HEIGHT = 10f;
        private const float ARROW_WIDTH = 7f;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        private static float GetPortOffset(TGraphTool graphTool)
        {
            return PORT_OFFSET * graphTool.View.Scale;
        }
        
        private static float GetBezierDiameter(TGraphTool graphTool)
        {
            return BEZIER_DIAMETER * graphTool.View.Scale;
        }
        
        private static float GetBezierRadius(TGraphTool graphTool)
        {
            return BEZIER_RADIUS * graphTool.View.Scale;
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public static void DrawLinear(
            Painter2D painter,
            Vector2 a, Vector2 b,
            bool drawDirection)
        {
            painter.BeginPath();
            
            painter.MoveTo(a);
            painter.LineTo(b);
            
            painter.Stroke();

            if (!drawDirection) return;
            
            Vector2 position = (a + b) * 0.5f;
            Vector2 direction = (b - a).normalized;
            
            DrawWireArrow(painter, position, direction);
        }
        
        public static void DrawBezier(
            Painter2D painter, 
            TGraphTool graphTool,
            Vector2 p1, Vector2 p4,
            Vector2 dir1, Vector2 dir2,
            bool drawDirection)
        {
            float offset = GetPortOffset(graphTool) + GetBezierDiameter(graphTool);
            
            float totalDistance = Vector2.Distance(p1, p4);

            offset = Mathf.Min(offset, (totalDistance - offset) * 0.5f);
            offset = Mathf.Max(offset, 0f);

            Vector2 p2 = p1 - dir1 * offset * Vector2.left;
            Vector2 p3 = p4 - dir2 * offset * Vector2.left;
            
            painter.BeginPath();
            
            painter.MoveTo(p1);
            painter.LineTo(p2 - (p2 - p1).normalized * GetBezierRadius(graphTool));

            Vector2 wireDirection = (p2 - p3).normalized;
            
            painter.BezierCurveTo(p2, p2, p2 - wireDirection * GetBezierRadius(graphTool));
            painter.LineTo(p3 + wireDirection * GetBezierRadius(graphTool));
            
            painter.BezierCurveTo(p3, p3, p4 - (p4 - p3).normalized * GetBezierRadius(graphTool));
            painter.LineTo(p4);
            
            painter.Stroke();
            
            if (!drawDirection) return;
            
            Vector2 position = (p2 + p3) * 0.5f;
            Vector2 direction = (p3 - p2).normalized;
            
            DrawWireArrow(painter, position, direction);
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private static void DrawWireArrow(Painter2D painter, Vector2 position, Vector2 direction)
        {
            Vector2 perpendicular = new Vector2(direction.y, -direction.x);
            
            Vector2 p1 = position + direction * ARROW_HEIGHT * 0.5f;
            Vector2 p2 = position + perpendicular * ARROW_WIDTH - direction * ARROW_HEIGHT * 0.5f;
            Vector2 p3 = position - perpendicular * ARROW_WIDTH - direction * ARROW_HEIGHT * 0.5f;
            
            painter.BeginPath();
            
            painter.MoveTo(p1);
            painter.LineTo(p2);
            painter.LineTo(p3);
            
            painter.ClosePath();
            painter.Fill();
        }
    }
}