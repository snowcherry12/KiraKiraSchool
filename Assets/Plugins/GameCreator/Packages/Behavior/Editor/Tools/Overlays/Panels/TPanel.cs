using System;
using GameCreator.Runtime.Behavior;
using GameCreator.Runtime.Common;
using UnityEditor.Overlays;
using UnityEditor.Toolbars;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    internal abstract class TPanel : 
        TGraphOverlay, 
        ICreateHorizontalToolbar, 
        ICreateVerticalToolbar
    {
        protected const string NAME = "Panel";
        protected const string ID = "GC-Overlay-Graph-Panel";
        
        private const string NAME_ROOT = "GC-Overlay-Graph-Panel-Root";

        private static readonly IIcon ICON_BLACKBOARD_SOLID = new IconBehaviorBlackboardSolid(ColorTheme.Type.TextNormal);
        private static readonly IIcon ICON_BLACKBOARD_OUTLINE = new IconBehaviorBlackboardOutline(ColorTheme.Type.TextLight);
        
        private static readonly IIcon ICON_INSPECTOR_SOLID = new IconBehaviorInspectorSolid(ColorTheme.Type.TextNormal);
        private static readonly IIcon ICON_INSPECTOR_OUTLINE = new IconBehaviorInspectorOutline(ColorTheme.Type.TextLight);
        
        private static readonly IIcon ICON_BREADCRUMBS_SOLID = new IconBehaviorBreadcrumbsSolid(ColorTheme.Type.TextNormal);
        private static readonly IIcon ICON_BREADCRUMBS_OUTLINE = new IconBehaviorBreadcrumbsOutline(ColorTheme.Type.TextLight);

        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private OverlayToolbar m_Toolbar;
        
        [NonSerialized] private EditorToolbarToggle m_ButtonBlackboard;
        [NonSerialized] private EditorToolbarToggle m_ButtonInspector;
        [NonSerialized] private EditorToolbarToggle m_ButtonBreadcrumb;
        [NonSerialized] private EditorToolbarButton m_ButtonMaximize;
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override Layout supportedLayouts => Layout.HorizontalToolbar |
                                                      Layout.VerticalToolbar;

        // INITIALIZERS: --------------------------------------------------------------------------

        public override void OnCreated()
        {
            base.OnCreated();
            this.GraphWindow.Overlays.Panel = this;
            this.GraphWindow.Overlays.EventDisplay += this.RefreshButtons;
        }

        public override void OnWillBeDestroyed()
        {
            base.OnWillBeDestroyed();
            this.GraphWindow.Overlays.EventDisplay -= this.RefreshButtons;
        }

        // CONTENT: -------------------------------------------------------------------------------

        public override VisualElement CreatePanelContent() => this.CreateOverlay();
        public virtual OverlayToolbar CreateHorizontalToolbarContent() => this.CreateOverlay();
        public virtual OverlayToolbar CreateVerticalToolbarContent() => this.CreateOverlay();

        private OverlayToolbar CreateOverlay()
        {
            this.m_Toolbar = new OverlayToolbar { name = NAME_ROOT };
            
            this.CreateButtons();
            this.m_Toolbar.SetupChildrenAsButtonStrip();
            return this.m_Toolbar;
        }
        
        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected virtual void CreateButtons()
        {
            this.m_ButtonBlackboard = new EditorToolbarToggle(
                string.Empty,
                ICON_BLACKBOARD_SOLID.Texture,
                ICON_BLACKBOARD_OUTLINE.Texture
            )
            {
                tooltip = "Toggle the Blackboard visibility",
            };
            
            this.m_ButtonInspector = new EditorToolbarToggle(
                string.Empty,
                ICON_INSPECTOR_SOLID.Texture,
                ICON_INSPECTOR_OUTLINE.Texture
            )
            {
                tooltip = "Toggle the Inspector visibility",
            };
            
            this.m_ButtonBreadcrumb = new EditorToolbarToggle(
                string.Empty,
                ICON_BREADCRUMBS_SOLID.Texture,
                ICON_BREADCRUMBS_OUTLINE.Texture
            )
            {
                tooltip = "Toggle the Breadcrumbs Toolbar visibility",
            };
            
            this.m_ButtonMaximize = new EditorToolbarButton(
                string.Empty,
                new IconFullscreen(ColorTheme.Type.TextNormal).Texture,
                this.ChangeMaximize
            )
            {
                tooltip = "Maximize or minimize the window",
            };
            
            this.m_ButtonBlackboard.RegisterValueChangedCallback(this.ChangeBlackboard);
            this.m_ButtonInspector.RegisterValueChangedCallback(this.ChangeInspector);
            this.m_ButtonBreadcrumb.RegisterValueChangedCallback(this.ChangeBreadcrumb);
            
            this.m_Toolbar.Add(this.m_ButtonBlackboard);
            this.m_Toolbar.Add(this.m_ButtonInspector);
            this.m_Toolbar.Add(this.m_ButtonBreadcrumb);
            this.m_Toolbar.Add(this.m_ButtonMaximize);

            this.RefreshButtons();
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void RefreshButtons()
        {
            this.m_ButtonBreadcrumb?.SetValueWithoutNotify(this.GraphWindow.Overlays.Breadcrumb.displayed);
            this.m_ButtonBlackboard?.SetValueWithoutNotify(this.GraphWindow.Overlays.Blackboard.displayed);
            this.m_ButtonInspector?.SetValueWithoutNotify(this.GraphWindow.Overlays.Inspector.displayed);
        }

        private void ChangeBlackboard(ChangeEvent<bool> eventChange)
        {
            switch (eventChange.newValue)
            {
                case true: this.GraphWindow.Overlays.Blackboard?.Show(); break;
                case false: this.GraphWindow.Overlays.Blackboard?.Hide(); break;
            }
        }
        
        private void ChangeInspector(ChangeEvent<bool> eventChange)
        {
            switch (eventChange.newValue)
            {
                case true: this.GraphWindow.Overlays.Inspector?.Show(); break;
                case false: this.GraphWindow.Overlays.Inspector?.Hide(); break;
            }
        }
        
        private void ChangeBreadcrumb(ChangeEvent<bool> eventChange)
        {
            switch (eventChange.newValue)
            {
                case true: this.GraphWindow.Overlays.Breadcrumb?.Show(); break;
                case false: this.GraphWindow.Overlays.Breadcrumb?.Hide(); break;
            }
        }

        private void ChangeMaximize()
        {
            if (this.GraphWindow == null) return;

            bool newValue = !this.GraphWindow.maximized;
            this.GraphWindow.maximized = newValue;
        }
    }
}