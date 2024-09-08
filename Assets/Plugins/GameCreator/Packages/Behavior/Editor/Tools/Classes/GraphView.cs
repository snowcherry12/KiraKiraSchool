using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    internal class GraphView : VisualElement
    {
        private const string NAME_CONTENT = "GC-Graph-Viewport-Content";

        public const string KEY_VIEW_POSITION = "gc-behavior:graph:position-{0}";
        public const string KEY_VIEW_SCALE = "gc-behavior:graph:scale-{0}";
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        [NonSerialized] private readonly TGraphTool m_GraphTool;
        [NonSerialized] private readonly VisualElement m_Content;

        // PROPERTIES: ----------------------------------------------------------------------------

        public Vector2 Position
        {
            get => this.m_Content.transform.position;
            set
            {
                this.m_Content.transform.position = value;

                string key = string.Format(KEY_VIEW_POSITION, this.m_GraphTool.InstanceId);
                SessionState.SetVector3(key, value);
            }
        }

        public float Scale
        {
            get => this.m_Content.transform.scale.x;
            set
            {
                float valueConstrained = Math.Clamp(
                    value,
                    ManipulatorGraphZoom.ZOOM_MIN,
                    ManipulatorGraphZoom.ZOOM_MAX
                );

                Vector3 scale = Vector3.one * valueConstrained;
                this.m_Content.transform.scale = scale;
                
                string key = string.Format(KEY_VIEW_SCALE, this.m_GraphTool.InstanceId);
                SessionState.SetFloat(key, valueConstrained);
            }
        }

        public Matrix4x4 ContentMatrix => this.m_Content.transform.matrix;
        public Rect ContentRect => this.m_Content.layout;

        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public GraphView(TGraphTool graphTool)
        {
            this.m_GraphTool = graphTool;
            this.pickingMode = PickingMode.Ignore;

            this.m_Content = new VisualElement
            {
                name = NAME_CONTENT,
                pickingMode = PickingMode.Ignore,
                usageHints = UsageHints.GroupTransform,
                style =
                {
                    transformOrigin = new TransformOrigin(
                        new Length(50, LengthUnit.Percent), 
                        new Length(50, LengthUnit.Percent)
                    )
                }
            };
            
            this.Add(this.m_Content);
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public Vector2 ChangeToContentCoordinatesFrom(VisualElement from, Vector2 point)
        {
            return from.ChangeCoordinatesTo(this.m_Content, point);
        }

        public Vector2 ChangeContentCoordinatesTo(VisualElement to, Vector2 point)
        {
            return this.m_Content.ChangeCoordinatesTo(to, point);
        }

        public void AddNode<T>(T node) where T : TNodeTool
        {
            this.m_Content.Add(node);
        }
        
        public void RemoveNode<T>(T node) where T : TNodeTool
        {
            this.m_Content.Remove(node);
        }

        public void ClearContent()
        {
            this.m_Content.Clear();
        }
    }
}