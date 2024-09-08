using System;
using System.Collections.Generic;
using System.Linq;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Behavior;
using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    internal abstract class TGraphWindow : EditorWindow, ISupportsOverlays
    {
        protected const string USS_PATH = EditorPaths.PACKAGES + "Behavior/Editor/StyleSheets/";
        
        private static readonly string[] DEFAULT_STYLESHEETS = 
        {
            USS_PATH + "Variables",
            USS_PATH + "Graph",
            USS_PATH + "Node",
            USS_PATH + "Port",
            USS_PATH + "Onboarding"
        };

        private const string NAME_GRAPH_CONTENT = "GC-Graph-Content";

        private static TGraphWindow Window;
        
        private const int MIN_WIDTH = 800;
        private const int MIN_HEIGHT = 600;

        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private VisualElement m_Content;
        [NonSerialized] private GraphOnboarding m_Onboarding;

        [NonSerialized] protected readonly Stack<TGraphTool> m_Pages = new Stack<TGraphTool>();

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected abstract string WindowTitle { get; }
        protected abstract Texture WindowIcon { get; }
        
        public abstract string AssetName { get; }
        public abstract Type  AssetType { get; }

        protected abstract IEnumerable<string> ExtraStyleSheets { get; }

        public GraphOverlays Overlays { get; } = new GraphOverlays();

        public TGraphTool CurrentPage => this.m_Pages.TryPeek(out TGraphTool page)
            ? page
            : null;
        
        public List<string> Pages
        {
            get
            {
                List<string> list = new List<string>();
                foreach (TGraphTool page in this.m_Pages)
                {
                    string assetName = page.Graph != null ? page.Graph.name : "(none)";
                    list.Add(assetName);
                }

                list.Reverse();
                return list;
            }
        }

        public TGraphTool FirstPage => this.m_Pages.Count != 0 ? this.m_Pages.First() : null; 

        [field: NonSerialized] public Selection Selection { get; } = new Selection();

        // EVENTS: --------------------------------------------------------------------------------

        public static event Action EventChangeBlackboard;
        public static event Action EventChangeInspector;
        
        public event Action<TGraphTool, TGraphTool> EventChangePage;

        // STATIC METHODS: ------------------------------------------------------------------------
        
        protected static void SetupWindow<T>(Graph graph = null) where T : TGraphWindow
        {
            Window = GetWindow<T>();
            Window.minSize = new Vector2(MIN_WIDTH, MIN_HEIGHT);
            
            if (graph != null) Window.NewPage(graph, false);
            else Window.NewOnboarding();
        }
        
        // INITIALIZERS: --------------------------------------------------------------------------

        private void OnEnable()
        {
            if (Window == null)
            {
                Window = this;
                Window.minSize = new Vector2(MIN_WIDTH, MIN_HEIGHT);
            }

            this.titleContent = new GUIContent(this.WindowTitle, this.WindowIcon);

            this.m_Content = new VisualElement { name = NAME_GRAPH_CONTENT };
            this.m_Onboarding = new GraphOnboarding(this);

            this.rootVisualElement.Add(this.m_Content);
            this.rootVisualElement.Add(this.m_Onboarding);

            string[] paths = DEFAULT_STYLESHEETS.Concat(this.ExtraStyleSheets).ToArray();
            StyleSheet[] styleSheetsCollection = StyleSheetUtils.Load(paths);
            
            foreach (StyleSheet styleSheet in styleSheetsCollection)
            {
                this.rootVisualElement.styleSheets.Add(styleSheet);
            }

            this.EventChangePage += this.OnChangePage;

            EditorApplication.playModeStateChanged -= this.OnChangePlayMode;
            EditorApplication.playModeStateChanged += this.OnChangePlayMode;

            this.Overlays.Blackboard.EventChange -= OnChangeBlackboard;
            this.Overlays.Blackboard.EventChange += OnChangeBlackboard;
            
            this.Overlays.Inspector.EventChange -= OnChangeInspector;
            this.Overlays.Inspector.EventChange += OnChangeInspector;

            this.rootVisualElement.RegisterCallback<PointerEnterEvent>(this.OnHoverWindow);
            this.rootVisualElement.RegisterCallback<ValidateCommandEvent>(this.OnValidateCommand);
            this.rootVisualElement.RegisterCallback<ExecuteCommandEvent>(this.OnExecuteCommand);

            this.OnHoverWindow(null);
            this.NewOnboarding();
        }

        private void OnDisable()
        {
            this.EventChangePage -= this.OnChangePage;
            
            EditorApplication.playModeStateChanged -= this.OnChangePlayMode;

            if (this.Overlays?.Blackboard != null) this.Overlays.Blackboard.EventChange -= OnChangeBlackboard;
            if (this.Overlays?.Inspector != null) this.Overlays.Inspector.EventChange -= OnChangeInspector;

            this.rootVisualElement.UnregisterCallback<PointerEnterEvent>(this.OnHoverWindow);
            this.rootVisualElement.UnregisterCallback<ValidateCommandEvent>(this.OnValidateCommand);
            this.rootVisualElement.UnregisterCallback<ExecuteCommandEvent>(this.OnExecuteCommand);

            this.Unstack(0);
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void NewPage(Graph graph, bool stack)
        {
            if (graph == null) return;
            TGraphTool previous = this.CurrentPage;

            this.m_Content.style.display = DisplayStyle.Flex;
            this.m_Onboarding.style.display = DisplayStyle.None;

            TGraphTool graphTool = this.CreateGraphTool(graph);

            if (stack && this.m_Pages.Count >= 1)
            {
                TGraphTool topGraphTool = this.m_Pages.Peek();
                topGraphTool?.OnDisable();
            }
            else
            {
                this.Unstack(0);
            }
            
            this.m_Pages.Push(graphTool);
            this.m_Content.Add(graphTool);
            
            graphTool.OnEnable();
            this.Overlays.ShowRequired();
            
            this.EventChangePage?.Invoke(previous, graphTool);
        }

        public void NewOnboarding()
        {
            TGraphTool previous = this.CurrentPage;
            this.Unstack(0);
            
            this.m_Content.style.display = DisplayStyle.None;
            this.m_Onboarding.style.display = DisplayStyle.Flex;
            
            this.Overlays.Hide();
            this.EventChangePage?.Invoke(previous, null);
        }

        public void CreateOpenAsset()
        {
            string path = EditorUtility.SaveFilePanelInProject(
                $"Create {this.AssetName}",
                $"My {this.AssetName}",
                "asset",
                string.Empty
            );

            if (path.Length == 0) return;
            
            Graph asset = this.CreateAsset();
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.Refresh();
            
            this.NewPage(asset, false);
        }

        public void Backtrack(int keepAmount)
        {
            TGraphTool previous = this.CurrentPage;
            this.Unstack(keepAmount);
            
            if (this.m_Pages.Count != 0)
            {
                this.EventChangePage?.Invoke(previous, this.CurrentPage);
                return;
            }
            
            this.NewOnboarding();
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void Unstack(int keepCount)
        {
            int removeCount = this.m_Pages.Count - keepCount;
            for (int i = 0; i < removeCount; ++i)
            {
                if (this.m_Pages.TryPop(out TGraphTool graphTool))
                {
                    graphTool.OnDisable();
                    this.m_Content.Remove(graphTool);   
                }
            }

            if (this.m_Pages.Count == 0) return;
            
            TGraphTool topGraphTool = this.m_Pages.Peek();
            topGraphTool?.OnEnable();
        }
        
        // PRIVATE CALLBACKS: ---------------------------------------------------------------------
        
        private void OnChangePage(TGraphTool prevGraphTool, TGraphTool nextGraphTool)
        {
            this.Selection.Clear();
            this.AfterChangePages();
        }
        
        private static void OnChangeBlackboard()
        {
            EventChangeBlackboard?.Invoke();
        }

        private static void OnChangeInspector()
        {
            EventChangeInspector?.Invoke();
        }
        
        private void OnHoverWindow(PointerEnterEvent eventPointerEnter)
        {
            this.rootVisualElement.pickingMode = PickingMode.Position;
            this.rootVisualElement.focusable = true;
            this.rootVisualElement.Focus();
        }
        
        private void OnValidateCommand(ValidateCommandEvent eventValidate)
        {
            
            if (this.CurrentPage?.OnValidateCommand(eventValidate.commandName) ?? false)
            {
                eventValidate.StopPropagation();
            }
        }
        
        private void OnExecuteCommand(ExecuteCommandEvent eventExecute)
        {
            ArgData argData = new ArgData(
                Vector2.zero,
                Vector2.zero,
                string.Empty, 
                null
            );
            
            if (this.CurrentPage?.OnExecuteCommand(eventExecute.commandName, argData) ?? false)
            {
                eventExecute.StopPropagation();
            }
        }

        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected abstract TGraphTool CreateGraphTool(Graph graph);
        protected abstract Graph CreateAsset();

        protected abstract void AfterChangePages();
        protected abstract void OnChangePlayMode(PlayModeStateChange stateChange);
    }
}