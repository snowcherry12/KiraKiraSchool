using System;
using System.Collections.Generic;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;
using UnityEngine.Rendering;

namespace GameCreator.Runtime.Melee
{
    [Serializable]
    public class Trail
    {
        private enum State
        {
            Inactive,
            Running,
            Fading
        }

        public const int DEFAULT_QUADS = 32;
        public const float DEFAULT_LENGTH = 2f;

        private const float MIN_LENGTH = 0.001f;
        private const float FADE_TIME = 0.15f;
        
        private static readonly int PROPERTY_COLOR = Shader.PropertyToID("_Color");

        private static readonly Color GIZMOS_TRAIL_COLOR = new Color(0f, 1f, 0);
        private static readonly Color GIZMOS_EDGE_COLOR = new Color(1f, 1f, 0);
        private const float GIZMOS_EDGE_RADIUS = 0.02f;
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private Vector3 m_PointA = Vector3.right * 0.25f;
        [SerializeField] private Vector3 m_PointB = Vector3.left * 0.25f;
        
        [SerializeField] private int m_Quads = DEFAULT_QUADS;
        [SerializeField] private float m_Length = DEFAULT_LENGTH;
        [SerializeField] private Material m_Material;

        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private Striker m_Striker;
        [NonSerialized] private Character m_Character;
        [NonSerialized] private Skill m_Skill;

        [NonSerialized] private State m_State = State.Inactive;
        [NonSerialized] private RenderParams m_Params;
        
        [NonSerialized] private float m_FadeTime;
        [NonSerialized] private float m_FadeDuration;

        [NonSerialized] private TrailArc m_TrailArc;
        [NonSerialized] private TrailMesh m_TrailMesh;

        [NonSerialized] private MaterialPropertyBlock m_PropertyBlock;

        // PROPERTIES: ----------------------------------------------------------------------------

        public Vector3 EdgeA => this.m_Striker.transform.TransformPoint(this.m_PointA);
        public Vector3 EdgeB => this.m_Striker.transform.TransformPoint(this.m_PointB);

        internal bool IsActive => this.m_Skill == null || this.m_Skill.Trail.IsActive;

        internal int Quads
        {
            get
            {
                int quads = this.m_Skill != null && this.m_Skill.Trail.HasQuads
                    ? this.m_Skill.Trail.Quads
                    : this.m_Quads;

                return Math.Max(0, quads);
            }
        }

        internal float Length
        {
            get
            {
                float length = this.m_Skill != null && this.m_Skill.Trail.HasLength
                    ? this.m_Skill.Trail.Length
                    : this.m_Length;

                return Math.Max(MIN_LENGTH, length);
            }
        }

        internal Material Material => this.m_Skill != null && this.m_Skill.Trail.HasMaterial
            ? this.m_Skill.Trail.Material
            : this.m_Material;

        internal float Visibility => this.m_State switch
        {
            State.Inactive => 0f,
            State.Running => 1f,
            State.Fading => Mathf.Clamp01(1f - (this.Time - this.m_FadeTime) / this.m_FadeDuration),
            _ => throw new ArgumentOutOfRangeException()
        };

        private float Time => this.m_Character.Time.Time;

        internal List<Segment> Segments => this.m_TrailArc.Output;

        // INITIALIZE METHODS: --------------------------------------------------------------------

        public void Awake(Striker striker)
        {
            this.m_Striker = striker;
            this.m_Params = new RenderParams
            {
                renderingLayerMask = GraphicsSettings.defaultRenderingLayerMask
            };
            
            this.m_TrailArc = new TrailArc(this);
            this.m_TrailMesh = new TrailMesh(this);

            this.m_PropertyBlock = new MaterialPropertyBlock();
        }

        public void OnEnable()
        {
            this.m_State = State.Inactive;

            this.m_TrailArc.OnEnable();
            this.m_TrailMesh.OnEnable();
        }

        public void OnDisable()
        {
            this.m_TrailArc.OnDisable();
            this.m_TrailMesh.OnDisable();
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Start(Character character, Skill skill)
        {
            this.m_Character = character;
            this.m_Skill = skill;

            this.m_Params.material = this.Material;
            this.m_Params.matProps = this.m_PropertyBlock;
            
            this.m_TrailArc.Start();
            this.m_TrailMesh.Start();
            
            if (!this.IsActive) return;
            this.m_State = State.Running;
        }

        public void Stop()
        {
            if (this.m_State != State.Running) return;

            this.m_FadeDuration = FADE_TIME;
            this.m_FadeTime = this.Time;
            
            this.m_State = State.Fading;
            
            this.m_TrailArc.Stop();
            this.m_TrailMesh.Stop();
        }
        
        // UPDATE METHODS: ------------------------------------------------------------------------

        public void Update()
        {
            if (this.m_State == State.Inactive) return;

            this.m_TrailArc.Update();
            this.m_TrailMesh.Update();

            float alphaRatio = 1f;
            if (this.m_State == State.Fading)
            {
                float elapsedFadeTime = this.Time - this.m_FadeTime;
                alphaRatio = 1f - elapsedFadeTime / this.m_FadeDuration;
                
                if (elapsedFadeTime >= this.m_FadeDuration)
                {
                    this.m_State = State.Inactive;
                }
            }

            if (this.m_Params.material != null)
            {
                this.m_PropertyBlock.SetColor(
                    PROPERTY_COLOR,
                    new Color(
                        this.m_Params.material.color.r, 
                        this.m_Params.material.color.g,
                        this.m_Params.material.color.b, 
                        this.m_Params.material.color.a * alphaRatio
                    )
                );
            }

            Graphics.RenderMesh(
                this.m_Params, 
                this.m_TrailMesh.Mesh, 0,
                Matrix4x4.identity
            );
        }

        // GIZMOS: --------------------------------------------------------------------------------

        public void OnDrawGizmos(Striker striker)
        {
            Transform transform = striker.transform;
            
            Vector3 pointA = transform.TransformPoint(this.m_PointA);
            Vector3 pointB = transform.TransformPoint(this.m_PointB);

            Gizmos.color = GIZMOS_EDGE_COLOR;
            Gizmos.DrawLine(pointA, pointB);
            
            GizmosExtension.Octahedron(pointA, Quaternion.identity, GIZMOS_EDGE_RADIUS, 3);
            GizmosExtension.Octahedron(pointB, Quaternion.identity, GIZMOS_EDGE_RADIUS, 3);
            
            if (!Application.isPlaying) return;
            if (this.m_TrailArc?.Output?.Count == 0) return;
            
            Gizmos.color = GIZMOS_TRAIL_COLOR;
            
            Segment previousSegment = Segment.NONE;
            bool isActive = this.m_State != State.Inactive;
            
            for (int i = 0; i < this.m_TrailArc?.Output?.Count && isActive; i++)
            {
                Segment segment = this.m_TrailArc.Output[i];
                
                if (i != 0)
                {
                    Gizmos.DrawLine(previousSegment.PointA, segment.PointA);
                    Gizmos.DrawLine(previousSegment.PointB, segment.PointB);
                }
                
                Gizmos.DrawLine(segment.PointA, segment.PointB);
                previousSegment = segment;
            }
        }
    }
}