using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Title("Target to Self Location")]
    [Category("Melee/Target to Self Location")]

    [Image(typeof(IconSelf), ColorTheme.Type.Green)]
    [Description("The position and rotation of the Target towards Self")]

    [Serializable]
    public class GetLocationMeleeTargetToSelf : TGetLocationTrackLocation
    {
        protected override GameObject GetFrom(Args args)
        {
            return args.Target;
        }

        protected override GameObject GetTo(Args args)
        {
            return args.Self;
        }

        public static PropertyGetLocation Create => new PropertyGetLocation(
            new GetLocationMeleeTargetToSelf()
        );
        
        public override string String => "Target to Self";
    }
}