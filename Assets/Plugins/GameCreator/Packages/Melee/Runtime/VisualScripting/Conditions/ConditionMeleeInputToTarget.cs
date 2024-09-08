using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Melee
{
    [Title("Push to Target")]
    [Description("Returns true if the user is pressing forward towards the Character's Target")]
    [Category("Input/Push to Target")]

    [Image(typeof(IconMeleeSword), ColorTheme.Type.Yellow, typeof(OverlayArrowRight))]
    
    [Serializable]
    public class ConditionMeleeInputToTarget : TConditionMeleeInput
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Direction => "Push to";
        protected override float DirectionSign => 1f;
    }
}
