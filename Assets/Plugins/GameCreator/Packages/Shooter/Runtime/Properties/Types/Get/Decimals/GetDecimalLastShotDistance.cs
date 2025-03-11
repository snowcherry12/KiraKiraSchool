using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Title("Last Shot Distance")]
    [Category("Shooter/Shots/Last Shot Distance")]
    
    [Image(typeof(IconShoot), ColorTheme.Type.Yellow)]
    [Description("The total distance traveled by the projectile of the last shot taken")]
    
    [Serializable]
    public class GetDecimalLastShotDistance : PropertyTypeGetDecimal
    {
        public override double Get(Args args) => ShotData.LastDistance;
        public override double Get(GameObject gameObject) => ShotData.LastDistance;

        public override string String => "Last Shot Distance";
    }
}