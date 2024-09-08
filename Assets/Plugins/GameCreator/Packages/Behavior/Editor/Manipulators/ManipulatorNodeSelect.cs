using System;
using System.Collections.Generic;
using GameCreator.Editor.Common;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    internal class ManipulatorNodeSelect : MouseManipulator
    {
        private const EventModifiers MODIFIERS_ADDITIVE = EventModifiers.Command |
                                                          EventModifiers.Control |
                                                          EventModifiers.Shift;

        private const float MAX_DISTANCE = 5f;
        
        private static readonly EventModifiers[] ActivatorModifiers =
        {
            EventModifiers.Command,
            EventModifiers.Control,
            EventModifiers.Shift
        };
        
        // MEMBERS: -------------------------------------------------------------------------------

        private Vector2 m_DownPosition;
        private int m_DownCounter;
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public ManipulatorNodeSelect()
        {
            this.activators.Add(new ManipulatorActivationFilter
            {
                button = MouseButton.LeftMouse
            });
            
            this.activators.Add(new ManipulatorActivationFilter
            {
                button = MouseButton.RightMouse
            });
            
            foreach (EventModifiers activatorModifier in ActivatorModifiers)
            {
                this.activators.Add(new ManipulatorActivationFilter
                {
                    button = MouseButton.LeftMouse,
                    modifiers = activatorModifier
                });
            }
        }
        
        // REGISTERS: -----------------------------------------------------------------------------
        
        protected override void RegisterCallbacksOnTarget()
        {
            this.target.RegisterCallback<MouseDownEvent>(this.OnMouseDown);
            this.target.RegisterCallback<MouseUpEvent>(this.OnMouseUp);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            this.target.UnregisterCallback<MouseDownEvent>(this.OnMouseDown);
            this.target.UnregisterCallback<MouseUpEvent>(this.OnMouseUp);
        }
        
        // CALLBACK METHODS: ----------------------------------------------------------------------
        
        private void OnMouseDown(MouseDownEvent mouseDownEvent)
        {
            this.m_DownCounter = mouseDownEvent.clickCount;
            this.m_DownPosition = mouseDownEvent.mousePosition;
            
            if (this.target is not TNodeTool nodeTool) return;
            if (!this.CanStartManipulation(mouseDownEvent)) return;

            if (mouseDownEvent.button == (int) MouseButton.RightMouse)
            {
                SelectSingle(mouseDownEvent, nodeTool, false);
                return;
            }

            if (mouseDownEvent.clickCount != 1) return;
            SelectSingle(mouseDownEvent, nodeTool, true);
        }
        
        private void OnMouseUp(MouseUpEvent mouseUpEvent)
        {
            if (this.target is not TNodeTool nodeTool) return;
            if (!this.CanStartManipulation(mouseUpEvent)) return;

            if (this.m_DownCounter != 2) return;
            
            float distance = Vector2.Distance(this.m_DownPosition, mouseUpEvent.mousePosition);
            if (distance > MAX_DISTANCE) return;
            
            SelectSubgraph(mouseUpEvent, nodeTool);
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private static void SelectSingle(MouseDownEvent mouseDownEvent, TNodeTool nodeTool, bool stopPropagation)
        {
            TGraphTool graphTool = nodeTool.GraphTool;
            if (graphTool.Window.Selection.IsSelected(nodeTool)) return;
            
            TNodeTool[] currentSelection = graphTool.Window.Selection.Group;
            foreach (TNodeTool selectedNodeTool in currentSelection)
            {
                if (selectedNodeTool == nodeTool) return;
            }

            bool isAdditive = (mouseDownEvent.modifiers & MODIFIERS_ADDITIVE) != 0;

            List<TNodeTool> selection = new List<TNodeTool>(isAdditive
                ? graphTool.Window.Selection.Group
                : Array.Empty<TNodeTool>()
            )
            {
                nodeTool
            };
            
            graphTool.Window.Selection.SelectGroup(selection);
            if (stopPropagation) mouseDownEvent.StopPropagation();
        }

        private static void SelectSubgraph(MouseUpEvent mouseUpEvent, TNodeTool nodeTool)
        {
            List<TNodeTool> selection = new List<TNodeTool>();
            SelectChildren(selection, nodeTool);
            
            nodeTool.GraphTool.Window.Selection.SelectGroup(selection);
            mouseUpEvent.StopPropagation();
        }
        
        private static void SelectChildren(ICollection<TNodeTool> selection, TNodeTool value)
        {
            if (selection.Contains(value)) return;

            selection.Add(value);
            foreach (TPortTool outputPortTool in value.OutputPortTools)
            {
                SerializedProperty connectionIds = outputPortTool.Connections;
                for (int i = 0; i < connectionIds.arraySize; ++i)
                {
                    string portId = connectionIds
                        .GetArrayElementAtIndex(i)
                        .FindPropertyRelative(ConnectionDrawer.PROP_VALUE)
                        .stringValue;
                    
                    if (value.GraphTool.PortTools.TryGetValue(portId, out TPortTool inputPortTool))
                    {
                        TNodeTool inputNodeTool = inputPortTool?.NodeTool;
                        
                        if (inputNodeTool == null) continue;
                        SelectChildren(selection, inputNodeTool); 
                    }
                }
            }
        }
    }
}
