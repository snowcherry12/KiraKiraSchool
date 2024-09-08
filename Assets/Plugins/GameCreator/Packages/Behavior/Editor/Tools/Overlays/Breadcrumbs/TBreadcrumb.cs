using System;
using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEditor.Toolbars;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    internal abstract class TBreadcrumb : TGraphOverlay, ICreateHorizontalToolbar
    {
        protected const string NAME = "Breadcrumb";
        protected const string ID = "GC-Overlay-Graph-Breadcrumb";

        private const float EDGE_RADIUS = 10f;
        private const float PADDING_LEFT = 7f;
        private const float PADDING_RIGHT = 2f;

        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private OverlayToolbar m_Toolbar;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        protected override Layout supportedLayouts => Layout.HorizontalToolbar;

        // INITIALIZERS: --------------------------------------------------------------------------

        public override void OnCreated()
        {
            base.OnCreated();
            this.GraphWindow.Overlays.Breadcrumb = this;
            this.GraphWindow.EventChangePage += this.OnChangePage;
        }

        public override void OnWillBeDestroyed()
        {
            base.OnWillBeDestroyed();
            this.GraphWindow.EventChangePage -= this.OnChangePage;
        }

        // CONTENT: -------------------------------------------------------------------------------

        public override VisualElement CreatePanelContent() => new VisualElement();

        public virtual OverlayToolbar CreateHorizontalToolbarContent()
        {
            this.m_Toolbar = new OverlayToolbar();
            this.RefreshBreadcrumbs();
            
            return this.m_Toolbar;
        }
        
        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected virtual void RefreshBreadcrumbs()
        {
            if (this.m_Toolbar == null) return;
            
            this.m_Toolbar.Clear();
            List<string> pages = this.GraphWindow.Pages;
            EditorToolbarToggle lastBreadcrumb = null;
            
            for (int i = 0; i < pages.Count; ++i)
            {
                lastBreadcrumb = this.CreateBreadcrumb(pages, i);
                this.m_Toolbar.Add(lastBreadcrumb);
            }
            
            this.m_Toolbar.SetupChildrenAsButtonStrip();

            foreach (VisualElement breadcrumb in this.m_Toolbar.Children())
            {
                breadcrumb.style.paddingRight = PADDING_RIGHT;
                breadcrumb.style.paddingLeft = PADDING_LEFT;
            }
            
            if (lastBreadcrumb != null)
            {
                lastBreadcrumb.style.borderBottomRightRadius = EDGE_RADIUS;
                lastBreadcrumb.style.borderTopRightRadius = EDGE_RADIUS;
            }
        }
        
        // PRIVATE CALLBACKS: ---------------------------------------------------------------------

        private void OnChangePage(TGraphTool graphTool, TGraphTool tool)
        {
            this.RefreshBreadcrumbs();
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private EditorToolbarToggle CreateBreadcrumb(IReadOnlyList<string> pages, int index)
        {
            bool isLast = index >= pages.Count - 1;

            EditorToolbarToggle button = new EditorToolbarToggle(pages[index])
            {
                value = isLast,
                icon = null,
                tooltip = $"Go back to {pages[index]}"
            };

            button.RegisterValueChangedCallback(_ =>
            {
                if (isLast)
                {
                    button.SetValueWithoutNotify(true);
                    return;
                }
                
                this.GraphWindow.Backtrack(index + 1);
            });

            return button;
        }
    }
}