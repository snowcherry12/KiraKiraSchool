using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Title("Last Shot Charge")]
    [Category("Shooter/Shots/Last Shot Charge")]
    
    [Image(typeof(IconShoot), ColorTheme.Type.Yellow)]
    [Description("The charge (between 0 and 1) of the last shot taken")]
    
    [Serializable]
    public class GetDecimalLastShotCharge : PropertyTypeGetDecimal
    {
        public override double Get(Args args) => ShotData.LastChargeRatio;
        public override double Get(GameObject gameObject) => ShotData.LastChargeRatio;

        public override string String => "Last Shot Charge";
    }
}