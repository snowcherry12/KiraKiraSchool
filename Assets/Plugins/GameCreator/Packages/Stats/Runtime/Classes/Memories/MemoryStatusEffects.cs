using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Title("Status Effects")]
    [Category("Stats/Status Effects")]
    
    [Image(typeof(IconStatusEffect), ColorTheme.Type.Green)]
    [Description("Remembers the Status Effects amount and their remaining time")]

    [Serializable]
    public class MemoryStatusEffects : Memory
    {
        public override string Title => "Status Effects";

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public override Token GetToken(GameObject target)
        {
            Traits traits = target.Get<Traits>();
            return traits != null ? new TokenStatusEffects(traits) : null;
        }

        public override void OnRemember(GameObject target, Token token)
        {
            Traits traits = target.Get<Traits>();
            if (traits == null) return;
            
            TokenStatusEffects.OnRemember(traits, token);
        }
    }
}