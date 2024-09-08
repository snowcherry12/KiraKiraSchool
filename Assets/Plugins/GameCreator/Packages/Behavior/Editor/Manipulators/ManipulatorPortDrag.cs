using System;
using System.Collections.Generic;
using GameCreator.Runtime.Behavior;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    internal class ManipulatorPortDrag : MouseManipulator
    {
        private const float CANDIDATE_MAX_DISTANCE = 50f;
        
        private static readonly Vector2 TRANSFORM_COORDINATES = new Vector2(1f, -1f);
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private readonly TPortTool m_PortTool;
        [NonSerialized] private Vector2 m_StartLocalPosition;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        [field: NonSerialized] public bool IsDragging { get; private set; }
        [field: NonSerialized] public Vector2 LocalMousePosition { get; private set; }
        [field: NonSerialized] public TPortTool Candidate { get; private set; }

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public ManipulatorPortDrag(TPortTool portTool)
        {
            this.m_PortTool = portTool;
            
            this.IsDragging = false;
            this.LocalMousePosition = Vector2.zero;
            this.Candidate = null;
            
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
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public bool CanConnectTo(TPortTool connectTo)
        {
            if (connectTo == null) return false;
            
            TPort portA = this.m_PortTool.Port;
            TPort portB = connectTo.Port;

            if (portA.Mode == portB.Mode) return false;
            
            TInputPort input = (portA.Mode == PortMode.Input ? portA : portB) as TInputPort;
            TOutputPort output = (portA.Mode == PortMode.Output ? portA : portB) as TOutputPort;
            
            return input?.CanConnectFrom(output) ?? false;
        }

        // CALLBACK METHODS: ----------------------------------------------------------------------
        
        private void OnMouseDown(MouseDownEvent eventMouseDown)
        {
            if (this.IsDragging)
            {
                eventMouseDown.StopImmediatePropagation();
                return;
            }

            if (!CanStartManipulation(eventMouseDown)) return;
            if (this.target is not TPortTool) return;

            this.m_StartLocalPosition = eventMouseDown.localMousePosition;
            
            this.IsDragging = true;
            this.LocalMousePosition = eventMouseDown.localMousePosition;
            this.Candidate = null;
            
            this.target.CaptureMouse();

            eventMouseDown.StopPropagation();
            this.m_PortTool.GraphTool.Refresh();
        }
        
        private void OnMouseMove(MouseMoveEvent eventMouseMove)
        {
            if (!this.IsDragging) return;
            if (this.target is not TPortTool) return;

            this.LocalMousePosition = eventMouseMove.localMousePosition;
            this.Candidate = this.GetCandidate(eventMouseMove.localMousePosition);

            eventMouseMove.StopPropagation();
            this.m_PortTool.NodeTool.GraphTool.Refresh();
        }

        private void OnMouseUp(MouseUpEvent eventMouseUp)
        {
            if (!this.IsDragging) return;
            if (!CanStopManipulation(eventMouseUp)) return;

            this.IsDragging = false;
            this.LocalMousePosition = eventMouseUp.localMousePosition;

            this.Candidate = this.GetCandidate(eventMouseUp.localMousePosition);

            if (this.Candidate == null)
            {
                Vector2 direction = eventMouseUp.localMousePosition - this.m_StartLocalPosition;
                direction *= TRANSFORM_COORDINATES;

                if (this.m_PortTool?.AutoCreateNode != null)
                {
                    this.m_PortTool.AutoCreateNode.Invoke(
                        eventMouseUp,
                        this.m_PortTool?.PortId,
                        direction.normalized
                    );
                }
                else
                {
                    this.m_PortTool.GraphTool.OpenCreationMenu(
                        eventMouseUp, 
                        this.m_PortTool?.PortId,
                        direction.normalized
                    );
                }
            }
            else
            {
                this.m_PortTool.GraphTool.ConnectPortTools(
                    this.m_PortTool,
                    this.Candidate
                );
            }
            
            eventMouseUp.StopPropagation();
            this.m_PortTool.GraphTool.Refresh();
            
            this.target.ReleaseMouse();
        }
        
        private void OnMouseCaptureOut(MouseCaptureOutEvent eventMouseOut)
        {
            if (!this.IsDragging) return;
            
            this.IsDragging = false;
            this.m_PortTool.GraphTool.Refresh();
            
            this.target.ReleaseMouse();
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private TPortTool GetCandidate(Vector2 mouse)
        {
            TPortTool closestPort = null;
            float closestDistance = 0f;
            
            Dictionary<string, TPortTool> portTools = this.m_PortTool.GraphTool.PortTools;

            foreach (KeyValuePair<string, TPortTool> entry in portTools)
            {
                if (entry.Value == null) continue;

                Vector2 candidatePosition = entry.Value.GetPortPositionFor(this.m_PortTool);
                float candidateDistance = Vector2.Distance(mouse, candidatePosition);
                
                if (closestPort != null && candidateDistance > closestDistance) continue;
                if (!this.CanConnectTo(entry.Value)) continue;
                
                closestPort = entry.Value;
                closestDistance = candidateDistance;
            }

            float scale = this.m_PortTool.GraphTool.View.Scale;
            float maxDistance = CANDIDATE_MAX_DISTANCE / scale;

            return closestDistance <= maxDistance ? closestPort : null;
        }
    }
}