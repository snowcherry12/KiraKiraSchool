using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    internal class ManipulatorGraphSelect : MouseManipulator
    {
        private const EventModifiers MODIFIERS_ADDITIVE = EventModifiers.Command |
                                                          EventModifiers.Control |
                                                          EventModifiers.Shift;
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private readonly TGraphTool m_GraphTool;
        [NonSerialized] private Vector2 m_StartMousePosition;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        [field: NonSerialized] public bool IsSelecting { get; private set; }

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public ManipulatorGraphSelect(TGraphTool graphTool)
        {
            this.m_GraphTool = graphTool;
            this.IsSelecting = false;
            
            this.activators.Add(new ManipulatorActivationFilter
            {
                button = MouseButton.LeftMouse
            });
            
            this.activators.Add(new ManipulatorActivationFilter
            {
                button = MouseButton.LeftMouse,
                modifiers = EventModifiers.Command
            });
            
            this.activators.Add(new ManipulatorActivationFilter
            {
                button = MouseButton.LeftMouse,
                modifiers = EventModifiers.Control
            });
            
            this.activators.Add(new ManipulatorActivationFilter
            {
                button = MouseButton.LeftMouse,
                modifiers = EventModifiers.Shift
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
            if (this.IsSelecting)
            {
                eventMouseDown.StopImmediatePropagation();
                return;
            }

            if (!CanStartManipulation(eventMouseDown)) return;
            if (this.target is not TGraphTool) return;

            this.m_StartMousePosition = eventMouseDown.localMousePosition;
            this.IsSelecting = true;
            
            this.m_GraphTool.Select.Selection = new Rect(this.m_StartMousePosition, Vector2.zero);

            this.target.CaptureMouse();
            eventMouseDown.StopPropagation();
        }
        
        private void OnMouseMove(MouseMoveEvent eventMouseMove)
        {
            if (!this.IsSelecting) return;
            if (this.target is not TGraphTool) return;

            Vector2 currentMousePosition = eventMouseMove.localMousePosition;
            Vector2 size = currentMousePosition - this.m_StartMousePosition;
            
            this.m_GraphTool.Select.Selection = new Rect(this.m_StartMousePosition, size);

            eventMouseMove.StopPropagation();
        }
        
        private void OnMouseUp(MouseUpEvent eventMouseUp)
        {
            if (!this.IsSelecting) return;
            if (!CanStopManipulation(eventMouseUp)) return;

            this.IsSelecting = false;

            Rect selection = this.m_GraphTool.Select.Selection;

            List<TNodeTool> candidates = new List<TNodeTool>();
            foreach (KeyValuePair<string, TNodeTool> entry in this.m_GraphTool.NodeTools)
            {
                Rect candidate = entry.Value.ChangeCoordinatesTo(
                    this.m_GraphTool.Select,
                    entry.Value.layout
                );
                
                if (selection.Overlaps(candidate, true))
                {
                    candidates.Add(entry.Value);
                }
            }

            if ((eventMouseUp.modifiers & MODIFIERS_ADDITIVE) != 0)
            {
                TNodeTool[] currentNodesSelection = this.m_GraphTool.Window.Selection.Group;
                foreach (TNodeTool nodeSelection in currentNodesSelection)
                {
                    if (candidates.Contains(nodeSelection)) continue;
                    candidates.Add(nodeSelection);
                }
            }

            this.m_GraphTool.Window.Selection.SelectGroup(candidates);
            this.m_GraphTool.Select.Selection = Rect.zero;
            
            eventMouseUp.StopPropagation();
            this.target.ReleaseMouse();
        }
        
        private void OnMouseCaptureOut(MouseCaptureOutEvent eventMouseOut)
        {
            if (!this.IsSelecting) return;
            
            this.IsSelecting = false;
            this.target.ReleaseMouse();
        }
    }
}