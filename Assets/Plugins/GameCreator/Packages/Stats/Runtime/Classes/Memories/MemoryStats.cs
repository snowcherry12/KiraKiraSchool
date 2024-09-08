using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Title("Stats")]
    [Category("Stats/Stats")]
    
    [Image(typeof(IconStat), ColorTheme.Type.Red)]
    [Description("Remembers the base value of the Stats from the object's Traits component")]

    [Serializable]
    public class MemoryStats : Memory
    {
        public override string Title => "Stats";

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public override Token GetToken(GameObject target)
        {
            Traits traits = target.Get<Traits>();
            return traits != null ? new TokenStats(traits) : null;
        }

        public override void OnRemember(GameObject target, Token token)
        {
            Traits traits = target.Get<Traits>();
            if (traits == null) return;
            
            TokenStats.OnRemember(traits, token);
        }
    }
}