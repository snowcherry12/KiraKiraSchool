using System;
using System.Collections.Generic;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Behavior;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    internal abstract class TGraphWires : VisualElement
    {
        private static readonly Color COLOR_DRAG = new Color(1f, 1f, 1f, 0.5f);

        private const float SIZE_DOT = 3f;

        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] protected readonly TGraphTool m_GraphTool;
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected abstract bool DrawDirection { get; }

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public TGraphWires(TGraphTool graphTool)
        {
            this.m_GraphTool = graphTool;
            this.generateVisualContent += this.OnGenerateVisualContent;
        }

        // DRAW METHODS: --------------------------------------------------------------------------

        private void OnGenerateVisualContent(MeshGenerationContext context)
        {
            this.DrawConnections(context);
            this.DrawDragWire(context);
        }

        private void DrawConnections(MeshGenerationContext context)
        {
            Painter2D painter = context.painter2D;
            Dictionary<string, TNodeTool> nodeTools = this.m_GraphTool.NodeTools;

            foreach (KeyValuePair<string, TNodeTool> entry in nodeTools)
            {
                foreach (TPortTool outputPortTool in entry.Value.OutputPortTools)
                {
                    SerializedProperty connections = outputPortTool.Connections;
                    for (int i = 0; i < connections.arraySize; ++i)
                    {
                        SerializedProperty connection = connections.GetArrayElementAtIndex(i);
                        string inputPortId = connection.FindPropertyRelative(ConnectionDrawer.PROP_VALUE).stringValue;

                        if (!this.m_GraphTool.PortTools.TryGetValue(inputPortId, out TPortTool inputPortTool))
                        {
                            continue;
                        }
                        
                        if (inputPortTool == null) continue;
                        
                        Vector2 pointA = outputPortTool.GetPortPositionFor(this);
                        Vector2 pointB = inputPortTool.GetPortPositionFor(this);

                        DrawWire(
                            painter,
                            this.GetWireColor(inputPortTool.NodeTool), 
                            WireUtils.WIRE_THICKNESS * this.m_GraphTool.View.Scale, 
                            pointA, 
                            pointB,
                            outputPortTool.Direction,
                            inputPortTool.Direction,
                            outputPortTool.Shape
                        );

                        this.AfterDrawConnection(
                            context,
                            pointA,
                            pointB,
                            i
                        );
                    }
                }
            }
        }

        private void DrawDragWire(MeshGenerationContext context)
        {
            Painter2D painter = context.painter2D;
            Dictionary<string, TNodeTool> nodeTools = this.m_GraphTool.NodeTools;

            foreach (KeyValuePair<string, TNodeTool> entry in nodeTools)
            {
                foreach (TPortTool inputPortTool in entry.Value.InputPortTools)
                {
                    this.DrawPortTool(painter, inputPortTool);
                }
                
                foreach (TPortTool outputPortTool in entry.Value.OutputPortTools)
                {
                    this.DrawPortTool(painter, outputPortTool);
                }
            }
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void DrawPortTool(Painter2D painter, TPortTool portTool)
        {
            if (!portTool.IsDragging) return;
            
            float wireWidth = WireUtils.WIRE_THICKNESS * this.m_GraphTool.View.Scale;
            Vector2 nodePosition = portTool.GetPortPositionFor(this);

            if (portTool.Candidate != null)
            {
                TPort port = portTool.Port;
                WireShape shape = port.Mode == PortMode.Output
                    ? portTool.Shape
                    : portTool.Candidate.Shape;

                Vector2 candidatePosition = portTool.Candidate.GetPortPositionFor(this);
                this.DrawWire(
                    painter,
                    this.GetWireColor(portTool.NodeTool),
                    wireWidth,
                    nodePosition,
                    candidatePosition,
                    portTool.Direction,
                    portTool.Candidate.Direction,
                    shape
                );
            }
            
            Vector2 mousePosition = portTool.GetMousePositionFor(this);
            Color wireColor = this.GetWireColor(portTool.NodeTool); 
            
            this.DrawWire(
                painter,
                wireColor * COLOR_DRAG,
                wireWidth,
                nodePosition,
                mousePosition,
                portTool.Direction,
                Vector2.zero,
                WireShape.Linear
            );
            
            painter.lineJoin = LineJoin.Round;
            painter.fillColor = wireColor;

            painter.BeginPath();
            painter.Arc(mousePosition, SIZE_DOT, 0, Angle.Turns(1));
            painter.ClosePath();
            
            painter.Fill();
        }

        private void DrawWire(
            Painter2D painter, Color color, float width,
            Vector2 a, Vector2 b,
            Vector2 direction1, Vector2 direction2,
            WireShape wireShape
        )
        {
            painter.strokeColor = color;
            painter.fillColor = color;
            painter.lineWidth = width;

            switch (wireShape)
            {
                case WireShape.Linear:
                    WireUtils.DrawLinear(
                        painter, a, b,
                        this.DrawDirection
                    );
                    break;
                
                case WireShape.Bezier:
                    WireUtils.DrawBezier(
                        painter, this.m_GraphTool,
                        a, b, direction1, direction2,
                        this.DrawDirection
                    );
                    break;
                
                default: throw new ArgumentOutOfRangeException(nameof(wireShape), wireShape, null);
            }
        }

        // ABSTRACT METHODS: ----------------------------------------------------------------------

        protected abstract Color GetWireColor(TNodeTool nodeTool);

        protected abstract void AfterDrawConnection(MeshGenerationContext context, Vector2 a, Vector2 b, int index);
    }
}