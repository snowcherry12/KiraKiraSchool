using System;
using System.Collections.Generic;
using GameCreator.Runtime.Behavior;
using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEngine;

namespace GameCreator.Editor.Behavior
{
    internal class WindowUtilityBoard : TGraphWindow
    {
        private const string USS_UTILITY_BOARD = USS_PATH + "UtilityBoard";
        
        // PROPERTIES: ----------------------------------------------------------------------------

        protected override string WindowTitle => "Utility Board";
        protected override Texture WindowIcon => new IconWindowUtilityBoard(ColorTheme.Type.TextLight).Texture;
        
        public override string AssetName => "Utility Board";
        public override Type AssetType => typeof(UtilityBoard);

        protected override IEnumerable<string> ExtraStyleSheets => new[]
        {
            USS_UTILITY_BOARD
        };

        // INITIALIZERS: --------------------------------------------------------------------------
        
        [MenuItem("Window/Game Creator/Behavior/Utility Board")]
        public static void Open()
        {
            Graph asset = UnityEditor.Selection.activeObject as Graph;
            Open(asset as UtilityBoard);
        }

        public static void Open(UtilityBoard utilityBoard)
        {
            SetupWindow<WindowUtilityBoard>(utilityBoard);
        }
        
        // OVERRIDE METHODS: ----------------------------------------------------------------------
        
        protected override TGraphTool CreateGraphTool(Graph graph)
        {
            return new ToolUtilityBoard(graph as UtilityBoard, this);
        }

        protected override Graph CreateAsset()
        {
            return CreateInstance<UtilityBoard>();
        }
        
        protected override void AfterChangePages()
        {
            List<string> paths = new List<string>();
            foreach (TGraphTool page in this.m_Pages)
            {
                if (page.Graph == null) continue;
                string path = AssetDatabase.GetAssetPath(page.Graph);
                
                paths.Add(path);
            }
            
            RestoreUtils.UpdateUtilityBoard(paths);
        }
        
        protected override void OnChangePlayMode(PlayModeStateChange stateChange)
        {
            if (stateChange != PlayModeStateChange.EnteredEditMode) return;
            string[] paths = RestoreUtils.UtilityBoard;

            foreach (string path in paths)
            {
                Graph asset = AssetDatabase.LoadAssetAtPath<Graph>(path);
                if (asset == null) continue;
                
                this.NewPage(asset, true);
            }
        }
    }
}