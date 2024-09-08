using System;
using System.Collections.Generic;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Perception;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Perception
{
    public class PerceptionView : VisualElement
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [NonSerialized] private readonly Runtime.Perception.Perception m_Perception;

        [NonSerialized] private readonly LabelTitle m_AwarenessTitle;
        [NonSerialized] private readonly List<AwarenessView> m_AwarenessViews;

        [NonSerialized] private readonly VisualElement m_Awareness;
        
        // INITIALIZERS: --------------------------------------------------------------------------
        
        public PerceptionView(Runtime.Perception.Perception perception)
        {
            this.m_Perception = perception;
            this.m_AwarenessViews = new List<AwarenessView>();

            this.m_AwarenessTitle = new LabelTitle("Awareness:");
            this.m_Awareness = new VisualElement();
            
            this.Add(this.m_AwarenessTitle);
            this.Add(this.m_Awareness);
            
            this.RegisterCallback<AttachToPanelEvent>(this.OnEnable);
            this.RegisterCallback<DetachFromPanelEvent>(this.OnDisable);
            
            this.RefreshAwareness();
        }

        private void OnEnable(AttachToPanelEvent eventAttach)
        {
            this.m_Perception.EventAwarenessChange += this.RefreshAwareness;
            this.RefreshAwareness();
        }

        private void OnDisable(DetachFromPanelEvent eventDetach)
        {
            this.m_Perception.EventAwarenessChange -= this.RefreshAwareness;
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private void RefreshAwareness()
        {
            this.m_Awareness.Clear();
            if (this.m_Perception == null) return;
            
            List<Tracker> awarenessList = this.m_Perception.TrackerList;
            
            for (int i = this.m_AwarenessViews.Count; i < awarenessList.Count; ++i)
            {
                AwarenessView awarenessView = new AwarenessView();
                this.m_AwarenessViews.Add(awarenessView);
            }

            this.m_AwarenessTitle.style.display = awarenessList.Count > 0
                ? DisplayStyle.Flex
                : DisplayStyle.None;
            
            for (int i = 0; i < awarenessList.Count; i++)
            {
                Tracker tracker = awarenessList[i];
                if (tracker.Target == null) continue;
                
                AwarenessView awarenessView = this.m_AwarenessViews[i];
                awarenessView.Refresh(tracker);
                
                this.m_Awareness.Add(awarenessView);
            }
        }
    }
}