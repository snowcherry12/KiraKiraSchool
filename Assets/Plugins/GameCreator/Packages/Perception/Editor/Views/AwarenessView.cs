using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Perception;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Perception
{
    public class AwarenessView : VisualElement
    {
        private const string NAME_PROGRESS = "GC-Perception-Awareness-View-Progress";
        private const string NAME_BAR = "GC-Perception-Awareness-View-Bar";

        private const float BAR_SIZE = 80f;
        
        // MEMBERS: -------------------------------------------------------------------------------

        private readonly VisualElement m_Bar = new VisualElement { name = NAME_BAR };
        private readonly Label m_Text = new Label();
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public AwarenessView()
        {
            VisualElement progress = new VisualElement
            {
                name = NAME_PROGRESS, 
                style = { width = BAR_SIZE }
            };
            
            progress.Add(this.m_Bar);
            
            this.Add(progress);
            this.Add(this.m_Text);
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Refresh(Tracker tracker)
        {
            this.m_Text.text = $"{tracker.Target.name}: {tracker.Awareness:0.000}";
            this.m_Bar.style.width = BAR_SIZE * tracker.Awareness;

            this.m_Bar.style.backgroundColor = Tracker.GetStage(tracker.Awareness) switch
            {
                AwareStage.None => ColorTheme.Get(ColorTheme.Type.TextLight),
                AwareStage.Suspicious => ColorTheme.Get(ColorTheme.Type.Yellow),
                AwareStage.Alert => ColorTheme.Get(ColorTheme.Type.Red),
                AwareStage.Aware => ColorTheme.Get(ColorTheme.Type.Green),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}