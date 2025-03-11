using System;
using GameCreator.Runtime.Common;
using UnityEngine;
using UnityEngine.Rendering;

namespace GameCreator.Runtime.Shooter
{
    [AddComponentMenu("")]
    [Icon(EditorPaths.PACKAGES + "Shooter/Editor/Gizmos/GizmoTracer.png")]
    
    [RequireComponent(typeof(LineRenderer))]
    [DefaultExecutionOrder(ApplicationManager.EXECUTION_ORDER_DEFAULT)]
    
    [Serializable]
    public class Tracer : MonoBehaviour
    {
        private const int NUM_CORNER_VERTICES = 0;
        private const int NUM_CAP_VERTICES = 3;
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private LineRenderer m_LineRenderer;
        
        [NonSerialized] private Vector3 m_PointSource;
        [NonSerialized] private Vector3 m_PointTarget;
        
        [NonSerialized] private float m_Duration;
        [NonSerialized] private float m_StartTime;
        [NonSerialized] private float m_MaxDistance;
        
        // INITIALIZERS: --------------------------------------------------------------------------
        
        private void Awake()
        {
            this.m_LineRenderer = this.GetComponent<LineRenderer>();
            this.m_LineRenderer.useWorldSpace = true;

            this.m_LineRenderer.enabled = true;
            this.m_LineRenderer.receiveShadows = false;
            this.m_LineRenderer.staticShadowCaster = false;
            this.m_LineRenderer.shadowCastingMode = ShadowCastingMode.Off;
            this.m_LineRenderer.positionCount = 2;
        }

        private void Update()
        {
            if (this.m_Duration <= 0f) return;
            float t = (Time.time - this.m_StartTime) / this.m_Duration;

            Vector3 direction = this.m_PointTarget - this.m_PointSource;
            Vector3 horizonPoint = this.m_PointSource + direction.normalized * this.m_MaxDistance;
            
            Vector3 sourcePosition = Vector3.Lerp(
                this.m_PointSource,
                horizonPoint,
                t
            );

            float maxDistance = Vector3.Distance(this.m_PointSource, this.m_PointTarget);
            float currentDistance = Vector3.Distance(this.m_PointSource, sourcePosition);

            if (maxDistance <= currentDistance)
            {
                this.m_LineRenderer.enabled = false;
                return;
            }

            this.m_LineRenderer.SetPosition(0, sourcePosition);
            this.m_LineRenderer.SetPosition(1, this.m_PointTarget);
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public void OnShoot(
            Vector3 pointSource,
            Vector3 pointTarget,
            float duration,
            float maxDistance,
            Material material,
            Color color,
            float width,
            LineTextureMode textureMode,
            LineAlignment textureAlign)
        {
            this.m_PointSource = pointSource;
            this.m_PointTarget = pointTarget;
            
            this.m_Duration = duration;
            this.m_StartTime = Time.time;
            this.m_MaxDistance = maxDistance;
            
            this.m_LineRenderer.enabled = true;
            this.m_LineRenderer.material = material;
            this.m_LineRenderer.startColor = color;
            this.m_LineRenderer.endColor = color;

            this.m_LineRenderer.numCornerVertices = NUM_CORNER_VERTICES;
            this.m_LineRenderer.numCapVertices = NUM_CAP_VERTICES;
            
            this.m_LineRenderer.startWidth = 0f;
            this.m_LineRenderer.endWidth = width;
            
            this.m_LineRenderer.textureMode = textureMode;
            this.m_LineRenderer.alignment = textureAlign;
            
            this.m_LineRenderer.SetPosition(0, this.m_PointSource);
            this.m_LineRenderer.SetPosition(1, this.m_PointTarget);
        }
    }
}