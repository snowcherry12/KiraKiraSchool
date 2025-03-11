using System;
using GameCreator.Runtime.Common;
using UnityEngine;
using UnityEngine.Rendering;

namespace GameCreator.Runtime.Shooter
{
    [AddComponentMenu("")]
    [Icon(EditorPaths.PACKAGES + "Shooter/Editor/Gizmos/GizmoLaser.png")]
    
    [RequireComponent(typeof(LineRenderer))]
    [DefaultExecutionOrder(ApplicationManager.EXECUTION_ORDER_DEFAULT)]
    
    [Serializable]
    public class Laser : MonoBehaviour
    {
        private const int NUM_CORNER_VERTICES = 0;
        private const int NUM_CAP_VERTICES = 3;
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private LineRenderer m_LineRenderer;
        [NonSerialized] private GameObject m_Dot;
        
        [NonSerialized] private Vector3 m_PointSource;
        [NonSerialized] private Vector3 m_PointTarget;
        
        // INITIALIZERS: --------------------------------------------------------------------------

        public static Laser Create(GameObject prefabDot)
        {
            GameObject container = new GameObject("Laser", typeof(Laser))
            {
                hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector
            };
            
            Laser laser = container.GetComponent<Laser>();

            if (prefabDot != null)
            {
                bool wasPrefabActive = prefabDot.activeSelf;
                prefabDot.SetActive(false);
                
                GameObject dot = Instantiate(prefabDot, container.transform);
                laser.m_Dot = dot;
                
                prefabDot.SetActive(wasPrefabActive);
            }

            return laser;
        }
        
        public void Dispose()
        {
            Destroy(this.gameObject);
        }
        
        private void Awake()
        {
            this.m_LineRenderer = this.GetComponent<LineRenderer>();
            this.m_LineRenderer.useWorldSpace = true;
            
            this.m_LineRenderer.receiveShadows = false;
            this.m_LineRenderer.staticShadowCaster = false;
            this.m_LineRenderer.shadowCastingMode = ShadowCastingMode.Off;
            this.m_LineRenderer.positionCount = 2;
        }

        private void Update()
        {
            this.m_LineRenderer.enabled = false;
            if (this.m_Dot != null) this.m_Dot.SetActive(false);
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public void Set(
            Vector3 pointSource,
            Vector3 pointTarget,
            Vector3 pointNormal,
            Material material,
            Color color,
            float width,
            LineTextureMode textureMode,
            LineAlignment textureAlign)
        {
            this.m_LineRenderer.enabled = true;
            
            this.m_PointSource = pointSource;
            this.m_PointTarget = pointTarget;
            
            this.m_LineRenderer.material = material;
            this.m_LineRenderer.startColor = color;
            this.m_LineRenderer.endColor = color;

            this.m_LineRenderer.numCornerVertices = NUM_CORNER_VERTICES;
            this.m_LineRenderer.numCapVertices = NUM_CAP_VERTICES;
            
            this.m_LineRenderer.startWidth = width;
            this.m_LineRenderer.endWidth = width;
            
            this.m_LineRenderer.textureMode = textureMode;
            this.m_LineRenderer.alignment = textureAlign;
            
            this.m_LineRenderer.SetPosition(0, this.m_PointSource);
            this.m_LineRenderer.SetPosition(1, this.m_PointTarget);

            if (this.m_Dot != null)
            {
                if (pointNormal != Vector3.zero)
                {
                    this.m_Dot.SetActive(true);
                    this.m_Dot.transform.rotation = Quaternion.FromToRotation(Vector3.up, pointNormal);
                }
                else
                {
                    this.m_Dot.SetActive(false);
                }
                
                this.m_Dot.transform.position = this.m_PointTarget;
            }
        }
    }
}