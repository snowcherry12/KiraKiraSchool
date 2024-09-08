using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    internal class ManipulatorGraphZoom : Manipulator
    {
        public const float ZOOM_MIN = 0.1f;
        public const float ZOOM_MAX = 4.0f;
        
        private const float ZOOM_STEP = 0.15f;
        private const float ZOOM_COEFFICIENT = 1f;

        // REGISTERS: -----------------------------------------------------------------------------
        
        protected override void RegisterCallbacksOnTarget()
        {
            this.target.RegisterCallback<WheelEvent>(this.OnZoom);
        }
        
        protected override void UnregisterCallbacksFromTarget()
        {
            this.target.UnregisterCallback<WheelEvent>(this.OnZoom);
        }
        
        // CALLBACK METHODS: ----------------------------------------------------------------------
        
        private void OnZoom(WheelEvent eventWheel)
        {
            if (this.target is not TGraphTool graphTool) return;

            Vector3 position = graphTool.View.Position;
            float scale = graphTool.View.Scale;
            
            Vector2 zoomCenter = graphTool.View.ChangeToContentCoordinatesFrom(target, eventWheel.localMousePosition);
            float x = zoomCenter.x + graphTool.View.ContentRect.x;
            float y = zoomCenter.y + graphTool.View.ContentRect.y;
            
            position += new Vector3(x, y, 0) * scale;
            scale = CalculateNewZoom(scale, -eventWheel.delta.y);

            position -= new Vector3(x, y, 0) * scale;
            position.x = GraphUtils.RoundToPixelGrid(position.x);
            position.y = GraphUtils.RoundToPixelGrid(position.y);

            graphTool.SetView(position, scale);
            eventWheel.StopPropagation();
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private static float CalculateNewZoom(float currentZoom, float wheelDelta)
        {
            currentZoom = Mathf.Clamp(currentZoom, ZOOM_MIN, ZOOM_MAX);

            if (Mathf.Approximately(wheelDelta, 0))
            {
                return currentZoom;
            }
            
            double a = Math.Log(ZOOM_COEFFICIENT, 1 + ZOOM_STEP);
            double b = ZOOM_COEFFICIENT - Math.Pow(1 + ZOOM_STEP, a);
            
            double minWheel = Math.Log(ZOOM_MIN - b, 1 + ZOOM_STEP) - a;
            double maxWheel = Math.Log(ZOOM_MAX - b, 1 + ZOOM_STEP) - a;
            double currentWheel = Math.Log(currentZoom - b, 1 + ZOOM_STEP) - a;
            
            wheelDelta = Math.Sign(wheelDelta);
            currentWheel += wheelDelta;
            
            if (currentWheel > maxWheel - 0.5)
            {
                return ZOOM_MAX;
            }
            if (currentWheel < minWheel + 0.5)
            {
                return ZOOM_MIN;
            }
            
            currentWheel = Math.Round(currentWheel);
            return (float) (Math.Pow(1 + ZOOM_STEP, currentWheel + a) + b);
        }
    }
}