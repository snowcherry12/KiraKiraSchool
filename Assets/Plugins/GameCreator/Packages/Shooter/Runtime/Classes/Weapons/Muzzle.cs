using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Serializable]
    public class Muzzle
    {
        private static readonly Color COLOR_GIZMOS = new Color(0f, 1f, 1f, 1f);
        private const float INDICATOR_GIZMOS = 0.03f;
        private const float RADIUS_GIZMOS = 0.01f;
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private Vector3 m_Position = Vector3.forward * 0.25f;
        [SerializeField] private Vector3 m_Rotation = Vector3.zero;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public Vector3 LocalPosition => this.m_Position;
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public Vector3 GetPosition(Args args)
        {
            return args.Target != null
                ? args.Target.transform.TransformPoint(this.m_Position)
                : default;
        }

        public Quaternion GetRotation(Args args)
        {
            return args.Target != null
                ? TransformUtils.TransformRotation(
                    Quaternion.Euler(this.m_Rotation),
                    args.Target.transform.position,
                    args.Target.transform.rotation,
                    args.Target.transform.lossyScale
                ) : default;
        }
        
        // GIZMOS: --------------------------------------------------------------------------------
        
        public void StageGizmos(StagingGizmos stagingGizmos)
        {
            Gizmos.color = COLOR_GIZMOS;
            Matrix4x4 restoreMatrix = Gizmos.matrix;

            Matrix4x4 rotationMatrix = Matrix4x4.TRS(
                stagingGizmos.transform.TransformPoint(this.m_Position),
                stagingGizmos.transform.rotation * Quaternion.Euler(this.m_Rotation),
                Vector3.one
            );
                
            Gizmos.matrix = rotationMatrix;
            
            GizmosExtension.Octahedron(Vector3.zero, Quaternion.identity, RADIUS_GIZMOS, 4);
            GizmosExtension.Circle(Vector3.zero, INDICATOR_GIZMOS, Vector3.forward);
            Gizmos.DrawLine(Vector3.zero, Vector3.forward * 0.5f);

            Gizmos.matrix = restoreMatrix;
        }
    }
}