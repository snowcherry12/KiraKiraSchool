using System;
using GameCreator.Runtime.Behavior;
using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    internal abstract class TToolbar :
        TGraphOverlay, 
        ICreateHorizontalToolbar,
        ICreateVerticalToolbar
    {
        protected const string NAME = "Toolbar";
        protected const string ID = "GC-Overlay-Graph-Toolbar";
        
        private const string NAME_ROOT = "GC-Overlay-Graph-Toolbar-Root";

        private const string KEY_SNAP = "gc:behavior:graph-snap-to-grid";
        
        private static readonly IIcon ICON_FRAME = new IconFrame(ColorTheme.Type.TextNormal);
        
        private static readonly IIcon ICON_SNAP_ON = new IconGridOn(ColorTheme.Type.TextNormal);
        private static readonly IIcon ICON_SNAP_OFF = new IconGridOff(ColorTheme.Type.TextLight);
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] protected OverlayToolbar m_Toolbar;
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override Layout supportedLayouts => Layout.HorizontalToolbar |
                                                      Layout.VerticalToolbar;

        public static bool Snap
        {
            get => EditorPrefs.GetBool(KEY_SNAP, false);
            private set => EditorPrefs.SetBool(KEY_SNAP, value);
        }
        
        // INITIALIZERS: --------------------------------------------------------------------------

        public override void OnCreated()
        {
            base.OnCreated();
            this.GraphWindow.Overlays.Toolbar = this;
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
            EditorToolbarButton frameDefault = new EditorToolbarButton(
                string.Empty,
                ICON_FRAME.Texture,
                this.Frame
            )
            {
                tooltip = "Focus on selection [F]"
            };
            
            EditorToolbarToggle toggleSnap = new EditorToolbarToggle(
                string.Empty,
                ICON_SNAP_ON.Texture,
                ICON_SNAP_OFF.Texture
            )
            {
                value = Snap,
                tooltip = "Toggle snapping to grid"
            };

            toggleSnap.RegisterValueChangedCallback(ToggleSnap);

            this.m_Toolbar.Add(frameDefault);
            this.m_Toolbar.Add(toggleSnap);
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private void Frame()
        {
            TGraphTool currentPage = this.GraphWindow.CurrentPage;
            currentPage?.FrameSelection();
        }

        private static void ToggleSnap(ChangeEvent<bool> eventChange)
        {
            Snap = !Snap;
        }
    }
}