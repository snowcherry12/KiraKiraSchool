using GameCreator.Runtime.Common;
using GameCreator.Runtime.Stats;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Stats
{
    public class AttributesView : TTraitsView
    {
        private const int ATTRIBUTE_SIZE = 150;
        
        private const string NAME_ELEM_ROOT = "GC-Stats-Traits-Attribute-Root";
        private const string NAME_ELEM_LABEL = "GC-Stats-Traits-Attribute-Label";
        private const string NAME_ELEM_PROGRESS_BKG = "GC-Stats-Traits-Attribute-Bck";
        private const string NAME_ELEM_PROGRESS_BAR = "GC-Stats-Traits-Attribute-Bar";
        
        // PROPERTIES: ----------------------------------------------------------------------------

        protected override string Label => "Attributes";

        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public AttributesView(Traits traits) : base(traits)
        {
            this.m_Traits.EventChange -= this.Rebuild;
            this.m_Traits.EventChange += this.Rebuild;
        }
        
        ~AttributesView()
        {
            if (this.m_Traits == null) return;
            this.m_Traits.EventChange -= this.Rebuild;
        }
        
        // IMPLEMENTATIONS: -----------------------------------------------------------------------
        
        protected override void Rebuild()
        {
            this.m_Body.Clear();

            Class traitsClass = this.m_Traits.Class;
            
            for (int i = 0; i < traitsClass.AttributesLength; i++)
            {
                AttributeItem attrItem = traitsClass.GetAttribute(i);
                if (attrItem == null || attrItem.Attribute == null) continue;
                
                RuntimeAttributeData attrData = this.m_Traits
                    .RuntimeAttributes
                    .Get(attrItem.Attribute.ID);
                
                if (attrData == null) continue;

                VisualElement root = new VisualElement { name = NAME_ELEM_ROOT };
                
                VisualElement progressBackground = new VisualElement { name = NAME_ELEM_PROGRESS_BKG };
                VisualElement progressBar = new VisualElement { name = NAME_ELEM_PROGRESS_BAR };
                
                progressBackground.Add(progressBar);

                double value = attrData.Value;
                double total = attrData.MaxValue - attrData.MinValue;
                
                double percent = MathUtils.Clamp01(total <= 0f ? 0f : value / total);

                progressBackground.style.flexBasis = new StyleLength(ATTRIBUTE_SIZE);
                progressBar.style.width = new StyleLength(ATTRIBUTE_SIZE * (float) percent);
                progressBar.style.backgroundColor = new StyleColor(attrItem.Attribute.Color);

                int roundValue = (int) value;
                int roundTotal = (int) total;
                
                Label title = new Label
                {
                    name = NAME_ELEM_LABEL,
                    text = $"<b>{TextUtils.Humanize(attrItem.Attribute.ID.String)}:</b> {roundValue} / {roundTotal}"
                };

                root.Add(progressBackground);
                root.Add(title);

                this.m_Body.Add(root);
            }
        }
    }
}