using System;
using System.Collections.Generic;
using GameCreator.Runtime.Behavior;
using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.Toolbars;
using UnityEngine;

namespace GameCreator.Editor.Behavior
{
    [Overlay(
        id = ID,
        displayName = NAME,
        editorWindowType = typeof(WindowBehaviorTree),
        defaultDisplay = true,
        defaultDockZone = DockZone.TopToolbar,
        defaultDockPosition = DockPosition.Top,
        defaultDockIndex = 1,
        defaultLayout = Layout.HorizontalToolbar
    )]

    internal class ToolbarBehaviorTree : TToolbar
    {
        public class Block
        {
            // PROPERTIES: ------------------------------------------------------------------------
            
            public string NodeId { get; }
            public Vector2 Position { get; set; }
            
            public float NodeWidth { get; }
            public float NodeHeight { get; }

            public float BlockWidth { get; set; }
            public float BlockHeight { get; set; }

            public Block Parent { get; }
            public List<Block> Children { get; }
            
            // CONSTRUCTOR: -----------------------------------------------------------------------

            public Block(string nodeId, float width, float height, Block parent)
            {
                this.NodeId = nodeId;

                this.NodeWidth = width;
                this.NodeHeight = height;

                this.BlockWidth = 0f;
                this.BlockHeight = 0f;

                this.Position = Vector2.zero;

                this.Parent = parent;
                this.Children = new List<Block>();
            }
        }
        
        ///////////////////////////////////////////////////////////////////////////////////////////
        // CONSTANTS: -----------------------------------------------------------------------------

        private static readonly IIcon ICON_FRAME = new IconSort(ColorTheme.Type.TextNormal);
        
        private const float BLOCK_SPACE = 50f;
        private const float SIBLING_SPACE = 20f;
        
        // OVERRIDE METHODS: ----------------------------------------------------------------------
        
        protected override void CreateButtons()
        {
            base.CreateButtons();
            
            EditorToolbarButton alignButton = new EditorToolbarButton(
                string.Empty,
                ICON_FRAME.Texture,
                this.AlignNodes
            )
            {
                tooltip = "Align Nodes"
            };

            this.m_Toolbar.Add(alignButton);
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void AlignNodes()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;
            if (this.GraphWindow.CurrentPage is not ToolBehaviorTree tool) return;
            
            BehaviorTree tree = tool.Graph as BehaviorTree;
            if (tree == null || tree.Nodes.Length <= 0) return;

            List<Block> blocks = Sort(tool, tree);
            if (blocks.Count <= 0) return;
            
            Vector2 offset = new Vector2(blocks[0].BlockWidth * 0.5f, 0f);
            foreach (Block block in blocks)
            {
                Vector2 position = block.Position - offset;
                tool.NodeTools[block.NodeId].PositionWithoutNotify = position;
            }
            
            tool.Refresh();
        }

        private static List<Block> Sort(TGraphTool tool, Graph tree)
        {
            if (tool == null) return new List<Block>();
            Block root = BuildData(tool, tree.Nodes[0].Id.String, null);
            
            CalculateSizes(root);
            CalculatePositions(root, 0f);

            Stack<Block> candidates = new Stack<Block>();
            candidates.Push(root);

            List<Block> list = new List<Block>();
            while (candidates.Count > 0)
            {
                Block data = candidates.Pop();
                list.Add(data);
                
                foreach (Block block in data.Children)
                {
                    candidates.Push(block);
                }
            }

            return list;
        }
        
        private static Block BuildData(TGraphTool tool, string nodeId, Block parent)
        {
            Block block = new Block(
                nodeId,
                GetWidth(tool, nodeId),
                GetHeight(tool, nodeId),
                parent
            );

            TPortTool[] outputPorts = tool.NodeTools[nodeId].OutputPortTools;
            foreach (TPortTool outputPort in outputPorts)
            {
                SerializedProperty connections = outputPort.Connections;
                for (int i = 0; i < connections.arraySize; ++i)
                {
                    SerializedProperty inputPort = connections.GetArrayElementAtIndex(i);
                    string inputPortId = inputPort
                        .FindPropertyRelative(ConnectionDrawer.PROP_VALUE)
                        .stringValue;

                    string subNodeId = tool.PortTools[inputPortId].NodeTool.NodeId;
                    Block subBlock = BuildData(tool, subNodeId, block);
                    
                    block.Children.Add(subBlock);
                }
            }

            return block;
        }

        private static float CalculateSizes(Block block)
        {
            float childrenWidth = 0.0f;
            int childrenCount = block.Children.Count;

            for (int i = 0; i < childrenCount; ++i)
            {
                childrenWidth += CalculateSizes(block.Children[i]);
            }

            float spaceBetween = (childrenCount - 1) * SIBLING_SPACE;
            block.BlockWidth = Math.Max(block.NodeWidth, childrenWidth + spaceBetween);

            return block.BlockWidth;
        }

        private static float CalculatePositions(Block data, float offset)
        {
            data.Position = new Vector2(
                data.BlockWidth * 0.5f - data.NodeWidth * 0.5f + offset,
                data.Parent?.BlockHeight ?? 0f
            );

            data.BlockHeight = data.Position.y + data.NodeHeight + BLOCK_SPACE;
            int childrenCount = data.Children.Count;
            float avgCenters = 0f;

            for (int i = 0; i < childrenCount; ++i)
            {
                offset += CalculatePositions(data.Children[i], offset + SIBLING_SPACE * i);
                avgCenters += data.Children[i].Position.x;
            }

            if (childrenCount > 0)
            {
                data.Position = new Vector2(
                    avgCenters / childrenCount,
                    data.Position.y
                );
            }

            return data.BlockWidth;
        }

        private static float GetWidth(TGraphTool tool, string nodeId)
        {
            return tool.NodeTools[nodeId].layout.width;
        }

        private static float GetHeight(TGraphTool tool, string nodeId)
        {
            return tool.NodeTools[nodeId].layout.height;
        }
    }
}