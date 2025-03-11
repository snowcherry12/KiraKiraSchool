using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Title("Last Shot Direction")]
    [Category("Shooter/Last Shot Direction")]
    
    [Image(typeof(IconPistol), ColorTheme.Type.Yellow, typeof(OverlayArrowRight))]
    [Description("The direction of the projectile of the last shot taken")]

    [Serializable]
    public class GetDirectionShooterLastShotDirection : PropertyTypeGetDirection
    {
        public override Vector3 Get(Args args) => ShotData.LastShooterDirection;
        public override Vector3 Get(GameObject gameObject) => ShotData.LastShooterDirection;

        public override string String => "Last Shot Direction";
    }
}