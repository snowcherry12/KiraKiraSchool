using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    internal class ManipulatorGraphPan : MouseManipulator
    {
        private Vector2 m_StartMousePosition;
        private Vector2 m_StartContentPosition;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        [field: NonSerialized] public bool IsPanning { get; private set; }
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public ManipulatorGraphPan()
        {
            this.IsPanning = false;
            
            this.activators.Add(new ManipulatorActivationFilter
            {
                button = MouseButton.LeftMouse,
                modifiers = EventModifiers.Alt
            });
            
            activators.Add(new ManipulatorActivationFilter
            {
                button = MouseButton.MiddleMouse
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
            if (this.IsPanning)
            {
                eventMouseDown.StopImmediatePropagation();
                return;
            }

            if (!CanStartManipulation(eventMouseDown)) return;
            if (this.target is not TGraphTool graphTool) return;
            
            this.m_StartMousePosition = eventMouseDown.localMousePosition;
            this.m_StartContentPosition = graphTool.View.Position;

            this.IsPanning = true;
            this.target.CaptureMouse();

            eventMouseDown.StopImmediatePropagation();
        }
        
        private void OnMouseMove(MouseMoveEvent eventMouseMove)
        {
            if (!this.IsPanning) return;
            if (this.target is not TGraphTool graphTool) return;

            Vector2 currentMousePosition = eventMouseMove.localMousePosition;
            Vector2 deltaMousePosition = currentMousePosition - this.m_StartMousePosition;

            Vector2 newPosition = this.m_StartContentPosition + deltaMousePosition;
            graphTool.SetView(newPosition, graphTool.View.Scale);

            eventMouseMove.StopPropagation();
        }
        
        private void OnMouseUp(MouseUpEvent eventMouseUp)
        {
            if (!this.IsPanning) return;
            if (!CanStopManipulation(eventMouseUp)) return;

            StopManipulation();
            eventMouseUp.StopPropagation();
        }
        
        private void OnMouseCaptureOut(MouseCaptureOutEvent eventMouseOut)
        {
            if (!this.IsPanning) return;
            StopManipulation();
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private void StopManipulation()
        {
            if (this.target is not TGraphTool) return;

            this.IsPanning = false;
            this.target.ReleaseMouse();
        }
    }
}
