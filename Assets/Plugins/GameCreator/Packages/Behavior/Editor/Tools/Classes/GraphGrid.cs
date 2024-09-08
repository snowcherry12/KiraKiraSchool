using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    internal class GraphGrid : VisualElement
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private readonly TGraphTool m_GraphTool;
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        private static float Spacing => 15f;
        private static int Thickness => 10;
        
        private static Color LineColor => EditorGUIUtility.isProSkin 
            ? new Color(200/255f, 200/255f, 200/255f, 0.05f) 
            : new Color(65/255f, 65/255f, 65/255f, 0.07f);

        private static Color ThickLineColor => EditorGUIUtility.isProSkin
            ? new Color(200/255f, 200/255f, 200/255f, 0.1f) 
            : new Color(65/255f, 65/255f, 65/255f, 0.1f);

        private static Color BackgroundColor => EditorGUIUtility.isProSkin
            ? new Color(32 / 255f, 32 / 255f, 32 / 255f)
            : new Color(180 / 255f, 180 / 255f, 180 / 255f);
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public GraphGrid(TGraphTool graphTool)
        {
            this.m_GraphTool = graphTool;
            this.pickingMode = PickingMode.Ignore;

            this.StretchToParentSize();
            this.generateVisualContent += GenerateVisualContent;

            this.m_GraphTool.style.backgroundColor = new StyleColor(BackgroundColor);
        }

        // PAINT GRID METHODS: --------------------------------------------------------------------
        
        private void GenerateVisualContent(MeshGenerationContext context)
        {
            if (this.m_GraphTool == null) return;
            
            Matrix4x4 viewMatrix = this.m_GraphTool.View.ContentMatrix;
            Rect graphToolLayout = this.m_GraphTool.layout;
            
            graphToolLayout.x = 0;
            graphToolLayout.y = 0;
            
            float scale = this.m_GraphTool.View.Scale;
            
            Vector4 containerTranslation = viewMatrix.GetColumn(3);
            Rect containerPosition = this.m_GraphTool.View.ContentRect;

            Painter2D painter = context.painter2D;

            void Line(Vector2 from, Vector2 to)
            {
                painter.MoveTo(GraphUtils.Clip(graphToolLayout, from));
                painter.LineTo(GraphUtils.Clip(graphToolLayout, to));
            }

            Vector3 from = new Vector3(graphToolLayout.x, graphToolLayout.y, 0.0f);
            Vector3 to = new Vector3(graphToolLayout.x, graphToolLayout.height, 0.0f);

            Matrix4x4 tx = Matrix4x4.TRS(containerTranslation, Quaternion.identity, Vector3.one);

            from = tx.MultiplyPoint(from);
            to = tx.MultiplyPoint(to);
            
            from.x += containerPosition.x * scale;
            from.y += containerPosition.y * scale;
            to.x += containerPosition.x * scale;
            to.y += containerPosition.y * scale;

            float thickGridLineX = from.x;
            float thickGridLineY = from.y;
            
            from.x = from.x % (Spacing * scale) - Spacing * scale;
            to.x = from.x;

            from.y = graphToolLayout.y;
            to.y = graphToolLayout.y + graphToolLayout.height;

            painter.BeginPath();
            painter.strokeColor = LineColor;
            while (from.x < graphToolLayout.width)
            {
                from.x += Spacing * scale;
                to.x += Spacing * scale;

                Line(from, to);
            }
            painter.Stroke();

            float thickLineSpacing = Spacing * Thickness;
            from.x = to.x = thickGridLineX % (thickLineSpacing * scale) - thickLineSpacing * scale;

            painter.BeginPath();
            painter.strokeColor = ThickLineColor;
            while (from.x < graphToolLayout.width + thickLineSpacing)
            {
                Line(from, to);
                
                from.x += Spacing * scale * Thickness;
                to.x += Spacing * scale * Thickness;
            }
            
            painter.Stroke();
            
            from = new Vector3(graphToolLayout.x, graphToolLayout.y, 0.0f);
            to = new Vector3(graphToolLayout.x + graphToolLayout.width, graphToolLayout.y, 0.0f);
            
            from.x += containerPosition.x * scale;
            from.y += containerPosition.y * scale;
            to.x += containerPosition.x * scale;
            to.y += containerPosition.y * scale;

            from = tx.MultiplyPoint(from);
            to = tx.MultiplyPoint(to);
            
            from.y = to.y = from.y % (Spacing * scale) - Spacing * scale;
            from.x = graphToolLayout.x;
            to.x = graphToolLayout.width;

            painter.BeginPath();
            painter.strokeColor = LineColor;
            
            while (from.y < graphToolLayout.height)
            {
                from.y += Spacing * scale;
                to.y += Spacing * scale;
                Line(from, to);
            }
            
            painter.Stroke();

            thickLineSpacing = Spacing * Thickness;
            from.y = to.y = thickGridLineY % (thickLineSpacing * scale) - thickLineSpacing * scale;

            painter.BeginPath();
            painter.strokeColor = ThickLineColor;
            
            while (from.y < graphToolLayout.height + thickLineSpacing)
            {
                Line(from, to);
                
                from.y += Spacing * scale * Thickness;
                to.y += Spacing * scale * Thickness;
            }
            
            painter.Stroke();
        }
    }
}