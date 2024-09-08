using System;
using System.Collections.Generic;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Behavior;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    internal abstract class TGraphTool : VisualElement
    {
        private const string NAME_GRAPH_TOOL = "GC-GraphTool";
        private const float FRAME_PADDING = 50f;

        private const string MENU_CONTEXT = "Create/{0}";
        private const string MENU_CREATE = "Create {0}";

        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private readonly ManipulatorGraphZoom m_ManipulatorGraphZoom;
        [NonSerialized] private readonly ManipulatorGraphPan m_ManipulatorGraphPan;
        [NonSerialized] private readonly ManipulatorGraphMenu m_ManipulatorGraphMenu;
        [NonSerialized] private readonly ManipulatorGraphSelect m_ManipulatorGraphSelect;

        // PROPERTIES: ----------------------------------------------------------------------------

        public Graph Graph { get; }
        public SerializedObject SerializedObject { get; }

        [field: NonSerialized] public TGraphWindow Window { get; }
        
        [field: NonSerialized] public GraphGrid Grid     { get; }
        [field: NonSerialized] public TGraphWires Wires   { get; }
        [field: NonSerialized] public GraphView View     { get; }
        [field: NonSerialized] public GraphSelect Select { get; }

        private SerializedProperty PropertyNodes => this.SerializedObject.FindProperty("m_Nodes");
        
        [field: NonSerialized] public Dictionary<string, TNodeTool> NodeTools { get; }
        [field: NonSerialized] public Dictionary<string, TPortTool> PortTools { get; }

        public int InstanceId => this.Graph != null ? this.Graph.GetInstanceID() : 0;
        
        public abstract bool AllowCycles { get; }

        public bool IsSelecting => this.m_ManipulatorGraphSelect.IsSelecting;

        // EVENTS: --------------------------------------------------------------------------------

        public event Action EventChange;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        protected TGraphTool(Graph graph, TGraphWindow window)
        {
            this.Graph = graph;
            this.SerializedObject = new SerializedObject(graph);
            
            this.name = NAME_GRAPH_TOOL;
            this.Window = window;
            
            this.NodeTools = new Dictionary<string, TNodeTool>();
            this.PortTools = new Dictionary<string, TPortTool>();

            this.Grid = new GraphGrid(this);
            this.Wires = this.CreateGraphWires(this);
            this.View = new GraphView(this);
            this.Select = new GraphSelect(this);

            this.Add(this.Grid);
            this.Add(this.Wires);
            this.Add(this.View);
            this.Add(this.Select);

            this.m_ManipulatorGraphPan = new ManipulatorGraphPan();
            this.m_ManipulatorGraphZoom = new ManipulatorGraphZoom();
            this.m_ManipulatorGraphMenu = new ManipulatorGraphMenu(this);
            this.m_ManipulatorGraphSelect = new ManipulatorGraphSelect(this);

            string keyPosition = string.Format(GraphView.KEY_VIEW_POSITION, this.InstanceId);
            string keyScale = string.Format(GraphView.KEY_VIEW_SCALE, this.InstanceId);

            Vector2 defaultPosition = new Vector2(
                this.Window.position.width / 2f,
                this.Window.position.height / 2f
            );
            
            Vector2 startPosition = SessionState.GetVector3(keyPosition, defaultPosition);
            float startScale = SessionState.GetFloat(keyScale, 1f);
            
            this.SetView(startPosition, startScale);
        }

        public virtual void OnEnable()
        {
            this.AddManipulator(this.m_ManipulatorGraphPan);
            this.AddManipulator(this.m_ManipulatorGraphZoom);
            this.AddManipulator(this.m_ManipulatorGraphMenu);
            this.AddManipulator(this.m_ManipulatorGraphSelect);
            
            this.Recreate();

            this.style.display = DisplayStyle.Flex;

            EditorApplication.playModeStateChanged += this.OnChangePlayMode;
            UnityEditor.Selection.selectionChanged += this.Refresh;
            this.Window.Overlays.Inspector.EventChange += this.Refresh;
            TargetUtils.EventRefresh += Refresh;
        }

        public virtual void OnDisable()
        {
            this.RemoveManipulator(this.m_ManipulatorGraphPan);
            this.RemoveManipulator(this.m_ManipulatorGraphZoom);
            this.RemoveManipulator(this.m_ManipulatorGraphMenu);
            this.RemoveManipulator(this.m_ManipulatorGraphSelect);
            
            this.View.ClearContent();

            this.style.display = DisplayStyle.None;
            
            EditorApplication.playModeStateChanged -= this.OnChangePlayMode;
            UnityEditor.Selection.selectionChanged -= this.Refresh;
            this.Window.Overlays.Inspector.EventChange -= this.Refresh;
            TargetUtils.EventRefresh -= Refresh;
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void SetZoom(float scale)
        {
            this.View.Scale = scale;
            this.Refresh();
        }

        public void SetPosition(Vector2 position)
        {
            position.x = GraphUtils.RoundToPixelGrid(position.x);
            position.y = GraphUtils.RoundToPixelGrid(position.y);
            
            this.View.Position = position;
            this.Refresh();
        }
        
        public void SetView(Vector2 position, float scale)
        {
            position.x = GraphUtils.RoundToPixelGrid(position.x);
            position.y = GraphUtils.RoundToPixelGrid(position.y);
            
            this.View.Position = position;
            this.View.Scale = scale;
            
            this.Refresh();
        }

        public void Refresh()
        {
            if (this.Graph == null)
            {
                int keepAmount = this.Window.Pages.Count - 1;
                this.Window.Backtrack(keepAmount);
                
                return;
            }
            
            if (this.SerializedObject == null) return; 
            this.SerializedObject.Update();
            
            foreach (KeyValuePair<string, TNodeTool> entry in this.NodeTools)
            {
                entry.Value?.Refresh();
            }
            
            foreach (KeyValuePair<string, TPortTool> entry in this.PortTools)
            {
                entry.Value?.Refresh();
            }
            
            this.Grid.MarkDirtyRepaint();
            this.Wires.MarkDirtyRepaint();
            this.Select.MarkDirtyRepaint();
        }

        public void ScheduleRefresh()
        {
            EditorApplication.delayCall -= this.Refresh;
            EditorApplication.delayCall += this.Refresh;
        }

        public void Recreate()
        {
            this.NodeTools.Clear();
            this.PortTools.Clear();
            
            this.View.ClearContent();
            
            if (this.Graph == null) return;
            this.SerializedObject.Update();
            
            SerializedProperty nodes = PropertyNodes;
            for (int i = 0; i < nodes.arraySize; ++i)
            {
                SerializedProperty node = nodes.GetArrayElementAtIndex(i);
                TNodeTool nodeTool = this.CreateFromNode(node);
                
                if (nodeTool == null) continue;
                
                this.NodeTools.Add(nodeTool.NodeId, nodeTool);
                this.View.AddNode(nodeTool);
            }
            
            this.Refresh();
        }

        public void DeleteNode(string deleteNodeId)
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;
            
            if (!this.NodeTools.TryGetValue(deleteNodeId, out TNodeTool nodeTool)) return;
            if (nodeTool is not { CanDelete: true }) return;

            this.SerializedObject.Update();

            foreach (TPortTool inputPortTool in nodeTool.InputPortTools)
            {
                this.RemovePortsTo(inputPortTool.PortId);
            }

            int nodesSize = this.PropertyNodes.arraySize;
            for (int i = nodesSize - 1; i >= 0; --i)
            {
                SerializedProperty propertyNode = this.PropertyNodes.GetArrayElementAtIndex(i);
                string nodeId = propertyNode
                    .FindPropertyRelative(TNodeTool.PROP_ID)
                    .FindPropertyRelative(IdStringDrawer.NAME_STRING)
                    .stringValue;
                
                if (deleteNodeId != nodeId) continue;
                
                this.View.RemoveNode(nodeTool);
                this.NodeTools.Remove(nodeId);
                
                this.PropertyNodes.DeleteArrayElementAtIndex(i);

                this.SerializedObject.ApplyModifiedPropertiesWithoutUndo();
                this.SerializedObject.Update();
                
                foreach (TPortTool outputPortTool in nodeTool.OutputPortTools)
                {
                    outputPortTool.PropertyPort.serializedObject.Update();
                }
                
                foreach (TPortTool inputPortTool in nodeTool.InputPortTools)
                {
                    inputPortTool.PropertyPort.serializedObject.Update();
                }
                
                this.Recreate();
            }

            this.SerializedObject.ApplyModifiedPropertiesWithoutUndo();
            this.SerializedObject.Update();
            
            this.EventChange?.Invoke();
        }

        public void ConnectPortTools(TPortTool a, TPortTool b)
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;
            
            if (a == null) return;
            if (b == null) return;
            
            if (a.NodeTool == b.NodeTool) return;
            
            TPort portA = a.Port;
            TPort portB = b.Port;

            if (portA.Mode == portB.Mode) return;
            
            TPortTool inputPortTool = portA.Mode == PortMode.Input ? a : b;
            TPortTool outputPortTool = portA.Mode == PortMode.Output ? a : b;

            TPort inputPort = inputPortTool.Port;
            TPort outputPort = outputPortTool.Port;
            
            if (inputPort == null) return;
            if (outputPort == null) return;

            if (!this.AllowCycles)
            {
                string outputNodeId = outputPortTool.NodeTool.NodeId;
                if (GraphUtils.CanReach(this, inputPortTool.NodeTool, outputNodeId)) return;
            }

            if (inputPort.Allowance == PortAllowance.Single) this.RemovePortsTo(inputPortTool.PortId);
            if (outputPort.Allowance == PortAllowance.Single) this.RemovePortsFrom(outputPortTool);

            int connectionsCount = outputPortTool.Connections.arraySize;
            outputPortTool.Connections.InsertArrayElementAtIndex(connectionsCount);
            SerializationUtils.ApplyUnregisteredSerialization(this.SerializedObject);
            
            outputPortTool
                .Connections
                .GetArrayElementAtIndex(connectionsCount)
                .FindPropertyRelative(ConnectionDrawer.PROP_VALUE)
                .stringValue = inputPortTool.PortId;
            
            SerializationUtils.ApplyUnregisteredSerialization(this.SerializedObject);
            outputPortTool.NodeTool.OnMoveChildren();
            
            this.EventChange?.Invoke();
        }

        public void RemovePortsTo(string portId)
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;
            
            this.SerializedObject.Update();

            foreach (KeyValuePair<string, TNodeTool> entry in this.NodeTools)
            {
                if (entry.Value == null) continue;
                foreach (TPortTool outputPortTool in entry.Value.OutputPortTools)
                {
                    SerializedProperty connections = outputPortTool.Connections;
                    for (int i = connections.arraySize - 1; i >= 0; --i)
                    {
                        SerializedProperty connection = connections.GetArrayElementAtIndex(i);
                        string connectionValue = connection
                            .FindPropertyRelative(ConnectionDrawer.PROP_VALUE)
                            .stringValue;

                        if (connectionValue != portId) continue;
                        
                        connections.DeleteArrayElementAtIndex(i);
                        connections.serializedObject.ApplyModifiedPropertiesWithoutUndo();
                    }
                }
            }
            
            this.SerializedObject.Update();
            this.EventChange?.Invoke();
        }

        public void RemovePortsFrom(TPortTool outputPortTool)
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;
            
            SerializedObject serializedObject = outputPortTool.Connections.serializedObject; 
            serializedObject.Update();
            
            outputPortTool.Connections.ClearArray();
            
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
            serializedObject.Update();
            
            this.SerializedObject.Update();
            this.EventChange?.Invoke();
        }
        
        public void FrameSelection()
        {
            TNodeTool[] selection = this.Window.Selection.Group;

            if (selection.Length == 0)
            {
                Vector2 resetPosition = this.View.Position;
                
                Vector2 resetCenter = this.View.ChangeToContentCoordinatesFrom(
                    this.View,
                    this.View.layout.size * 0.5f
                );
                
                float extraX = resetCenter.x + this.View.ContentRect.x;
                float extraY = resetCenter.y + this.View.ContentRect.y;
                
                resetPosition += new Vector2(extraX, extraY) * this.View.Scale;
                resetPosition -= new Vector2(extraX, extraY);
                
                this.SetView(resetPosition, 1f);
                return;
            }

            Rect envelope = Rect.zero;
            foreach (TNodeTool nodeTool in selection)
            {
                Vector2 nodeCenter = nodeTool.Position;
                Vector2 nodeSize = nodeTool.layout.size;
                
                Rect nodeRect = new Rect(
                    nodeCenter - Vector2.right * nodeSize.x * 0.5f,
                    nodeSize
                );
                
                envelope = envelope != Rect.zero 
                    ? GraphUtils.ExpandWith(envelope, nodeRect) 
                    : nodeRect;
            }

            envelope = GraphUtils.Expand(envelope, FRAME_PADDING);

            float scale = Math.Clamp(
                Math.Min(
                    this.View.layout.size.x / envelope.width, 
                    this.View.layout.size.y / envelope.height
                ),
                ManipulatorGraphZoom.ZOOM_MIN, 
                1f
            );

            Vector2 position = this.View.layout.size * 0.5f - envelope.center;

            this.SetView(position, 1f);
            
            Vector2 zoomCenter = this.View.ChangeToContentCoordinatesFrom(
                this.View,
                this.View.layout.size * 0.5f
            );
            
            float x = zoomCenter.x + this.View.ContentRect.x;
            float y = zoomCenter.y + this.View.ContentRect.y;
            
            position += new Vector2(x, y);
            position -= new Vector2(x, y) * scale;
            
            position.x = GraphUtils.RoundToPixelGrid(position.x);
            position.y = GraphUtils.RoundToPixelGrid(position.y);

            this.SetView(position, scale);
        }

        public void OpenCreationMenu(MouseUpEvent eventMouseUp, string fromPortId, Vector2 direction)
        {
            this.m_ManipulatorGraphMenu.CreationMenu(eventMouseUp, fromPortId, direction);
        }

        // PRIVATE CALLBACKS: ---------------------------------------------------------------------

        private void OnChangePlayMode(PlayModeStateChange playModeStateChange)
        {
            this.Recreate();
        }

        // PUBLIC CALLBACKS: ----------------------------------------------------------------------
        
        public virtual bool OnValidateCommand(string commandName)
        {
            return CommandsUtils.LIST.ContainsKey(commandName);
        }
        
        public virtual bool OnExecuteCommand(string commandName, ArgData argData)
        {
            return CommandsUtils.LIST.TryGetValue(
                commandName,
                out CommandHandle callback
            ) && (callback?.Invoke(this, argData) ?? false);
        }

        public void GenerateGraphContextMenu(ContextualMenuPopulateEvent eventMenu)
        {
            ArgData argData = new ArgData(
                this.View.ChangeToContentCoordinatesFrom(
                    this.Window.rootVisualElement,
                    eventMenu.mousePosition
                ),
                Vector2.zero,
                string.Empty, 
                null
            );
            
            this.OnGenerateCreateMenu(eventMenu, MENU_CONTEXT, argData);
            this.OnGenerateGraphContextMenu(eventMenu, argData);
        }

        public void GenerateCreateMenu(
            ContextualMenuPopulateEvent eventMenu,
            string fromPortId,
            Vector2 direction,
            ContextMenuHandle callback)
        {
            string prefix = string.IsNullOrEmpty(fromPortId) ? MENU_CONTEXT : MENU_CREATE;
            VisualElement root = this.Window.rootVisualElement;
            
            ArgData argData = new ArgData(
                this.View.ChangeToContentCoordinatesFrom(root, eventMenu.mousePosition),
                direction,
                fromPortId,
                callback
            );
            
            this.OnGenerateCreateMenu(eventMenu, prefix, argData);
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void CreateNode(Type type, ArgData argData)
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;
            
            Vector2 position = argData?.Cursor ?? default;

            if (this.Graph == null) return;
            this.SerializedObject.Update();
            
            SerializedProperty nodes = PropertyNodes;

            if (Activator.CreateInstance(type) is not TNode instance) return;
            
            int newIndex = nodes.arraySize;
            nodes.InsertArrayElementAtIndex(newIndex);

            instance.SetPortDirection(argData?.Direction ?? default);
            
            SerializedProperty node = nodes.GetArrayElementAtIndex(newIndex);
            node.SetValue(instance);
            node.FindPropertyRelative("m_Position").vector2Value = position;
            
            this.SerializedObject.ApplyModifiedPropertiesWithoutUndo();
            this.SerializedObject.Update();
            
            TNodeTool nodeTool = this.CreateFromNode(node);
            if (nodeTool == null) return;
            
            this.NodeTools.Add(nodeTool.NodeId, nodeTool);
            this.View.AddNode(nodeTool);
            
            argData?.Callback?.Invoke(argData.FromPortId, nodeTool);
            this.Window.Selection.Active = nodeTool;
        }
        
        public abstract bool AcceptNode(TNode node);

        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected abstract TNodeTool CreateFromNode(SerializedProperty nodeProperty);

        protected abstract TGraphWires CreateGraphWires(TGraphTool graphTool);

        // PROTECTED MENU METHODS: ----------------------------------------------------------------
        
        protected DropdownMenuAction.Status MenuNormalStatus(DropdownMenuAction menuAction)
        {
            return DropdownMenuAction.Status.Normal;
        }

        protected DropdownMenuAction.Status MenuDisabledStatus(DropdownMenuAction menuAction)
        {
            return DropdownMenuAction.Status.Disabled;
        }
        
        protected DropdownMenuAction.Status MenuCanCreateNode(DropdownMenuAction menuAction)
        {
            return EditorApplication.isPlayingOrWillChangePlaymode 
                ? DropdownMenuAction.Status.Disabled
                : DropdownMenuAction.Status.Normal;
        }

        protected DropdownMenuAction.Status MenuCanCopyNode(DropdownMenuAction menuAction)
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return DropdownMenuAction.Status.Disabled;
            }
            
            if (!this.Window.Selection.HasSelection)
            {
                return DropdownMenuAction.Status.Disabled;
            }
            
            return this.Window.Selection.Active is { CanDelete: true } 
                ? DropdownMenuAction.Status.Normal
                : DropdownMenuAction.Status.Disabled;
        }
        
        protected DropdownMenuAction.Status MenuCanPasteNode(DropdownMenuAction menuAction)
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return DropdownMenuAction.Status.Disabled;
            }

            return CopyPasteUtils.CanSoftPaste(typeof(TNode))
                ? DropdownMenuAction.Status.Normal
                : DropdownMenuAction.Status.Disabled;
        }

        protected DropdownMenuAction.Status MenuCanDeleteNode(DropdownMenuAction menuAction)
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return DropdownMenuAction.Status.Disabled;
            }

            if (!this.Window.Selection.HasSelection)
            {
                return DropdownMenuAction.Status.Disabled;
            }
            
            foreach (TNodeTool selection in this.Window.Selection.Group)
            {
                if (selection?.CanDelete ?? false)
                {
                    return DropdownMenuAction.Status.Normal;
                }
            }

            return DropdownMenuAction.Status.Disabled;
        }
        
        protected DropdownMenuAction.Status MenuCanInspectNode(DropdownMenuAction menuAction)
        {
            if (this.Window.Overlays.Inspector.displayed)
            {
                return DropdownMenuAction.Status.Disabled;
            }
            
            return this.Window.Selection.Group.Length >= 1
                ? DropdownMenuAction.Status.Normal
                : DropdownMenuAction.Status.Disabled;
        }

        protected void OnGenerateGraphContextMenu(ContextualMenuPopulateEvent eventMenu, ArgData argData)
        {
            eventMenu.menu.AppendAction("Inspect", this.Inspect, this.MenuCanInspectNode, argData);
            eventMenu.menu.AppendAction("Select All", SelectEverything, this.MenuNormalStatus, this);
            
            eventMenu.menu.AppendSeparator();
            
            eventMenu.menu.AppendAction("Copy", Copy, this.MenuCanCopyNode, argData);
            eventMenu.menu.AppendAction("Cut", Cut, this.MenuCanCopyNode, argData);
            eventMenu.menu.AppendAction("Duplicate", Duplicate, this.MenuCanCopyNode, argData);
            eventMenu.menu.AppendAction("Paste", Paste, this.MenuCanPasteNode, argData);
            
            if (this.Window.Selection.HasSelection)
            {
                string deleteName = this.Window.Selection.Group.Length == 1 && this.Window.Selection.Active != null
                    ? $"Delete {this.Window.Selection.Active.Title}"
                    : $"Delete {this.Window.Selection.Group.Length} nodes";
                
                eventMenu.menu.AppendSeparator();
                eventMenu.menu.AppendAction(deleteName, Delete, this.MenuCanDeleteNode, argData);
            }
            else
            {
                eventMenu.menu.AppendSeparator();
                eventMenu.menu.AppendAction("Delete", Delete, this.MenuDisabledStatus, argData);
            }
        }

        protected abstract void OnGenerateCreateMenu(
            ContextualMenuPopulateEvent eventMenu, 
            string prefix, 
            ArgData argData
        );

        protected void Inspect(DropdownMenuAction menuAction)
        {
            this.Window.Overlays.Inspector.Show();
        }

        protected void SelectEverything(DropdownMenuAction menuAction)
        {
            ArgData argData = menuAction.userData as ArgData;
            CommandsUtils.SelectEverything(this, argData);
        }
        
        protected void Copy(DropdownMenuAction menuAction)
        {
            ArgData argData = menuAction.userData as ArgData;
            CommandsUtils.Copy(this, argData);
        }
        
        protected void Cut(DropdownMenuAction menuAction)
        {
            ArgData argData = menuAction.userData as ArgData;
            CommandsUtils.Cut(this, argData);
        }
        
        protected void Paste(DropdownMenuAction menuAction)
        {
            ArgData argData = menuAction.userData as ArgData;
            CommandsUtils.Paste(this, argData);
        }
        
        protected void Duplicate(DropdownMenuAction menuAction)
        {
            ArgData argData = menuAction.userData as ArgData;
            CommandsUtils.Duplicate(this, argData);
        }

        protected void Delete(DropdownMenuAction menuAction)
        {
            ArgData argData = menuAction.userData as ArgData;
            CommandsUtils.Delete(this, argData);
        }
        
        protected void CreateNode<T>(DropdownMenuAction menuAction) where T : TNode
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;
            
            ArgData argData = menuAction.userData as ArgData;
            this.CreateNode(typeof(T), argData);
        }
    }
}