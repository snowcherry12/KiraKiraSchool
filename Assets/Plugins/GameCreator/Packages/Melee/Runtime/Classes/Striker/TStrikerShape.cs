using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Title("Shape")]
    
    [Serializable]
    public abstract class TStrikerShape : IStrikerShape
    {
        protected static readonly Collider[] HITS = new Collider[25];
        
        private static readonly Color GIZMOS_COLOR_NORMAL = new Color(1f, 1f, 0f, 0.1f);
        private static readonly Color GIZMOS_COLOR_COLLECT  = new Color(0f, 1f, 0f, 0.5f);
        private static readonly Color GIZMOS_COLOR_PREDICTED  = new Color(0f, 0f, 1f, 0.2f);

        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private Vector3 m_CurrentPosition;
        [NonSerialized] private Quaternion m_CurrentRotation;
        
        [NonSerialized] private Vector3 m_PreviousPosition;
        [NonSerialized] private Quaternion m_PreviousRotation;

        [NonSerialized] private int m_Predictions;
        [NonSerialized] private bool m_IsCollecting;
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected abstract Vector3 Position { get; }
        protected abstract Vector3 Rotation { get; }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Start(Transform transform)
        {
            this.m_CurrentPosition = transform.TransformPoint(this.Position);
            this.m_CurrentRotation = transform.rotation * Quaternion.Euler(this.Rotation);
            
            this.m_PreviousPosition = this.m_CurrentPosition;
            this.m_PreviousRotation = this.m_CurrentRotation;

            this.m_IsCollecting = true;
        }

        public void Stop(Transform transform)
        {
            this.m_IsCollecting = false;
        }
        
        public List<StrikeOutput> Collect(Transform transform, LayerMask layerMask, int prediction)
        {
            this.m_Predictions = prediction;
            
            this.m_PreviousPosition = this.m_CurrentPosition;
            this.m_PreviousRotation = this.m_CurrentRotation;
            
            this.m_CurrentPosition = transform.TransformPoint(this.Position);
            this.m_CurrentRotation = transform.rotation * Quaternion.Euler(this.Rotation);
            
            Vector3 prevPredictedPosition = Vector3.zero;
            Quaternion prevPredictedRotation = Quaternion.identity;

            List<StrikeOutput> candidates = new List<StrikeOutput>();
            
            for (int i = 0; i <= this.m_Predictions; ++i)
            {
                float t = i / (float) this.m_Predictions;
                
                Vector3 predictedPosition = this.GetPosition(t);
                Quaternion predictedRotation = this.GetRotation(t);

                if (i != 0)
                {
                    bool samePosition = predictedPosition == prevPredictedPosition;
                    bool sameRotation = predictedRotation == prevPredictedRotation;

                    if (samePosition && sameRotation) return candidates;
                }

                prevPredictedPosition = predictedPosition;
                prevPredictedRotation = predictedRotation;

                int numHits = this.Cast(predictedPosition, predictedRotation, layerMask);
                for (int hitIndex = 0; hitIndex < numHits; ++hitIndex)
                {
                    GameObject newCandidate = HITS[hitIndex].gameObject;
                    Vector3 point = this.GetPoint(
                        HITS[hitIndex].gameObject,
                        predictedPosition,
                        predictedRotation
                    );
                    
                    if (Contains(candidates, newCandidate)) continue;
                    candidates.Add(new StrikeOutput(newCandidate, point));
                }
            }

            return candidates;
        }
        
        // ABSTRACT METHODS: ----------------------------------------------------------------------

        protected abstract int Cast(Vector3 position, Quaternion rotation, LayerMask layerMask);

        protected abstract Vector3 GetPoint(GameObject hit, Vector3 position, Quaternion rotation);

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private Vector3 GetPosition(float t)
        {
            if (t <= 0f) return this.m_PreviousPosition;
            return t < 1f
                ? Vector3.Lerp(this.m_PreviousPosition, this.m_CurrentPosition, t)
                : this.m_CurrentPosition;
        }
        
        private Quaternion GetRotation(float t)
        {
            if (t <= 0f) return this.m_PreviousRotation;

            return t < 1f
                ? Quaternion.Lerp(this.m_PreviousRotation, this.m_CurrentRotation, t)
                : this.m_CurrentRotation;
        }

        private bool Contains(List<StrikeOutput> candidates, GameObject gameObject)
        {
            foreach (StrikeOutput candidate in candidates)
            {
                if (candidate.GameObject == gameObject) return true;
            }

            return false;
        }
        
        // GIZMOS: --------------------------------------------------------------------------------

        public void OnDrawGizmos(Transform transform)
        {
            if (transform == null) return;

            if (Application.isPlaying)
            {
                if (this.m_IsCollecting)
                {
                    Gizmos.color = GIZMOS_COLOR_PREDICTED;
                    
                    Vector3 prevPredictedPosition = Vector3.zero;
                    Quaternion prevPredictedRotation = Quaternion.identity;

                    for (int i = 0; i < this.m_Predictions; ++i)
                    {
                        float t = i / (float) this.m_Predictions;

                        Vector3 predictedPosition = this.GetPosition(t);
                        Quaternion predictedRotation = this.GetRotation(t);

                        if (i != 0)
                        {
                            bool samePosition = predictedPosition == prevPredictedPosition;
                            bool sameRotation = predictedRotation == prevPredictedRotation;

                            if (samePosition && sameRotation) continue;
                        }
                        
                        prevPredictedPosition = predictedPosition;
                        prevPredictedRotation = predictedRotation;
                        
                        this.DrawGizmos(predictedPosition, predictedRotation);
                    }
                    
                    Gizmos.color = GIZMOS_COLOR_COLLECT;
                    
                    Vector3 position = this.GetPosition(2.0f);
                    Quaternion rotation = this.GetRotation(2.0f);
                    this.DrawGizmos(position, rotation);
                }
                else
                {
                    Gizmos.color = GIZMOS_COLOR_NORMAL;
                    
                    this.DrawGizmos(
                        transform.TransformPoint(this.Position),
                        transform.rotation * Quaternion.Euler(this.Rotation)
                    );
                }
            }
            else
            {
                Gizmos.color = GIZMOS_COLOR_COLLECT;

                this.DrawGizmos(
                    transform.TransformPoint(this.Position),
                    transform.rotation * Quaternion.Euler(this.Rotation)
                );
            }
        }

        protected abstract void DrawGizmos(Vector3 position, Quaternion rotation);
    }
}