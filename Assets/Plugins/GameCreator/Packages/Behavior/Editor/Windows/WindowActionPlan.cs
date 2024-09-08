using System;
using System.Collections.Generic;
using GameCreator.Runtime.Behavior;
using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEngine;

namespace GameCreator.Editor.Behavior
{
    internal class WindowActionPlan : TGraphWindow
    {
        private const string USS_ACTION_PLAN = USS_PATH + "ActionPlan";
        
        // PROPERTIES: ----------------------------------------------------------------------------

        protected override string WindowTitle => "Action Planner";
        protected override Texture WindowIcon => new IconWindowActionPlan(ColorTheme.Type.TextLight).Texture;
        
        public override string AssetName => "Action Plan";
        public override Type AssetType => typeof(ActionPlan);

        protected override IEnumerable<string> ExtraStyleSheets => new[]
        {
            USS_ACTION_PLAN
        };

        // INITIALIZERS: --------------------------------------------------------------------------
        
        [MenuItem("Window/Game Creator/Behavior/Action Plan")]
        public static void Open()
        {
            Graph asset = UnityEditor.Selection.activeObject as Graph;
            Open(asset as ActionPlan);
        }

        public static void Open(ActionPlan actionPlan)
        {
            SetupWindow<WindowActionPlan>(actionPlan);
        }
        
        // OVERRIDE METHODS: ----------------------------------------------------------------------
        
        protected override TGraphTool CreateGraphTool(Graph graph)
        {
            return new ToolActionPlan(graph as ActionPlan, this);
        }

        protected override Graph CreateAsset()
        {
            return CreateInstance<ActionPlan>();
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
            
            RestoreUtils.UpdateActionPlan(paths);
        }

        protected override void OnChangePlayMode(PlayModeStateChange stateChange)
        {
            if (stateChange != PlayModeStateChange.EnteredEditMode) return;
            string[] paths = RestoreUtils.ActionPlan;

            foreach (string path in paths)
            {
                Graph asset = AssetDatabase.LoadAssetAtPath<Graph>(path);
                if (asset == null) continue;
                
                this.NewPage(asset, true);
            }
        }
    }
}