using System;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Behavior;
using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    internal abstract class TPortTool : VisualElement
    {
        private const string NAME_ROOT = "GC-Graph-Port-Root";

        private const float HOVER_SATURATION = -0.25f;
        private const float HOVER_VALUE = 0.2f;

        public const string PROP_CONNECTIONS = "m_Connections";

        // MEMBERS: -------------------------------------------------------------------------------
        
        [NonSerialized] protected readonly Label m_Text;

        [NonSerialized] private readonly ManipulatorPortDrag m_ManipulatorPortDrag;
        [NonSerialized] private readonly ManipulatorPortHover m_ManipulatorPortHover;

        // PROPERTIES: ----------------------------------------------------------------------------

        [field: NonSerialized] public TNodeTool NodeTool { get; }
        public TGraphTool GraphTool => this.NodeTool.GraphTool;
        
        [field: NonSerialized] public SerializedProperty PropertyPort { get; set; }

        public TPort Port => this.PropertyPort.managedReferenceValue as TPort;

        public string PortId => this.PropertyPort
            .FindPropertyRelative("m_Id")
            .FindPropertyRelative(ConnectionDrawer.PROP_VALUE)
            .stringValue;

        public SerializedProperty Connections => this.PropertyPort.FindPropertyRelative(PROP_CONNECTIONS);

        public bool IsDragging => this.m_ManipulatorPortDrag.IsDragging;
        public TPortTool Candidate => this.m_ManipulatorPortDrag.Candidate;

        public Vector2 Direction => this.Port.Position switch
        {
            PortPosition.Top => Vector2.up,
            PortPosition.Right => Vector2.right,
            PortPosition.Bottom => Vector2.down,
            PortPosition.Left => Vector2.left,
            _ => throw new ArgumentOutOfRangeException()
        };
        
        public virtual Action<MouseUpEvent, string, Vector2> AutoCreateNode => null;

        public WireShape Shape => this.Port.WireShape;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public TPortTool(TNodeTool nodeTool, SerializedProperty propertyPort)
        {
            this.name = NAME_ROOT;
            
            this.NodeTool = nodeTool;
            this.PropertyPort = propertyPort;

            this.m_Text = new Label();
            this.Add(this.m_Text);

            this.m_ManipulatorPortDrag = new ManipulatorPortDrag(this);
            this.m_ManipulatorPortHover = new ManipulatorPortHover();

            this.AddManipulator(this.m_ManipulatorPortDrag);
            this.AddManipulator(this.m_ManipulatorPortHover);
            this.AddManipulator(new ContextualMenuManipulator(this.MenuContextual));
            
            this.Refresh();

            this.generateVisualContent += this.OnGenerateContent;
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Refresh()
        {
            string portText = this.NodeTool.GetPortText(this.PortId);
            
            this.m_Text.text = portText;
            this.m_Text.style.display = string.IsNullOrEmpty(portText)
                ? DisplayStyle.None
                : DisplayStyle.Flex;
            
            this.MarkDirtyRepaint();
        }
        
        public Vector2 GetPortPositionFor(VisualElement to)
        {
            Vector2 centerOffset = new Vector2(
                this.layout.width / 2f,
                this.layout.height / 2f
            );
            
            return this.ChangeCoordinatesTo(to, centerOffset);
        }

        public Vector2 GetMousePositionFor(VisualElement to)
        {
            return this.ChangeCoordinatesTo(to, this.m_ManipulatorPortDrag.LocalMousePosition);
        }
        
        public virtual void OnHoverEnter()
        {
            this.MarkDirtyRepaint();
        }
        
        public virtual void OnHoverExit()
        {
            this.MarkDirtyRepaint();
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private Color GetColor(TPort port)
        {
            if (!Application.isPlaying)
            {
                if (port.Mode == PortMode.Input) return GraphUtils.Blue;
                return this.Connections.arraySize == 0
                    ? GraphUtils.Gray
                    : GraphUtils.Blue;
            }
            
            IdString nodeId = new IdString(this.NodeTool.NodeId);
            Status status = TargetUtils.Get?.GetStatus(nodeId) ?? Status.Ready;
                
            return GraphUtils.GetColor(status, GraphUtils.Gray);
        }
        
        // VISUAL CONTENT: ------------------------------------------------------------------------
        
        private void MenuContextual(ContextualMenuPopulateEvent eventMenu)
        {
            TPort portInstance = this.Port;
            
            switch (portInstance.Mode)
            {
                case PortMode.Input:
                    this.NodeTool.GraphTool.RemovePortsTo(portInstance.Id.Value);
                    break;
                
                case PortMode.Output:
                    this.NodeTool.GraphTool.RemovePortsFrom(this);
                    break;
                
                default: throw new ArgumentOutOfRangeException();
            }
            
            this.NodeTool.GraphTool.Refresh();
            eventMenu.StopPropagation();
        }
        
        private void OnGenerateContent(MeshGenerationContext context)
        {
            Painter2D painter = context.painter2D;
            TPort port = this.Port;

            Color color = this.GetColor(port);
            
            if (this.m_ManipulatorPortHover.IsHovering)
            {
                Color.RGBToHSV(color, out float h, out float s, out float v);
                color = Color.HSVToRGB(h, s + HOVER_SATURATION, v + HOVER_VALUE);
            }
            
            painter.strokeColor = color;
            painter.fillColor = color;
            
            switch (port.Mode)
            {
                case PortMode.Input:
                    this.GenerateInput(painter, port as TInputPort);
                    break;
                
                case PortMode.Output:
                    this.GenerateOutput(painter, port as TOutputPort);
                    break;
                
                default: throw new ArgumentOutOfRangeException();
            }
        }
        
        // ABSTRACT METHODS: ----------------------------------------------------------------------

        protected abstract void GenerateInput(Painter2D painter, TInputPort port);
        protected abstract void GenerateOutput(Painter2D painter, TOutputPort port);
    }
}