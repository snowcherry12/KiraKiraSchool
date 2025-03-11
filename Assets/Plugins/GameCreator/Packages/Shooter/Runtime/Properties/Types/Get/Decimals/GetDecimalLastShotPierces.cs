using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Title("Last Shot Pierces")]
    [Category("Shooter/Shots/Last Shot Pierces")]
    
    [Image(typeof(IconShoot), ColorTheme.Type.Yellow)]
    [Description("The number of objects pierced by the last projectile shot")]
    
    [Serializable]
    public class GetDecimalLastShotPierces : PropertyTypeGetDecimal
    {
        public override double Get(Args args) => ShotData.LastNumPierces;
        public override double Get(GameObject gameObject) => ShotData.LastNumPierces;

        public override string String => "Last Shot Pierces";
    }
}