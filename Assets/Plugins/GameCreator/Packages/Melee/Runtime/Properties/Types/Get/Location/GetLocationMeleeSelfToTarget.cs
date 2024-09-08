using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Title("Self to Target Location")]
    [Category("Melee/Self to Target Location")]

    [Image(typeof(IconTarget), ColorTheme.Type.Red)]
    [Description("The position and rotation of Self towards the Target")]

    [Serializable]
    public class GetLocationMeleeSelfToTarget : TGetLocationTrackLocation
    {
        protected override GameObject GetFrom(Args args)
        {
            return args.Self;
        }

        protected override GameObject GetTo(Args args)
        {
            return args.Target;
        }
        
        public static PropertyGetLocation Create => new PropertyGetLocation(
            new GetLocationMeleeSelfToTarget()
        );
        
        public override string String => "Self to Target";
    }
}