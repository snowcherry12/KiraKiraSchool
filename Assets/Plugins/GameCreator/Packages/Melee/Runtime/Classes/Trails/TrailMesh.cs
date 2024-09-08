using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    internal class TrailMesh
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private readonly List<Vector3> m_Vertices;
        [NonSerialized] private readonly List<Vector2> m_Textures;
        [NonSerialized] private readonly List<int> m_Triangles;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        [field: NonSerialized] private Trail Trail { get; }
        [field: NonSerialized] public Mesh Mesh { get; }

        // INITIALIZERS: --------------------------------------------------------------------------

        public TrailMesh(Trail trail)
        {
            this.Trail = trail;

            this.Mesh = new Mesh();
            this.Mesh.MarkDynamic();

            this.m_Vertices = new List<Vector3>(Trail.DEFAULT_QUADS * 2);
            this.m_Textures = new List<Vector2>(Trail.DEFAULT_QUADS * 2);
            this.m_Triangles = new List<int>(Trail.DEFAULT_QUADS * 2 * 3);
        }

        public void OnEnable()
        {
            
        }
        
        public void OnDisable()
        {
            
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Start()
        {
            
        }
        
        public void Stop()
        {
            
        }

        public void Update()
        {
            this.UpdateVertices();
            this.UpdateTextures();
            this.UpdateTriangles();
            
            this.UpdateMesh();
        }
        
        // UPDATE METHODS: ------------------------------------------------------------------------

        private void UpdateVertices()
        {
            this.m_Vertices.Clear();
            
            foreach (Segment segment in this.Trail.Segments)
            {
                this.m_Vertices.Add(segment.PointA);
                this.m_Vertices.Add(segment.PointB);
            }
        }

        private void UpdateTextures()
        {
            this.m_Textures.Clear();
            int verticesCount = this.m_Vertices.Count;
            
            for (int i = 0; i < verticesCount; i += 2)
            {
                float denominator = Math.Max(verticesCount - 2f, 0f);
                float offset = i / denominator;

                this.m_Textures.Add(new Vector2(offset, 1f));
                this.m_Textures.Add(new Vector2(offset, 0f));
            }
        }
        
        private void UpdateTriangles()
        {
            this.m_Triangles.Clear();
            
            for (int i = 0; i < this.m_Vertices.Count - 2; i += 2)
            {
                this.m_Triangles.Add(i + 0);
                this.m_Triangles.Add(i + 1);
                this.m_Triangles.Add(i + 3);
                
                this.m_Triangles.Add(i + 0);
                this.m_Triangles.Add(i + 3);
                this.m_Triangles.Add(i + 2);
            }
        }
        
        private void UpdateMesh()
        {
            this.Mesh.Clear();
            
            this.Mesh.SetVertices(this.m_Vertices);
            this.Mesh.SetUVs(0, this.m_Textures);
            this.Mesh.SetTriangles(this.m_Triangles, 0);

            this.Mesh.RecalculateNormals();
        }
    }
}