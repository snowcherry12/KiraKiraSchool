using GameCreator.Runtime.Behavior;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    internal delegate void ContextMenuHandle(string portId, TNodeTool newNode);
    
    internal class ArgData
    {
        public Vector2 Cursor { get; set; }
        public Vector2 Direction { get; private set; }
        public string FromPortId { get; private set; }
        
        public ContextMenuHandle Callback { get; set; }
        
        public ArgData(Vector2 cursor, Vector2 direction, string fromPortId, ContextMenuHandle callback)
        {
            this.Cursor = cursor;
            this.Direction = direction;
            this.FromPortId = fromPortId;
            this.Callback = callback;
        }
    }
    
    internal class ManipulatorGraphMenu : PointerManipulator
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        private readonly TGraphTool m_GraphTool;

        private bool m_UseContext;
        private string m_FromPortId;
        private Vector2 m_Direction;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public ManipulatorGraphMenu(TGraphTool graphTool)
        {
            this.m_GraphTool = graphTool;

            activators.Add(new ManipulatorActivationFilter
            {
                button = MouseButton.RightMouse
            });
        }

        // INITIALIZERS: --------------------------------------------------------------------------
        
        protected override void RegisterCallbacksOnTarget()
        {
            this.target.RegisterCallback<ContextClickEvent>(this.OnContextMenu);
            this.target.RegisterCallback<ContextualMenuPopulateEvent>(this.OnContextualMenu);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            this.target.UnregisterCallback<ContextClickEvent>(this.OnContextMenu);
            this.target.UnregisterCallback<ContextualMenuPopulateEvent>(this.OnContextualMenu);
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void CreationMenu(MouseUpEvent eventMouseUp, string fromPortId, Vector2 direction)
        {
            this.m_UseContext = false;
            this.m_FromPortId = fromPortId;
            this.m_Direction = direction;
            
            if (this.target.panel?.contextualMenuManager == null) return;
            this.target.panel.contextualMenuManager.DisplayMenu(eventMouseUp, this.target);
            
            eventMouseUp.StopPropagation();
            eventMouseUp.PreventDefault();
        }

        // CALLBACK METHODS: ----------------------------------------------------------------------

        private void OnContextMenu(ContextClickEvent eventContextClick)
        {
            if (!this.CanStartManipulation(eventContextClick)) return;

            this.m_UseContext = true;
            this.m_FromPortId = string.Empty;
            this.m_Direction = Vector2.zero;
            
            if (this.target.panel?.contextualMenuManager == null) return;
            this.target.panel.contextualMenuManager.DisplayMenu(eventContextClick, this.target);
            
            eventContextClick.StopPropagation();
            eventContextClick.PreventDefault();
        }
        
        private void OnContextualMenu(ContextualMenuPopulateEvent eventContextMenu)
        {
            if (this.m_UseContext)
            {
                this.m_GraphTool?.GenerateGraphContextMenu(eventContextMenu);
                return;
            }
            
            this.m_GraphTool?.GenerateCreateMenu(
                eventContextMenu, 
                this.m_FromPortId,
                this.m_Direction,
                OnCreateNode
            );
        }
        
        // PUBLIC CALLBACKS: ----------------------------------------------------------------------
        
        public static void OnCreateNode(string fromPortId, TNodeTool newNodeTool)
        {
            ConnectNodes(fromPortId, newNodeTool);
            newNodeTool?.GraphTool.Refresh();
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private static void ConnectNodes(string fromPortId, TNodeTool newNodeTool)
        {
            if (string.IsNullOrEmpty(fromPortId)) return;
            if (newNodeTool == null) return;

            TPortTool fromPortTool = newNodeTool.GraphTool.PortTools[fromPortId];
            
            if (fromPortTool.Port.Mode == PortMode.Input)
            {
                if (fromPortTool.Port is not TInputPort fromInputPort) return;
                TPortTool[] outputPortTools = newNodeTool.OutputPortTools;
                
                foreach (TPortTool outputPortTool in outputPortTools)
                {
                    if (outputPortTool.Port is not TOutputPort outputPort) continue;
                    if (!fromInputPort.CanConnectFrom(outputPort)) continue;
                    
                    newNodeTool.GraphTool.ConnectPortTools(fromPortTool, outputPortTool);
                    return;
                }
            }
            else
            {
                if (fromPortTool.Port is not TOutputPort fromOutputPort) return;
                TPortTool[] inputPortTools = newNodeTool.InputPortTools;
                
                foreach (TPortTool inputPortTool in inputPortTools)
                {
                    if (inputPortTool.Port is not TInputPort inputPort) continue;
                    if (!inputPort.CanConnectFrom(fromOutputPort)) continue;
                    
                    newNodeTool.GraphTool.ConnectPortTools(fromPortTool, inputPortTool);
                    return;
                }
            }
        }
    }
}