using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Title("Wind Direction")]
    [Category("Shooter/Wind/Wind Direction")]
    
    [Image(typeof(IconWind), ColorTheme.Type.Green, typeof(OverlayDot))]
    [Description("The normalized direction of the Wind")]

    [Serializable]
    public class GetDirectionWindDirection : PropertyTypeGetDirection
    {
        public override Vector3 Get(Args args) => WindManager.Instance.Direction;
        public override Vector3 Get(GameObject gameObject) => WindManager.Instance.Direction;

        public static PropertyGetDirection Create => new PropertyGetDirection(
            new GetDirectionWindDirection()
        );

        public override string String => "Wind Direction";
    }
}