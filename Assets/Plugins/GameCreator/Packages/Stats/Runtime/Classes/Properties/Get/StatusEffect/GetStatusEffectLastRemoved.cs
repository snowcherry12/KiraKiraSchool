using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Title("Last Removed")]
    [Category("Last Removed")]
    
    [Image(typeof(IconStatusEffect), ColorTheme.Type.Green, typeof(OverlayMinus))]
    [Description("A reference to the last Status Effect removed from a Traits")]

    [Serializable]
    public class GetStatusEffectLastRemoved : PropertyTypeGetStatusEffect
    {
        public override StatusEffect Get(Args args) => StatusEffect.LastRemoved;
        public override StatusEffect Get(GameObject gameObject) => StatusEffect.LastRemoved;

        public override string String => "Last Removed";
    }
}