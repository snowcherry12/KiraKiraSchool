using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Title("Last Shot Muzzle")]
    [Category("Shooter/Last ShotMuzzle")]
    
    [Image(typeof(IconPistol), ColorTheme.Type.Yellow)]
    [Description("The muzzle position of the last shot taken")]

    [Serializable]
    public class GetPositionShooterLastShooterPosition : PropertyTypeGetPosition
    {
        public override Vector3 Get(Args args) => ShotData.LastShooterPosition;
        public override Vector3 Get(GameObject gameObject) => ShotData.LastShooterPosition;

        public override string String => "Last Shot Muzzle";
    }
}