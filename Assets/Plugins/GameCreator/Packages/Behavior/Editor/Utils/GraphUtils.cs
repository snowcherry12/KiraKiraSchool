using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Behavior;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    internal static class GraphUtils
    {
        public static Color Gray => EditorGUIUtility.isProSkin
            ? new Color(193f / 255f, 193f / 255f, 193f / 255f)
            : new Color(90f / 255f, 90f / 255f, 90f / 255f);
        
        public static Color Blue => EditorGUIUtility.isProSkin
            ? new Color(103f / 255f, 190f / 255f, 249f / 255f)
            : new Color(44f / 255f, 108f / 255f, 195f / 255f);
        
        public static Color Green => EditorGUIUtility.isProSkin
            ? new Color(130f / 255f, 209f / 255f, 54f / 255f)
            : new Color(67f / 255f, 121f / 255f, 59f / 255f);

        public static Color Red => EditorGUIUtility.isProSkin
            ? new Color(252f / 255f, 51f / 255f, 51f / 255f)
            : new Color(159f / 255f, 37f / 255f, 26f / 255f);

        public static Color Orange => EditorGUIUtility.isProSkin
            ? new Color(206f / 255f, 123f / 255f, 72f / 255f)
            : new Color(161f / 255f, 87f / 255f, 34f / 255f);
        
        public static Color Dark => EditorGUIUtility.isProSkin
            ? new Color(26f / 255f, 26f / 255f, 26f / 255f)
            : new Color(146 / 255f, 189 / 255f, 255 / 255f, 0.11f);
        
        private const int MAX_COUNT = 2;
        
        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public static float RoundToPixelGrid(float value)
        {
            double pixelsPerPoint = EditorGUIUtility.pixelsPerPoint;
            float result = (float) (value * pixelsPerPoint + 0.48f);
            
            return Mathf.Floor(result) / (float) pixelsPerPoint;
        }
        
        public static Rect ExpandWith(Rect rect, Rect other)
        {
            if (other.xMin < rect.xMin) rect.xMin = other.xMin;
            if (other.xMax > rect.xMax) rect.xMax = other.xMax;
            
            if (other.yMin < rect.yMin) rect.yMin = other.yMin;
            if (other.yMax > rect.yMax) rect.yMax = other.yMax;
            
            return rect;
        }

        public static Rect Expand(Rect rect, float padding)
        {
            rect.x -= padding;
            rect.y -= padding;
            rect.width += padding * 2f;
            rect.height += padding * 2f;

            return rect;
        }
        
        public static Vector3 Clip(Rect rect, Vector3 point)
        {
            if (point.x < rect.xMin) point.x = rect.xMin;
            if (point.x > rect.xMax) point.x = rect.xMax;

            if (point.y < rect.yMin) point.y = rect.yMin;
            if (point.y > rect.yMax) point.y = rect.yMax;

            return point;
        }

        public static Color GetColor(Status status, Color readyColor) => status switch
        {
            Status.Ready => readyColor,
            Status.Running => Blue,
            Status.Success => Green,
            Status.Failure => Red,
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };

        public static bool CanReach(TGraphTool graph, TNodeTool sourceNode, string reachNodeId)
        {
            if (sourceNode.NodeId == reachNodeId) return true;

            foreach (TPortTool sourcePort in sourceNode.OutputPortTools)
            {
                SerializedProperty sourceConnections = sourcePort.Connections;
                for (int i = 0; i < sourceConnections.arraySize; ++i)
                {
                    string sourceConnection = sourceConnections
                        .GetArrayElementAtIndex(i)
                        .FindPropertyRelative(ConnectionDrawer.PROP_VALUE)
                        .stringValue;
                    
                    if (string.IsNullOrEmpty(sourceConnection)) continue;
                    TPortTool sourceConnectionPort = graph.PortTools[sourceConnection];
                    
                    if (CanReach(graph, sourceConnectionPort.NodeTool, reachNodeId))
                    {
                        return true;
                    }
                }
            }
            
            return false;
        }
        
        public static bool HasMultipleInstances(Graph root, Graph graph)
        {
            if (graph == null) return false;

            int totalCount = 0;
            TooManyGraphInstances(root, graph, ref totalCount);
            
            return totalCount >= MAX_COUNT;
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private static void TooManyGraphInstances(Graph root, Graph graph, ref int count)
        {
            if (graph == null) return;

            if (root == graph) count += 1;
            if (count >= MAX_COUNT) return;

            foreach (TNode node in root.Nodes)
            {
                Graph subGraph = node.Subgraph;
                if (subGraph == null) continue;

                TooManyGraphInstances(subGraph, graph, ref count);
            }
        }
    }
}