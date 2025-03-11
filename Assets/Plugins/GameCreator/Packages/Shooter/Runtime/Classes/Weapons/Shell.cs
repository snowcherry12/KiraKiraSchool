using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Serializable]
    public class Shell
    {
        private static readonly Color COLOR_GIZMOS = new Color(1f, 1f, 0f, 1f);
        private const float RADIUS_GIZMOS = 0.01f;
        private const float WIDTH_GIZMOS = 0.03f;
        private const float HEIGHT_GIZMOS = 0.03f;

        private const int LAYER_IGNORE_RAYCAST = 2;

        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private Vector3 m_Position = Vector3.right * 0.25f;
        [SerializeField] private Vector3 m_Rotation = new Vector3(0f, 90f, 0f);

        [SerializeField] private PropertyGetInstantiate m_Prefab = new PropertyGetInstantiate();
        [SerializeField] private PropertyGetDecimal m_Force = GetDecimalDecimal.Create(5f);
        [SerializeField] private PropertyGetDecimal m_Torque = GetDecimalDecimal.Create(180f);
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public Vector3 Position => this.m_Position;
        public Quaternion Rotation => Quaternion.Euler(this.m_Rotation);
        
        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public void Eject(Args args)
        {
            Vector3 position = args.Target.transform.TransformPoint(this.m_Position);
            Quaternion rotation = args.Target.transform.rotation * Quaternion.Euler(this.m_Rotation);
            
            GameObject shell = this.m_Prefab.Get(args, position, rotation);
            if (shell == null) return;
            
            shell.layer = LAYER_IGNORE_RAYCAST;
            
            Rigidbody rigidbody = shell.Get<Rigidbody>();
            if (rigidbody == null) return;

            float force = (float) this.m_Force.Get(args);
            float torque = (float) this.m_Torque.Get(args);
            
            rigidbody.linearVelocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            
            rigidbody.AddForce(
                rotation * Vector3.forward * force,
                ForceMode.VelocityChange
            );
            
            rigidbody.AddTorque(
                UnityEngine.Random.onUnitSphere * torque,
                ForceMode.VelocityChange
            );
        }
        
        // GIZMOS: --------------------------------------------------------------------------------
        
        public void StageGizmos(StagingGizmos stagingGizmos)
        {
            Gizmos.color = COLOR_GIZMOS;
            Matrix4x4 restoreMatrix = Gizmos.matrix;

            Matrix4x4 rotationMatrix = Matrix4x4.TRS(
                stagingGizmos.transform.TransformPoint(this.Position),
                stagingGizmos.transform.rotation * this.Rotation,
                Vector3.one
            );
            
            Gizmos.matrix = rotationMatrix;
            
            GizmosExtension.Octahedron(Vector3.zero, Quaternion.identity, RADIUS_GIZMOS, 4);
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(WIDTH_GIZMOS, HEIGHT_GIZMOS, 0f));
            Gizmos.DrawLine(Vector3.zero, Vector3.forward * 0.25f);
            
            Gizmos.matrix = restoreMatrix;
        }
    }
}