using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Title("None")]
    [Category("None")]
    [Description("Don't save on anything")]
    
    [Image(typeof(IconNull), ColorTheme.Type.TextLight)]

    [Serializable]
    public class SetStatusEffectNone : PropertyTypeSetStatusEffect
    {
        public override void Set(StatusEffect value, Args args)
        { }
        
        public override void Set(StatusEffect value, GameObject gameObject)
        { }

        public static PropertySetStatusEffect Create => new PropertySetStatusEffect(
            new SetStatusEffectNone()
        );

        public override string String => "(none)";
    }
}