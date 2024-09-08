using System;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    internal abstract class TInspector : TGraphOverlay
    {
        protected const string NAME = "Inspector";
        protected const string ID = "GC-Overlay-Graph-Inspector";
        
        private const string NAME_ROOT = "GC-Overlay-Graph-Inspector-Root";
        private const string NAME_HEAD = "GC-Overlay-Graph-Inspector-Head";
        private const string NAME_BODY = "GC-Overlay-Graph-Inspector-Body";
        
        private const string NAME_BODY_TEXT = "GC-Overlay-Graph-Inspector-Body-Text";

        private const string TITLE_NO_SELECTION = "No Selection";
        private const string TXT_NO_SELECTION = "Nothing is selected";
        
        private const string TITLE_MULTI_SELECTION = "Selection ({0})";
        private const string TXT_MULTI_SELECTION = "Multiple selected";

        private static readonly IIcon ICON_NOTHING = new IconCubeOutline(ColorTheme.Type.TextLight);
        private static readonly IIcon ICON_MULTIPLE = new IconMultiple(ColorTheme.Type.TextLight);

        // MEMBERS: -------------------------------------------------------------------------------
        
        [NonSerialized] private VisualElement m_Root;
        [NonSerialized] private VisualElement m_Head;
        [NonSerialized] protected ScrollView m_Body;

        [NonSerialized] private Label m_Title;
        [NonSerialized] private Image m_Icon;

        [NonSerialized] private TNodeTool m_ActiveSelection;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override Layout supportedLayouts => Layout.Panel;
        
        // EVENTS: --------------------------------------------------------------------------------

        public event Action EventChange;
        
        // INITIALIZERS: --------------------------------------------------------------------------

        public override void OnCreated()
        {
            base.OnCreated();
            this.GraphWindow.Overlays.Inspector = this;

            this.GraphWindow.Selection.EventChangeSelection += OnChangeSelection;
            this.GraphWindow.EventChangePage += OnChangePage;
            EditorApplication.playModeStateChanged += this.OnChangePlayMode;
        }

        public override void OnWillBeDestroyed()
        {
            base.OnWillBeDestroyed();
            this.GraphWindow.Selection.EventChangeSelection -= OnChangeSelection;
            this.GraphWindow.EventChangePage -= OnChangePage;
            EditorApplication.playModeStateChanged -= this.OnChangePlayMode;
        }

        private void OnChangePage(TGraphTool prevGraphTool, TGraphTool nextGraphTool)
        {
            if (prevGraphTool != null) prevGraphTool.EventChange -= this.OnGraphToolChange;
            if (nextGraphTool != null) nextGraphTool.EventChange += this.OnGraphToolChange;
        }

        public override VisualElement CreatePanelContent()
        {
            this.m_Root = new VisualElement { name = NAME_ROOT };
            SetupStyleSheet(this.m_Root);
            
            this.m_Head = new VisualElement { name = NAME_HEAD };
            this.m_Body = new ScrollView
            {
                name = NAME_BODY,
                mode = ScrollViewMode.Vertical
            };
            
            this.m_Body.AddToClassList(AlignLabel.CLASS_UNITY_INSPECTOR_ELEMENT);
            this.m_Body.AddToClassList(AlignLabel.CLASS_UNITY_MAIN_CONTAINER);

            this.m_Icon = new Image();
            this.m_Title = new Label();
            
            this.m_Head.Add(this.m_Icon);
            this.m_Head.Add(this.m_Title);

            this.m_Root.Add(this.m_Head);
            this.m_Root.Add(this.m_Body);
            
            this.Refresh();
            
            return this.m_Root;
        }
        
        // CALLBACKS: -----------------------------------------------------------------------------
        
        private void OnChangeSelection()
        {
            if (this.m_Head == null) return;
            if (this.m_Body == null) return;
            
            this.m_Body.Clear();
            this.Refresh();
        }
        
        private void OnChangePlayMode(PlayModeStateChange state)
        {
            if (this.m_Head == null) return;
            if (this.m_Body == null) return;
            
            this.m_Body.Clear();
            this.Refresh();
        }

        private void OnChangeValue(SerializedPropertyChangeEvent changeEvent)
        {
            this.m_ActiveSelection?.OnChangeNode();
            this.EventChange?.Invoke();

            if (this.m_ActiveSelection == null) return;
            
            this.m_Icon.image = this.m_ActiveSelection.Icon;
            this.m_Title.text = this.m_ActiveSelection.Title;
        }
        
        private void OnGraphToolChange()
        {
            if (this.m_Head == null) return;
            if (this.m_Body == null) return;
            
            this.m_Body.Clear();
            this.Refresh();
        }

        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected virtual void Refresh()
        {
            TGraphTool currentPage = this.GraphWindow.CurrentPage;
            if (currentPage == null) return;

            TNodeTool[] selection = this.GraphWindow.Selection.Group;
            if (selection.Length == 0)
            {
                this.m_Icon.image = ICON_NOTHING.Texture;
                this.m_Title.text = TITLE_NO_SELECTION;

                Label text = new Label(TXT_NO_SELECTION) { name = NAME_BODY_TEXT };
                this.m_Body.Add(text);
                return;
            }
            
            if (selection.Length >= 2)
            {
                this.m_Icon.image = ICON_MULTIPLE.Texture;
                this.m_Title.text = string.Format(TITLE_MULTI_SELECTION, selection.Length);

                Label text = new Label(TXT_MULTI_SELECTION) { name = NAME_BODY_TEXT };
                this.m_Body.Add(text);
                return;
            }

            TNodeTool selected = selection[0];
            if (selected == null) return;
            
            this.m_Icon.image = selected.Icon;
            this.m_Title.text = selected.Title;

            PropertyField fieldNode = new PropertyField(selected.Property);

            this.m_Body.Add(fieldNode);
            fieldNode.Bind(selected.Property.serializedObject);
            
            this.m_ActiveSelection = selected;
            
            fieldNode.RegisterValueChangeCallback(this.OnChangeValue);
            
            this.m_Body.SetEnabled(!EditorApplication.isPlayingOrWillChangePlaymode);
        }
    }
}