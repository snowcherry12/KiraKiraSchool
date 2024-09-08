using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    internal abstract class TGraphOverlay : Overlay
    {
        private const string USS_OVERLAY = EditorPaths.PACKAGES + "Behavior/Editor/StyleSheets/Overlay";
        private const string USS_VARS = EditorPaths.PACKAGES + "Behavior/Editor/StyleSheets/Variables";

        protected static readonly Vector2 PANEL_DEF_SIZE = new Vector2(300f, 500f);
        protected static readonly Vector2 PANEL_MIN_SIZE = new Vector2(200f, 200f);
        protected static readonly Vector2 PANEL_MAX_SIZE = new Vector2(1000f, 1000f);
        
        // PROPERTIES: ----------------------------------------------------------------------------

        protected TGraphWindow GraphWindow => this.containerWindow as TGraphWindow;

        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected void SetupStyleSheet(VisualElement root)
        {
            StyleSheet[] styleSheetsCollection = StyleSheetUtils.Load(USS_VARS, USS_OVERLAY);
            foreach (StyleSheet styleSheet in styleSheetsCollection)
            {
                root.styleSheets.Add(styleSheet);
            }
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Show() => this.displayed = true;
        public void Hide() => this.displayed = false;
    }
}