using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Title("Sphere")]
    [Category("Sphere")]
    
    [Image(typeof(IconSphereSolid), ColorTheme.Type.Yellow)]
    [Description("A spherical shape that detects any overlapping game objects with a collider")]
    
    [Serializable]
    public class StrikerSphere : TStrikerShape
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private Vector3 m_Position = Vector3.zero;
        [SerializeField] private float m_Radius = 0.5f;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        protected override Vector3 Position => this.m_Position;
        protected override Vector3 Rotation => Vector3.zero;

        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected override int Cast(Vector3 position, Quaternion rotation, LayerMask layerMask)
        {
            int numHits = Physics.OverlapSphereNonAlloc(
                position, this.m_Radius, HITS, 
                layerMask, QueryTriggerInteraction.Ignore
            );

            return numHits;
        }

        protected override Vector3 GetPoint(GameObject hit, Vector3 position, Quaternion rotation)
        {
            return position;
        }

        // GIZMOS: --------------------------------------------------------------------------------

        protected override void DrawGizmos(Vector3 position, Quaternion rotation)
        {
            GizmosExtension.Octahedron(position, rotation, this.m_Radius, 4);
        }
    }
}