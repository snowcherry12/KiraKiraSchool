using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Title("Attributes")]
    [Category("Stats/Attributes")]
    
    [Image(typeof(IconAttr), ColorTheme.Type.Blue)]
    [Description("Remembers the value of the Attributes from the object's Traits component")]

    [Serializable]
    public class MemoryAttributes : Memory
    {
        public override string Title => "Attributes";

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public override Token GetToken(GameObject target)
        {
            Traits traits = target.Get<Traits>();
            return traits != null ? new TokenAttributes(traits) : null;
        }

        public override void OnRemember(GameObject target, Token token)
        {
            Traits traits = target.Get<Traits>();
            if (traits == null) return;
            
            TokenAttributes.OnRemember(traits, token);
        }
    }
}