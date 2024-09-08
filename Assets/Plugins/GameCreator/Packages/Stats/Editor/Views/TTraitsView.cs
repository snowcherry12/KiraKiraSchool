using GameCreator.Runtime.Stats;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Stats
{
    public abstract class TTraitsView : VisualElement
    {
        // MEMBERS: -------------------------------------------------------------------------------

        protected readonly Traits m_Traits;
        
        protected readonly VisualElement m_Head;
        protected readonly VisualElement m_Body;
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected abstract string Label { get; }

        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        protected TTraitsView(Traits traits)
        {
            this.m_Traits = traits;
            
            this.m_Head = new VisualElement();
            this.m_Body = new VisualElement();

            Label title = new Label
            {
                name = TOverrideDrawer.NAME_TITLE,
                text = this.Label
            };
            
            this.m_Head.Add(title);
            this.Rebuild();
            
            this.Add(this.m_Head);
            this.Add(this.m_Body);
        }
        
        // ABSTRACT METHODS: ----------------------------------------------------------------------

        protected abstract void Rebuild();
    }
}