using System;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Behavior;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    internal class GraphOnboarding : VisualElement
    {
        private const string NAME_ONBOARDING_CONTENT = "GC-Graph-Onboarding-Content";
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private readonly TGraphWindow m_Window;
        
        [NonSerialized] private readonly Button m_ButtonCreateAsset;
        [NonSerialized] private readonly Button m_ButtonOpenSelection;
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public GraphOnboarding(TGraphWindow window)
        {
            this.m_Window = window;
            
            VisualElement content = new VisualElement { name = NAME_ONBOARDING_CONTENT };
            
            this.Add(new FlexibleSpace());
            this.Add(content);
            this.Add(new FlexibleSpace());
            
            this.m_ButtonCreateAsset = new Button(window.CreateOpenAsset)
            {
                text = $"Create {window.AssetName}"
            };

            this.m_ButtonOpenSelection = new Button(this.OpenSelection);
            
            content.Add(this.m_ButtonCreateAsset);
            content.Add(new TextSeparator("or"));
            content.Add(this.m_ButtonOpenSelection);

            UnityEditor.Selection.selectionChanged += this.RefreshSelection;
            this.RefreshSelection();
        }

        private void RefreshSelection()
        {
            Graph selection = UnityEditor.Selection.activeObject as Graph;
            
            if (selection != null && selection.GetType() == this.m_Window.AssetType)
            {
                this.m_ButtonOpenSelection.text = $"Open {selection.name}";
            }
            else
            {
                this.m_ButtonOpenSelection.text = "Open...";
            }
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void OpenSelection()
        {
            Graph selection = UnityEditor.Selection.activeObject as Graph;
            if (selection != null)
            {
                this.m_Window.NewPage(selection, false);
                return;
            }

            string path = EditorUtility.OpenFilePanel(
                $"Open {this.m_Window.AssetName}", 
                "Assets/", 
                "asset"
            );
            
            path = path.Replace(Application.dataPath, "Assets");
            if (path.Length == 0) return;

            Graph asset = AssetDatabase.LoadAssetAtPath<Graph>(path);
            if (asset == null || asset.GetType() != this.m_Window.AssetType) return;

            this.m_Window.NewPage(asset, false);
        }
    }
}