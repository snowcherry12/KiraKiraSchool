using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    internal class GraphSelect : VisualElement
    {
        private const float LINE_WIDTH = 1f;
        private static readonly Color COLOR_TRANSPARENCY = new Color(1f, 1f, 1f, 0.1f);

        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private readonly TGraphTool m_GraphTool;

        [NonSerialized] private Rect m_Selection = Rect.zero;
        
        // EVENTS: --------------------------------------------------------------------------------

        public event Action EventChange;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public Rect Selection
        {
            get => this.m_Selection;
            set
            {
                this.m_Selection = value;
                
                this.EventChange?.Invoke();
                this.m_GraphTool?.Select.MarkDirtyRepaint();
            }
        }

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public GraphSelect(TGraphTool graphTool)
        {
            this.m_GraphTool = graphTool;
            this.generateVisualContent += this.OnGenerateVisualContent;
        }

        // DRAW METHODS: --------------------------------------------------------------------------

        private void OnGenerateVisualContent(MeshGenerationContext context)
        {
            Painter2D painter = context.painter2D;
            
            if (this.m_Selection == Rect.zero) return;
            
            painter.lineJoin = LineJoin.Round;
            painter.lineWidth = LINE_WIDTH;
            
            painter.strokeColor = GraphUtils.Orange;
            painter.fillColor = GraphUtils.Orange * COLOR_TRANSPARENCY;
            
            painter.BeginPath();
            
            painter.MoveTo(this.m_Selection.position);
            painter.LineTo(this.m_Selection.position + Vector2.right * this.m_Selection.width);
            painter.LineTo(this.m_Selection.position + this.m_Selection.size);
            painter.LineTo(this.m_Selection.position + Vector2.up * this.m_Selection.height);
            
            painter.ClosePath();
            
            painter.Stroke();
            painter.Fill();
        }
    }
}