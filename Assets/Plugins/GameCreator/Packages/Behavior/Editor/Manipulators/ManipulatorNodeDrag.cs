using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    internal class ManipulatorNodeDrag : MouseManipulator
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private bool m_IsDragging;
        [NonSerialized] private Vector2 m_StartMousePosition;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public ManipulatorNodeDrag()
        {
            this.m_IsDragging = false;
            this.activators.Add(new ManipulatorActivationFilter
            {
                button = MouseButton.LeftMouse
            });
        }
        
        // REGISTERS: -----------------------------------------------------------------------------
        
        protected override void RegisterCallbacksOnTarget()
        {
            this.target.RegisterCallback<MouseDownEvent>(this.OnMouseDown);
            this.target.RegisterCallback<MouseMoveEvent>(this.OnMouseMove);
            this.target.RegisterCallback<MouseUpEvent>(this.OnMouseUp);
            this.target.RegisterCallback<MouseCaptureOutEvent>(this.OnMouseCaptureOut);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            this.target.UnregisterCallback<MouseDownEvent>(this.OnMouseDown);
            this.target.UnregisterCallback<MouseMoveEvent>(this.OnMouseMove);
            this.target.UnregisterCallback<MouseUpEvent>(this.OnMouseUp);
            this.target.UnregisterCallback<MouseCaptureOutEvent>(this.OnMouseCaptureOut);
        }
        
        // CALLBACK METHODS: ----------------------------------------------------------------------
        
        private void OnMouseDown(MouseDownEvent eventMouseDown)
        {
            if (this.m_IsDragging)
            {
                eventMouseDown.StopImmediatePropagation();
                return;
            }

            if (!CanStartManipulation(eventMouseDown)) return;
            if (this.target is not TNodeTool) return;
            
            this.m_StartMousePosition = eventMouseDown.localMousePosition;
            this.m_IsDragging = true;

            this.target.CaptureMouse();
            eventMouseDown.StopPropagation();
        }
        
        private void OnMouseMove(MouseMoveEvent eventMouseMove)
        {
            if (!this.m_IsDragging) return;
            if (this.target is not TNodeTool nodeTool) return;

            Vector2 currentMousePosition = eventMouseMove.localMousePosition;
            Vector2 movementFromStart = currentMousePosition - this.m_StartMousePosition;
            
            TNodeTool[] selection = nodeTool.GraphTool.Window.Selection.Group;
            if (selection.Length == 0) return;
            
            BatchMoveSelection(selection, movementFromStart);
            eventMouseMove.StopPropagation();
        }
        
        private void OnMouseUp(MouseUpEvent eventMouseUp)
        {
            if (!this.m_IsDragging) return;
            if (!CanStopManipulation(eventMouseUp)) return;

            this.m_IsDragging = false;
            
            eventMouseUp.StopPropagation();
            this.target.ReleaseMouse();
        }
        
        private void OnMouseCaptureOut(MouseCaptureOutEvent eventMouseOut)
        {
            if (!this.m_IsDragging) return;
            
            this.m_IsDragging = false;
            this.target.ReleaseMouse();
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private static void BatchMoveSelection(TNodeTool[] selection, Vector2 movement)
        {
            List<TNodeTool> parents = new List<TNodeTool>();
            
            for (int i = 0; i < selection.Length - 1; ++i)
            {
                TNodeTool nodeTool = selection[i];
                nodeTool.PositionWithoutNotify += movement;

                TNodeTool parentTool = FirstParent(nodeTool);
                if (parentTool == null || parents.Contains(parentTool)) continue;
                parents.Add(parentTool);
            }

            TNodeTool lastNodeTool = selection[^1]; 
            lastNodeTool.Position += movement;

            TNodeTool lastParentTool = FirstParent(lastNodeTool);
            if (lastParentTool != null && !parents.Contains(lastParentTool))
            {
                parents.Add(lastParentTool);
            }

            foreach (TNodeTool parent in parents)
            {
                parent.OnMoveChildren();
            }
            
            if (parents.Count == 0) return;
            parents[0].GraphTool.Refresh();
        }
        
        private static TNodeTool FirstParent(TNodeTool nodeTool)
        {
            foreach (TPortTool inputPort in nodeTool.InputPortTools)
            {
                string inputPortId = inputPort.PortId;
                foreach (KeyValuePair<string, TNodeTool> entryNodeTool in nodeTool.GraphTool.NodeTools)
                {
                    foreach (TPortTool outputPortTool in entryNodeTool.Value.OutputPortTools)
                    {
                        for (int i = 0; i < outputPortTool.Connections.arraySize; ++i)
                        {
                            string connectionId = outputPortTool.Connections
                                .GetArrayElementAtIndex(i)
                                .FindPropertyRelative(ConnectionDrawer.PROP_VALUE)
                                .stringValue;

                            if (connectionId == inputPortId)
                            {
                                return entryNodeTool.Value;
                            }
                        }
                    }
                }
            }
            
            return null;
        }
    }
}
