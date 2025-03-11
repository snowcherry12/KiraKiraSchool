using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Title("Wind Force")]
    [Category("Shooter/Wind/Wind Force")]
    
    [Image(typeof(IconWind), ColorTheme.Type.Green)]
    [Description("The direction of the Wind with its Force value")]

    [Serializable]
    public class GetDirectionWindForce : PropertyTypeGetDirection
    {
        public override Vector3 Get(Args args) => WindManager.Instance.Wind;
        public override Vector3 Get(GameObject gameObject) => WindManager.Instance.Wind;

        public static PropertyGetDirection Create => new PropertyGetDirection(
            new GetDirectionWindForce()
        );

        public override string String => "Wind Force";
    }
}