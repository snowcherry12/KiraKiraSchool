using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [AddComponentMenu("")]
    public class HearManager : Singleton<HearManager>
    {
        private struct GizmoNoise
        {
            public Vector3 position;
            public float radius;
            public float startTime;
            public float duration;
        }
        
        private const float GIZMO_DURATION = 1.0f;
        private static readonly Color GIZMO_NOISE_COLOR = Color.yellow;
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private float m_GlobalDin;

        [NonSerialized] private Dictionary<int, Din> m_LocalDins;

        [NonSerialized] private List<GizmoNoise> m_GizmoNoises;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public float GlobalDin
        {
            get => this.m_GlobalDin;
            set
            {
                this.m_GlobalDin = value;
                this.EventChangeGlobalDin?.Invoke();
            }
        }
        
        // EVENTS: --------------------------------------------------------------------------------

        public event Action EventChangeGlobalDin; 
        
        // ON CREATE: -----------------------------------------------------------------------------
        
        protected override void OnCreate()
        {
            base.OnCreate();
            
            this.m_GlobalDin = 0f;
            this.m_LocalDins = new Dictionary<int, Din>();
            this.m_GizmoNoises = new List<GizmoNoise>();
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void InsertLocalDin(Din din)
        {
            if (din == null) return;
            this.m_LocalDins.TryAdd(din.GetInstanceID(), din);
        }
        
        public void RemoveLocalDin(Din din)
        {
            if (din == null) return;
            this.m_LocalDins.Remove(din.GetInstanceID());
        }
        
        public float DinFor(Perception perception)
        {
            float din = this.m_GlobalDin;
            foreach (KeyValuePair<int, Din> entry in this.m_LocalDins)
            {
                float value = entry.Value.GetValue(perception);
                din = Math.Max(din, value);
            }

            return din;
        }

        public void AddGizmoNoise(Vector3 point, float radius, float duration = GIZMO_DURATION)
        {
            #if UNITY_EDITOR
            
            GizmoNoise gizmoNoise = new GizmoNoise
            {
                position = point,
                radius = radius,
                startTime = Time.unscaledTime,
                duration = duration
            };
            
            this.m_GizmoNoises.Add(gizmoNoise);
            
            #endif
        }
        
        // GIZMOS: --------------------------------------------------------------------------------
        
        private void OnDrawGizmos()
        {
            #if UNITY_EDITOR

            for (int i = this.m_GizmoNoises.Count - 1; i >= 0; --i)
            {
                GizmoNoise noise = this.m_GizmoNoises[i];
                float elapsedTime = Time.unscaledTime - noise.startTime;
                
                float t = noise.duration > float.Epsilon
                    ? Mathf.Clamp01(elapsedTime / noise.duration)
                    : 0f;
                
                Gizmos.color = new Color(
                    GIZMO_NOISE_COLOR.r,
                    GIZMO_NOISE_COLOR.g,
                    GIZMO_NOISE_COLOR.b, 
                    Mathf.Lerp(0.25f, 0f, t)
                );
                
                GizmosExtension.Octahedron(noise.position, Quaternion.identity, noise.radius);
                
                if (elapsedTime <= noise.duration) continue;
                this.m_GizmoNoises.RemoveAt(i);
            }
            
            #endif
        }
    }
}