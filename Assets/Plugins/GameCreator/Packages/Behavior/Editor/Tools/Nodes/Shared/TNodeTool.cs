using System;
using GameCreator.Editor.Common;
using GameCreator.Editor.VisualScripting;
using GameCreator.Runtime.Behavior;
using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    internal abstract class TNodeTool : VisualElement
    {
        public const string PROP_ID = "m_Id";
        public const string PROP_PORTS = "m_Ports";
        public const string PROP_INPUTS = "m_Inputs";
        public const string PROP_OUTPUTS = "m_Outputs";

        private const string NAME_ROOT = "GC-Graph-Node-Root";
        private const string NAME_WRAP = "GC-Graph-Node-Wrap";
        
        private const string NAME_HEAD = "GC-Graph-Node-Head";
        private const string NAME_BODY = "GC-Graph-Node-Body";
        private const string NAME_FOOT = "GC-Graph-Node-Foot";

        private const string NAME_PORTS_TOP = "GC-Graph-Node-Ports-Top";
        private const string NAME_PORTS_RIGHT = "GC-Graph-Node-Ports-Right";
        private const string NAME_PORTS_BOTTOM = "GC-Graph-Node-Ports-Bottom";
        private const string NAME_PORTS_LEFT = "GC-Graph-Node-Ports-Left";

        protected const string NAME_CONTENT_CONTENT = "GC-Graph-Node-Body-Content";
        protected const string NAME_CONTENT_SEPARATOR = "GC-Graph-Node-Body-Separator";

        private const float SNAP_VALUE = 10f;
        private const int SPACE_VALUE = 8;
        
        private static readonly Color COLOR_SELECTION = EditorGUIUtility.isProSkin
            ? new Color(230f/256f, 230f/256f, 230f/256f)
            : new Color(60f/256f, 60/256f, 60/256f);
        
        private static readonly Color COLOR_HOVER = EditorGUIUtility.isProSkin
            ? new Color(145f/256f, 145f/256f, 145f/256f)
            : new Color(120f/256f, 120f/256f, 120f/256f);
        
        private const int BORDER_EDITOR_WIDTH = 1;
        private const int BORDER_RUNTIME_WIDTH = 2;

        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private readonly VisualElement m_Wrap;
        [NonSerialized] protected readonly VisualElement m_Head;
        [NonSerialized] protected readonly VisualElement m_Body;
        [NonSerialized] protected readonly VisualElement m_Foot;

        [NonSerialized] private readonly Image m_HeadIcon;
        [NonSerialized] private readonly Label m_HeadLabel;
        
        [NonSerialized] private VisualElement m_ContentConditions;
        [NonSerialized] private VisualElement m_ContentSeparator;
        [NonSerialized] private VisualElement m_ContentInstructions;

        [NonSerialized] protected readonly VisualElement[] m_Ports;

        [NonSerialized] private readonly ManipulatorNodeHover m_ManipulatorNodeHover;
        [NonSerialized] private readonly ManipulatorNodeSelect m_ManipulatorNodeSelect;
        [NonSerialized] private readonly ManipulatorNodeDrag m_ManipulatorNodeDrag;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        public TNode Node => this.Property.managedReferenceValue as TNode;

        protected abstract float Width { get; }

        public abstract bool CanMove { get; }
        public abstract bool CanDelete { get; }
        
        protected abstract bool ShowHead { get; }
        protected abstract bool ShowBody { get; }
        protected abstract bool ShowFoot { get; }
        
        public abstract string Title { get; }
        public abstract Texture Icon { get; }

        public virtual bool ShowTitle => true;
        public virtual bool ShowIcon => true;

        protected abstract bool DrawConditions { get; }
        protected abstract bool DrawInstructions { get; }

        public string NodeId => this.Property
            .FindPropertyRelative(PROP_ID)
            .FindPropertyRelative(IdStringDrawer.NAME_STRING)
            .stringValue;

        public Vector2 Position
        {
            get => this.Property.FindPropertyRelative("m_Position").vector2Value;
            set
            {
                if (EditorApplication.isPlayingOrWillChangePlaymode) return;
                PositionWithoutNotify = value;
                
                this.EventChangePosition?.Invoke();
                this.GraphTool.Refresh();
            }
        }

        public Vector2 PositionWithoutNotify
        {
            get => this.Property.FindPropertyRelative("m_Position").vector2Value;
            set
            {
                if (EditorApplication.isPlayingOrWillChangePlaymode) return;

                if (TToolbar.Snap)
                {
                    value = new Vector2(
                        Mathf.Round(value.x / SNAP_VALUE) * SNAP_VALUE,
                        Mathf.Round(value.y / SNAP_VALUE) * SNAP_VALUE
                    );
                }
                
                this.Property.FindPropertyRelative("m_Position").vector2Value = value;
                this.GraphTool.SerializedObject.ApplyModifiedPropertiesWithoutUndo();
                
                this.transform.position = value - new Vector2(this.Width / 2f, 0f);
            }
        }

        [field: NonSerialized] public SerializedProperty Property { get; }

        public SerializedProperty InputPortsProperty => this.Property
            .FindPropertyRelative(PROP_PORTS)
            .FindPropertyRelative(PROP_INPUTS);

        public SerializedProperty OutputPortsProperty => this.Property
            .FindPropertyRelative(PROP_PORTS)
            .FindPropertyRelative(PROP_OUTPUTS);

        [field: NonSerialized] public TGraphTool GraphTool { get; }

        [field: NonSerialized] public TPortTool[] InputPortTools  { get; private set; }
        [field: NonSerialized] public TPortTool[] OutputPortTools { get; private set; }

        // EVENTS: --------------------------------------------------------------------------------

        public event Action EventChangePosition;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        protected TNodeTool(TGraphTool graphTool, SerializedProperty property)
        {
            this.name = NAME_ROOT;

            this.GraphTool = graphTool;
            this.Property = property;

            this.m_Wrap = new VisualElement { name = NAME_WRAP }; 
            this.m_Head = new VisualElement { name = NAME_HEAD };
            this.m_Body = new VisualElement { name = NAME_BODY };
            this.m_Foot = new VisualElement { name = NAME_FOOT };

            this.m_HeadIcon = new Image();
            this.m_HeadLabel = new Label();

            this.m_Head.Add(this.m_HeadIcon);
            this.m_Head.Add(this.m_HeadLabel);

            this.Add(this.m_Wrap);
            
            this.m_Wrap.Add(this.m_Head);
            this.m_Wrap.Add(this.m_Body);
            this.m_Wrap.Add(this.m_Foot);

            VisualElement portsTop = new VisualElement    { pickingMode = PickingMode.Ignore, name = NAME_PORTS_TOP    };
            VisualElement portsRight = new VisualElement  { pickingMode = PickingMode.Ignore, name = NAME_PORTS_RIGHT  };
            VisualElement portsBottom = new VisualElement { pickingMode = PickingMode.Ignore, name = NAME_PORTS_BOTTOM };
            VisualElement portsLeft = new VisualElement   { pickingMode = PickingMode.Ignore, name = NAME_PORTS_LEFT   };

            this.m_Ports = new[] { portsTop, portsRight, portsBottom, portsLeft };
            foreach (VisualElement portContainer in this.m_Ports) this.Add(portContainer);
            
            this.CreatePorts();
            this.CreateContent();

            this.m_ManipulatorNodeHover = new ManipulatorNodeHover();
            this.m_ManipulatorNodeSelect = new ManipulatorNodeSelect();
            this.m_ManipulatorNodeDrag = new ManipulatorNodeDrag();

            this.RefreshRoot();
            
            this.AddManipulator(this.m_ManipulatorNodeHover);
            this.AddManipulator(this.m_ManipulatorNodeSelect);

            if (this.CanMove)
            {
                this.AddManipulator(this.m_ManipulatorNodeDrag);
            }
        }
        
        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected virtual void CreatePorts()
        {
            SerializedProperty propertyInputs = this.InputPortsProperty;
            SerializedProperty propertyOutputs = this.OutputPortsProperty;
            
            this.InputPortTools = new TPortTool[propertyInputs.arraySize];
            this.OutputPortTools = new TPortTool[propertyOutputs.arraySize];

            TNode node = this.Node;

            for (int i = 0; i < this.InputPortTools.Length; ++i)
            {
                SerializedProperty port = propertyInputs.GetArrayElementAtIndex(i);
                TPortTool portTool = this.CreatePort(this, port);
                
                this.InputPortTools[i] = portTool;
                PortPosition position = ((TPort) port.managedReferenceValue).Position;
                
                this.m_Ports[(int) position].Add(portTool);
                this.GraphTool.PortTools.Add(portTool.PortId, portTool);
            }
            
            for (int i = 0; i < this.OutputPortTools.Length; ++i)
            {
                SerializedProperty port = propertyOutputs.GetArrayElementAtIndex(i);
                TPortTool portTool = this.CreatePort(this, port);
                
                this.OutputPortTools[i] = portTool;
                PortPosition position = ((TPort) port.managedReferenceValue).Position;
                
                this.m_Ports[(int) position].Add(portTool);
                this.GraphTool.PortTools.Add(portTool.PortId, portTool);
            }

            this.m_Ports[(int) PortPosition.Top].Insert(0, new FlexibleSpace { pickingMode = PickingMode.Ignore });
            this.m_Ports[(int) PortPosition.Top].Add(new FlexibleSpace { pickingMode = PickingMode.Ignore });
            this.m_Ports[(int) PortPosition.Bottom].Insert(0, new FlexibleSpace { pickingMode = PickingMode.Ignore });
            this.m_Ports[(int) PortPosition.Bottom].Add(new FlexibleSpace { pickingMode = PickingMode.Ignore });
            
            this.m_Ports[(int) PortPosition.Left].Insert(0, new SpaceCustom(SPACE_VALUE) { pickingMode = PickingMode.Ignore });
            this.m_Ports[(int) PortPosition.Left].Add(new FlexibleSpace { pickingMode = PickingMode.Ignore });
            this.m_Ports[(int) PortPosition.Right].Insert(0, new SpaceCustom(SPACE_VALUE) { pickingMode = PickingMode.Ignore });
            this.m_Ports[(int) PortPosition.Right].Add(new FlexibleSpace { pickingMode = PickingMode.Ignore });
        }

        protected virtual void CreateContent()
        {
            this.m_ContentConditions = new VisualElement { name = NAME_CONTENT_CONTENT };
            this.m_ContentSeparator = new VisualElement { name = NAME_CONTENT_SEPARATOR };
            this.m_ContentInstructions = new VisualElement { name = NAME_CONTENT_CONTENT };

            this.m_Body.Add(this.m_ContentConditions);
            this.m_Body.Add(this.m_ContentSeparator);
            this.m_Body.Add(this.m_ContentInstructions);

            this.m_ContentConditions.style.display = this.DrawConditions
                ? DisplayStyle.Flex
                : DisplayStyle.None;
            
            this.m_ContentInstructions.style.display = this.DrawInstructions
                ? DisplayStyle.Flex
                : DisplayStyle.None;

            this.m_ContentSeparator.style.display = this.DrawConditions && this.DrawInstructions
                ? DisplayStyle.Flex
                : DisplayStyle.None;
        }

        protected virtual void RefreshRoot()
        {
            this.style.width = this.Width;

            Vector2 position = this.Position - new Vector2(this.Width / 2f, 0f);
            this.transform.position = position;
            
            this.Refresh();
        }

        protected virtual void RefreshHead()
        {
            this.m_HeadIcon.image = this.ShowIcon ? this.Icon : null;
            this.m_HeadLabel.text = this.ShowTitle ? this.Title : string.Empty;

            this.m_Head.style.display = this.ShowHead
                ? DisplayStyle.Flex
                : DisplayStyle.None;
        }

        protected virtual void RefreshBody()
        {
            bool showBody = this.ShowBody;
            
            this.m_Body.style.display = showBody
                ? DisplayStyle.Flex
                : DisplayStyle.None;
            
            if (!showBody) return;
            
            int numConditions = 0;
            int numInstructions = 0;
            
            if (this.DrawConditions)
            {
                SerializedProperty conditions = this.Property
                    .FindPropertyRelative("m_Conditions")
                    .FindPropertyRelative(RunConditionsListDrawer.PROP_CONDITIONS)
                    .FindPropertyRelative(ConditionListDrawer.NAME_CONDITIONS);
            
                numConditions = conditions.arraySize;
                
                for (int i = this.m_ContentConditions.childCount; i < numConditions; ++i)
                {
                    this.m_ContentConditions.Add(new NodeConditionTool());
                }
                
                for (int i = this.m_ContentConditions.childCount - 1; i >= numConditions; --i)
                {
                    this.m_ContentConditions.RemoveAt(i);
                }
                
                for (int i = 0; i < numConditions; ++i)
                {
                    NodeConditionTool instance = this.m_ContentConditions[i] as NodeConditionTool;
                    instance?.Refresh(conditions.GetArrayElementAtIndex(i));
                }
            }

            if (this.DrawInstructions)
            {
                SerializedProperty instructions = this.Property
                    .FindPropertyRelative("m_Instructions")
                    .FindPropertyRelative(RunInstructionsListDrawer.PROP_INSTRUCTIONS)
                    .FindPropertyRelative(InstructionListDrawer.NAME_INSTRUCTIONS);

                numInstructions = instructions.arraySize;
            
                for (int i = this.m_ContentInstructions.childCount; i < numInstructions; ++i)
                {
                    this.m_ContentInstructions.Add(new NodeInstructionTool());
                }

                for (int i = this.m_ContentInstructions.childCount - 1; i >= numInstructions; --i)
                {
                    this.m_ContentInstructions.RemoveAt(i);
                }

                for (int i = 0; i < numInstructions; ++i)
                {
                    NodeInstructionTool instance = this.m_ContentInstructions[i] as NodeInstructionTool;
                    instance?.Refresh(instructions.GetArrayElementAtIndex(i));
                }   
            }

            this.m_ContentConditions.style.display = numConditions > 0
                ? DisplayStyle.Flex
                : DisplayStyle.None;
            
            this.m_ContentInstructions.style.display = numInstructions > 0
                ? DisplayStyle.Flex
                : DisplayStyle.None;
            
            this.m_ContentSeparator.style.display = numConditions > 0 && numInstructions > 0
                ? DisplayStyle.Flex
                : DisplayStyle.None;
        }

        protected virtual void RefreshFoot()
        {
            this.m_Foot.style.display = this.ShowFoot
                ? DisplayStyle.Flex
                : DisplayStyle.None;
        }
        
        protected virtual void RefreshPorts()
        { }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public virtual void Refresh()
        {
            int borderWidth = EditorApplication.isPlayingOrWillChangePlaymode
                ? BORDER_RUNTIME_WIDTH
                : BORDER_EDITOR_WIDTH;
            
            this.m_Wrap.style.borderTopWidth = borderWidth;
            this.m_Wrap.style.borderBottomWidth = borderWidth;
            this.m_Wrap.style.borderRightWidth = borderWidth;
            this.m_Wrap.style.borderLeftWidth = borderWidth;

            Color borderColor = GraphUtils.Dark;
            
            if (Application.isPlaying)
            {
                IdString nodeId = new IdString(this.NodeId);
                Status status = TargetUtils.Get?.GetStatus(nodeId) ?? Status.Ready;
                
                borderColor = GraphUtils.GetColor(status, GraphUtils.Dark);
            }

            this.m_Wrap.style.borderTopColor = borderColor;
            this.m_Wrap.style.borderBottomColor = borderColor;
            this.m_Wrap.style.borderRightColor = borderColor;
            this.m_Wrap.style.borderLeftColor = borderColor;
            
            this.RefreshHead();
            this.RefreshBody();
            this.RefreshFoot();
            this.RefreshPorts();
        }
        
        public virtual void OnSelect()
        {
            this.SetSelectionBorder(COLOR_SELECTION);
        }

        public virtual void OnDeselect()
        {
            this.SetSelectionBorder(StyleKeyword.Initial);
        }

        public virtual void OnHoverEnter()
        {
            if (this.GraphTool.Window.Selection.IsSelected(this)) return;
            this.SetSelectionBorder(COLOR_HOVER);
        }
        
        public virtual void OnHoverExit()
        {
            if (this.GraphTool.Window.Selection.IsSelected(this)) return;
            this.SetSelectionBorder(StyleKeyword.Initial);
        }
        
        public virtual void OnChangeNode()
        {
            this.Refresh();
        }
        
        public virtual void OnMoveChildren()
        { }
        
        public void PasteValues(object source)
        {
            this.Node.CopyFrom(source);
            
            this.Property.serializedObject.Update();
            this.Property.serializedObject.ApplyModifiedPropertiesWithoutUndo();
            
            this.GraphTool.Refresh();
        }

        // ABSTRACT METHODS: ----------------------------------------------------------------------

        public abstract string GetPortText(string portId);
        
        protected abstract TPortTool CreatePort(TNodeTool nodeTool, SerializedProperty property);

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void SetSelectionBorder(StyleColor styleColor)
        {
            this.style.borderTopColor = styleColor;
            this.style.borderBottomColor = styleColor;
            this.style.borderRightColor = styleColor;
            this.style.borderLeftColor = styleColor;
        }
    }
}