using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [DefaultExecutionOrder(ApplicationManager.EXECUTION_ORDER_DEFAULT_LATER)]
    
    [AddComponentMenu("")]
    [Icon(EditorPaths.PACKAGES + "Shooter/Editor/Gizmos/GizmoWell.png")]
    
    public class Well : MonoBehaviour
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private Vector3 m_SourceLocalPosition;
        [NonSerialized] private Quaternion m_SourceLocalRotation;
        
        [NonSerialized] private Vector3 m_TargetLocalPosition;
        [NonSerialized] private Quaternion m_TargetLocalRotation;

        [NonSerialized] private TimeMode m_TimeMode;
        [NonSerialized] private float m_StartTime;
        [NonSerialized] private float m_Duration;
        
        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public void Run(Vector3 position, Quaternion rotation, TimeMode mode, float duration)
        {
            this.m_SourceLocalPosition = this.transform.localPosition;
            this.m_SourceLocalRotation = this.transform.localRotation;

            this.m_TargetLocalPosition = position;
            this.m_TargetLocalRotation = rotation;

            this.m_TimeMode = mode;
            this.m_StartTime = mode.Time;
            this.m_Duration = duration;
        }
        
        // CYCLE METHODS: -------------------------------------------------------------------------

        private void LateUpdate()
        {
            float t = this.m_Duration >= 0
                ? (this.m_TimeMode.Time - this.m_StartTime) / this.m_Duration
                : 1f;
            
            if (t > 1f + float.Epsilon) return;
            
            this.transform.SetLocalPositionAndRotation(
                Vector3.Lerp(this.m_SourceLocalPosition, this.m_TargetLocalPosition, t),
                Quaternion.Lerp(this.m_SourceLocalRotation, this.m_TargetLocalRotation, t)
            );
        }
    }
}