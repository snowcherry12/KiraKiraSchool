using System;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    internal abstract class TBlackboard : TGraphOverlay
    {
        protected const string NAME = "Blackboard";
        protected const string ID = "GC-Overlay-Graph-Blackboard";

        private const string NAME_ROOT = "GC-Overlay-Graph-Blackboard-Root";

        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private VisualElement m_Root;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override Layout supportedLayouts => Layout.Panel;
        
        protected abstract string Title { get; }
        
        // EVENTS: --------------------------------------------------------------------------------

        public event Action EventChange;
        
        // INITIALIZERS: --------------------------------------------------------------------------

        public override void OnCreated()
        {
            base.OnCreated();
            this.GraphWindow.Overlays.Blackboard = this;
            this.GraphWindow.EventChangePage += this.OnChangePage;
        }

        public override VisualElement CreatePanelContent()
        {
            this.m_Root = new VisualElement { name = NAME_ROOT };
            SetupStyleSheet(this.m_Root);

            this.Refresh();
            return this.m_Root;
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Refresh()
        {
            if (this.m_Root == null) return;
            this.m_Root.Clear();
            
            if (this.GraphWindow.CurrentPage == null) return;

            SerializedObject serializedObject = this.GraphWindow.CurrentPage.SerializedObject;
            serializedObject.Update();
            
            SerializedProperty data = serializedObject.FindProperty("m_Data");
            PropertyField fieldData = new PropertyField(data);
            
            fieldData.Bind(serializedObject);
            fieldData.RegisterValueChangeCallback(this.OnChange);
            
            this.m_Root.Add(fieldData);
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void OnChange(SerializedPropertyChangeEvent eventChange)
        {
            this.EventChange?.Invoke();
        }

        private void OnChangePage(TGraphTool prevGraphTool, TGraphTool nextGraphTool)
        {
            this.Refresh();
        }
    }
}