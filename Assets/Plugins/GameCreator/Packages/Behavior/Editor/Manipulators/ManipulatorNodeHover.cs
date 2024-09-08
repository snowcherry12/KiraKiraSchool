using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    internal class ManipulatorNodeHover : MouseManipulator
    {
        // REGISTERS: -----------------------------------------------------------------------------
        
        protected override void RegisterCallbacksOnTarget()
        {
            this.target.RegisterCallback<MouseEnterEvent>(this.OnMouseEnter);
            this.target.RegisterCallback<MouseLeaveEvent>(this.OnMouseExit);
            this.target.RegisterCallback<MouseCaptureOutEvent>(this.OnMouseCaptureOut);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            this.target.UnregisterCallback<MouseEnterEvent>(this.OnMouseEnter);
            this.target.UnregisterCallback<MouseLeaveEvent>(this.OnMouseExit);
            this.target.UnregisterCallback<MouseCaptureOutEvent>(this.OnMouseCaptureOut);
        }
        
        // CALLBACK METHODS: ----------------------------------------------------------------------
        
        private void OnMouseEnter(MouseEnterEvent eventMouseEnter)
        {
            if (this.target is not TNodeTool nodeTool) return;
            if (nodeTool.GraphTool.IsSelecting) return;
            
            nodeTool.OnHoverEnter();
        }
        
        private void OnMouseExit(MouseLeaveEvent eventMouseLeave)
        {
            if (this.target is not TNodeTool nodeTool) return;
            nodeTool.OnHoverExit();
        }
        
        private void OnMouseCaptureOut(MouseCaptureOutEvent eventMouseOut)
        {
            if (this.target is not TNodeTool nodeTool) return;
            nodeTool.OnHoverExit();
        }
    }
}
