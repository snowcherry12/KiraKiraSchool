using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Melee
{
    [Title("Pull from Target")]
    [Description("Returns true if the user is pressing backwards against the Character's Target")]
    [Category("Input/Pull from Target")]

    [Image(typeof(IconMeleeSword), ColorTheme.Type.Yellow, typeof(OverlayArrowLeft))]
    
    [Serializable]
    public class ConditionMeleeInputAgainstTarget : TConditionMeleeInput
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Direction => "Pull from";
        protected override float DirectionSign => -1f;
    }
}
