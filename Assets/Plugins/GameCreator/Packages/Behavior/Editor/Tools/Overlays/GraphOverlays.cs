using System;

namespace GameCreator.Editor.Behavior
{
    internal class GraphOverlays
    { 
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private TToolbar m_Toolbar;
        [NonSerialized] private TPanel m_Panel;
        [NonSerialized] private TBreadcrumb m_Breadcrumb;

        [NonSerialized] private TBlackboard m_Blackboard;
        [NonSerialized] private TInspector m_Inspector;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public TToolbar Toolbar
        {
            get => this.m_Toolbar;
            set
            {
                if (this.m_Toolbar != null)
                {
                    this.m_Toolbar.displayedChanged -= this.OnChangeDisplay;
                }
                
                this.m_Toolbar = value;
                this.m_Toolbar.displayedChanged += this.OnChangeDisplay;
            }
        }

        public TPanel Panel
        {
            get => this.m_Panel;
            set
            {
                if (this.m_Panel != null)
                {
                    this.m_Panel.displayedChanged -= this.OnChangeDisplay;
                }
                
                this.m_Panel = value;
                this.m_Panel.displayedChanged += this.OnChangeDisplay;
            }
        }
        
        public TBreadcrumb Breadcrumb
        {
            get => this.m_Breadcrumb;
            set
            {
                if (this.m_Breadcrumb != null)
                {
                    this.m_Breadcrumb.displayedChanged -= this.OnChangeDisplay;
                }
                
                this.m_Breadcrumb = value;
                this.m_Breadcrumb.displayedChanged += this.OnChangeDisplay;
            }
        }
        
        public TBlackboard Blackboard
        {
            get => this.m_Blackboard;
            set
            {
                if (this.m_Blackboard != null)
                {
                    this.m_Blackboard.displayedChanged -= this.OnChangeDisplay;
                }
                
                this.m_Blackboard = value;
                this.m_Blackboard.displayedChanged += this.OnChangeDisplay;
            }
        }
        
        public TInspector Inspector
        {
            get => this.m_Inspector;
            set
            {
                if (this.m_Inspector != null)
                {
                    this.m_Inspector.displayedChanged -= this.OnChangeDisplay;
                }
                
                this.m_Inspector = value;
                this.m_Inspector.displayedChanged += this.OnChangeDisplay;
            }
        }
        
        // EVENTS: --------------------------------------------------------------------------------

        public event Action EventDisplay;

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Show()
        {
            this.Toolbar?.Show();
            this.Panel?.Show();
            this.Breadcrumb?.Show();
            
            this.Blackboard?.Show();
            this.Inspector?.Show();
        }

        public void ShowRequired()
        {
            this.Toolbar?.Show();
            this.Panel?.Show();
        }
        
        public void Hide()
        {
            this.Toolbar?.Hide();
            this.Panel?.Hide();
            this.Breadcrumb?.Hide();
            
            this.Blackboard?.Hide();
            this.Inspector?.Hide();
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void OnChangeDisplay(bool display)
        {
            EventDisplay?.Invoke();
        }
    }
}