using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace GameCreator.Runtime.Shooter
{
    [AddComponentMenu("")]
    [Icon(EditorPaths.PACKAGES + "Shooter/Editor/Gizmos/GizmoTrajectory.png")]
    
    [RequireComponent(typeof(LineRenderer))]
    [DefaultExecutionOrder(ApplicationManager.EXECUTION_ORDER_DEFAULT)]
    
    [Serializable]
    public class Trajectory : MonoBehaviour
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private LineRenderer m_LineRenderer;
        [NonSerialized] private GameObject m_Dot;

        [NonSerialized] private NativeArray<Vector3> m_Points;

        // INITIALIZERS: --------------------------------------------------------------------------

        public static Trajectory Create(int maxLength, GameObject prefabDot)
        {
            GameObject container = new GameObject("Trajectory", typeof(Trajectory))
            {
                hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector
            };
            
            Trajectory trajectory = container.GetComponent<Trajectory>();
            
            trajectory.m_Points = new NativeArray<Vector3>(
                maxLength,
                Allocator.Persistent,
                NativeArrayOptions.UninitializedMemory
            );
            
            if (prefabDot != null)
            {
                bool wasPrefabActive = prefabDot.activeSelf;
                prefabDot.SetActive(false);
                
                GameObject dot = Instantiate(prefabDot, container.transform);
                trajectory.m_Dot = dot;
                
                prefabDot.SetActive(wasPrefabActive);
            }

            return trajectory;
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
        }

        private void OnDestroy()
        {
            this.m_Points.Dispose();
        }

        // UPDATE METHODS: ------------------------------------------------------------------------

        private void Update()
        {
            this.m_LineRenderer.positionCount = 0;
            if (this.m_Dot != null) this.m_Dot.SetActive(false);
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Set(
            List<Vector3> points,
            Material material,
            Color color,
            float width,
            LineTextureMode textureMode,
            LineAlignment textureAlign,
            int cornerVertices,
            int capVertices)
        {
            int pointsCount = points.Count;
            
            if (this.m_Points.Length < pointsCount)
            {
                if (this.m_Points.IsCreated)
                {
                    this.m_Points.Dispose();
                }
                
                this.m_Points = new NativeArray<Vector3>(
                    pointsCount,
                    Allocator.Persistent,
                    NativeArrayOptions.UninitializedMemory
                );
            }
            
            for (int i = 0; i < pointsCount; ++i)
            {
                this.m_Points[i] = points[i];
            }

            NativeSlice<Vector3> slice = this.m_Points.Slice(0, pointsCount);

            this.m_LineRenderer.enabled = width > float.Epsilon;
            this.m_LineRenderer.material = material;
            this.m_LineRenderer.startColor = color;
            this.m_LineRenderer.endColor = color;

            this.m_LineRenderer.numCornerVertices = cornerVertices;
            this.m_LineRenderer.numCapVertices = capVertices;
            
            this.m_LineRenderer.startWidth = width;
            this.m_LineRenderer.endWidth = width;
            
            this.m_LineRenderer.textureMode = textureMode;
            this.m_LineRenderer.alignment = textureAlign;
            
            this.m_LineRenderer.positionCount = slice.Length;
            this.m_LineRenderer.SetPositions(slice);
            
            if (this.m_Dot != null && pointsCount > 1)
            {
                Vector3 pointTarget = slice[^1];
                Vector3 pointNormal = pointTarget - slice[0];
                
                if (pointNormal != Vector3.zero)
                {
                    this.m_Dot.SetActive(true);
                    this.m_Dot.transform.rotation = Quaternion.FromToRotation(Vector3.up, pointNormal);
                }
                else
                {
                    this.m_Dot.SetActive(false);
                }
                
                this.m_Dot.transform.position = pointTarget;
            }
        }
    }
}