using System;
using System.Collections.Generic;
using GameCreator.Runtime.Behavior;
using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEngine;

namespace GameCreator.Editor.Behavior
{
    internal class WindowBehaviorTree : TGraphWindow
    {
        private const string USS_BEHAVIOR_TREE = USS_PATH + "BehaviorTree";
        
        // PROPERTIES: ----------------------------------------------------------------------------

        protected override string WindowTitle => "Behavior Tree";
        protected override Texture WindowIcon => new IconWindowBehaviorTree(ColorTheme.Type.TextLight).Texture;
        
        public override string AssetName => "Behavior Tree";
        public override Type AssetType => typeof(BehaviorTree);

        protected override IEnumerable<string> ExtraStyleSheets => new[]
        {
            USS_BEHAVIOR_TREE
        };

        // INITIALIZERS: --------------------------------------------------------------------------
        
        [MenuItem("Window/Game Creator/Behavior/Behavior Tree")]
        public static void Open()
        {
            Graph asset = UnityEditor.Selection.activeObject as Graph;
            Open(asset as BehaviorTree);
        }

        public static void Open(BehaviorTree behaviorTree)
        {
            SetupWindow<WindowBehaviorTree>(behaviorTree);
        }
        
        // OVERRIDE METHODS: ----------------------------------------------------------------------
        
        protected override TGraphTool CreateGraphTool(Graph graph)
        {
            return new ToolBehaviorTree(graph as BehaviorTree, this);
        }

        protected override Graph CreateAsset()
        {
            return CreateInstance<BehaviorTree>();
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
            
            RestoreUtils.UpdateBehaviorTree(paths);
        }
        
        protected override void OnChangePlayMode(PlayModeStateChange stateChange)
        {
            if (stateChange != PlayModeStateChange.EnteredEditMode) return;
            string[] paths = RestoreUtils.BehaviorTree;

            for (int i = 0; i < paths.Length; i++)
            {
                string path = paths[i];
                Graph asset = AssetDatabase.LoadAssetAtPath<Graph>(path);
                if (asset == null) continue;

                this.NewPage(asset, i != 0);
            }
        }
    }
}