using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Title("Last Added")]
    [Category("Last Added")]
    
    [Image(typeof(IconStatusEffect), ColorTheme.Type.Green, typeof(OverlayPlus))]
    [Description("A reference to the last Status Effect added to a Traits")]

    [Serializable]
    public class GetStatusEffectLastAdded : PropertyTypeGetStatusEffect
    {
        public override StatusEffect Get(Args args) => StatusEffect.LastAdded;
        public override StatusEffect Get(GameObject gameObject) => StatusEffect.LastAdded;

        public override string String => "Last Added";
    }
}