using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Title("Wind")]
    [Category("Shooter/Wind/Wind")]
    
    [Image(typeof(IconWind), ColorTheme.Type.Green)]
    [Description("The magnitude of the current Wind")]

    [Keywords("Wind", "Drift", "Force", "Air", "Storm")]
    
    [Serializable]
    public class GetDecimalWind : PropertyTypeGetDecimal
    {
        public override double Get(Args args) => WindManager.Instance.Magnitude;
        public override double Get(GameObject gameObject) => WindManager.Instance.Magnitude;

        public static PropertyGetDecimal Create => new PropertyGetDecimal(
            new GetDecimalWind()
        );

        public override string String => "Wind";
    }
}