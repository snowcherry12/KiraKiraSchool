using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Stats;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Stats
{
    public class StatusEffectsView : TTraitsView
    {
        private static readonly IIcon ICON_STATUS_EFFECT = new IconStatusEffect(Color.white);

        private const string NAME_STATUS_EFFECTS_ROW = "GC-Stats-Traits-Status-Effects-Row";
        private const string NAME_STATUS_EFFECTS_CONTENT = "GC-Stats-Traits-Status-Effects-Content";

        private static readonly Color COLOR_DEFAULT = ColorTheme.Get(ColorTheme.Type.Green);
        
        // PROPERTIES: ----------------------------------------------------------------------------

        protected override string Label => "Status Effects";

        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public StatusEffectsView(Traits traits) : base(traits)
        {
            this.m_Traits.EventChange -= this.Rebuild;
            this.m_Traits.EventChange += this.Rebuild;
        }
        
        ~StatusEffectsView()
        {
            if (this.m_Traits == null) return;
            this.m_Traits.EventChange -= this.Rebuild;
        }
        
        // IMPLEMENTATIONS: -----------------------------------------------------------------------
        
        protected override void Rebuild()
        {
            this.m_Body.Clear();

            List<IdString> actives = this.m_Traits.RuntimeStatusEffects.GetActiveList();
            
            actives.Sort(SortStatusEffects);
            bool noActiveStatusEffects = true; 

            foreach (IdString activeID in actives)
            {
                int count = this.m_Traits.RuntimeStatusEffects.GetActiveStackCount(activeID);
                if (count == 0) continue;
                
                noActiveStatusEffects = false;
                
                StatusEffect statusEffect = this.m_Traits
                    .RuntimeStatusEffects
                    .GetActiveStatusEffect(activeID);

                Color tint = statusEffect != null ? statusEffect.Color : COLOR_DEFAULT;
                this.CreateActiveStatusEffect(activeID.String, count, tint);
            }

            if (noActiveStatusEffects)
            {
                this.CreateActiveStatusEffect("(None)", 0, Color.white);
            }
        }

        private void CreateActiveStatusEffect(string name, int count, Color tint)
        {
            VisualElement row = new VisualElement { name = NAME_STATUS_EFFECTS_ROW };
            VisualElement field = new VisualElement { name = NAME_STATUS_EFFECTS_CONTENT };
                
            row.Add(new Label(TextUtils.Humanize(name)));
            row.Add(field);
            
            for (int i = 0; i < count; ++i)
            {
                Image icon = new Image
                {
                    image = ICON_STATUS_EFFECT.Texture,
                    tintColor = tint
                };
                    
                field.Add(icon);
            }
                
            this.m_Body.Add(row);
        }

        // STATIC METHODS: ------------------------------------------------------------------------
        
        private static int SortStatusEffects(IdString x, IdString y)
        {
            return string.Compare(x.String, y.String, StringComparison.Ordinal);
        }
    }
}